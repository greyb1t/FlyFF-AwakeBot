using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FlyFF_AwakeBot.Utils;

namespace FlyFF_AwakeBot
{
    public class AwakeningRoutine
    {
        private ServerConfig _serverConfig { get; }
        private AwakeBotUserInterface _ui { get; }
        private BotConfig _botConfig { get; }
        private Thread _awakeningThread { get; set; }

        private bool _isRunning = false;

        public AwakeningRoutine(AwakeBotUserInterface ui, BotConfig botConfig, ServerConfig serverConfig)
        {
            _ui = ui;
            _botConfig = botConfig;
            _serverConfig = serverConfig;
        }

        private void AppendLog(string message)
        {
            string currentTime = DateTime.Now.ToString("[HH:mm:ss] ");

            ThreadSafeControlHelper.AppendTextBox(_ui.TextBoxLog, currentTime + message + Environment.NewLine);
        }

        public void StartBot(List<AwakeItem> preferredAwakes)
        {
            _isRunning = true;

            _awakeningThread = new Thread((awakes) => AwakeningLoopThread(preferredAwakes));
            _awakeningThread.Start(preferredAwakes);

            ThreadSafeControlHelper.ChangeControlText(_ui.ButtonStartBot, "Stop [END]");
        }

        public void StopBot()
        {
            _isRunning = false;
            ThreadSafeControlHelper.ChangeControlText(_ui.ButtonStartBot, "Start");
        }

        public void WaitAwakeningThreadFinish()
        {
            // Disable the main bot window while waiting for the awakening thread to finish
            _ui.Invoke(new Action(() =>
            {
                _ui.Enabled = false;
            }));

            _awakeningThread.Join();

            // The awakening thread has finished
            _ui.Invoke(new Action(() =>
            {
                _ui.Enabled = true;
            }));
        }

        private void UpdateIterationTimeLabels(long ms)
        {
            ThreadSafeControlHelper.ChangeControlText(_ui.LabelIterationTime, ms.ToString() + " ms");
            ThreadSafeControlHelper.ChangeControlText(_ui.LabelTotalTries, (Convert.ToInt32(_ui.LabelTotalTries.Text) + 1).ToString());
        }

        private void UpdateStepUi(PictureBox pictureBox, Label label, Bitmap bitmap)
        {
            ThreadSafeControlHelper.UpdatePictureBox(pictureBox, (Image)bitmap.Clone());

            ThreadSafeControlHelper.InvokeControl(label, () =>
            {
                label.Top = pictureBox.Bounds.Bottom;
            });
        }

        List<Awake> GetAllAwakesInGroup(List<AwakeItem> awakeItemList, int group)
        {
            List<Awake> awakes = new List<Awake>();

            foreach (var awakeItem in awakeItemList)
            {
                if (awakeItem.Group == group)
                    awakes.Add(awakeItem.Awake);
            }

            return awakes;
        }

        List<Awake>[] ConvertAwakeItemListToAwakeList(List<AwakeItem> awakeItemList)
        {
            // Read all the different groups
            HashSet<int> groups = new HashSet<int>();

            foreach (var awakeItem in awakeItemList)
            {
                groups.Add(awakeItem.Group);
            }

            List<int> groupList = groups.ToList();
            List<Awake>[] awakeListSortedByGroup = new List<Awake>[groups.Count];

            for (int i = 0; i < groupList.Count; ++i)
            {
                var awka = GetAllAwakesInGroup(awakeItemList, groupList[i]);
                awakeListSortedByGroup[i] = awka;
            }

            return awakeListSortedByGroup;
        }

        private void AwakeningLoopThread(List<AwakeItem> preferredAwakeItemList)
        {
            try
            {
                AppendLog("Bot has started");

                Win32.SetForegroundWindow(_botConfig.Process.Handle);

                using (AwakeningResolver awakeResolver = new AwakeningResolver(_serverConfig))
                {
                    while (_isRunning)
                    {
                        var stopWatch = Stopwatch.StartNew();

                        // Hover over the item to check the awake
                        MouseSimulator.SetCursorPosition(_botConfig.ItemPosition);

                        // Add specified delay to compensate for laggy server
                        int delayBeforeSnapshot = 0;

                        _ui.Invoke(new Action(() =>
                        {
                            delayBeforeSnapshot = _ui.TrackBarBeforeSnapshotMsDelay.Value;
                        }));

                        Thread.Sleep(delayBeforeSnapshot);

                        Bitmap bmp = awakeResolver.SnapshotRectangle(_botConfig.AwakeReadRectangle);

                        UpdateStepUi(_ui.PictureBoxDebug1, _ui.LabelStep1, bmp);

                        bmp = awakeResolver.DifferentiateAwakeText(bmp);

                        UpdateStepUi(_ui.PictureBoxDebug2, _ui.LabelStep2, bmp);

                        bmp = awakeResolver.CropBitmapSmart(bmp);

                        UpdateStepUi(_ui.PictureBoxDebug3, _ui.LabelStep3, bmp);

                        string awakeText = awakeResolver.GetAwakening(bmp);

                        AppendLog("Raw awake In-game text: \"" + awakeText + "\"");

                        List<Awake> itemAwakes = new AwakeningParser(_serverConfig, awakeText).GetCompletedAwakes();

                        string awakeAchieved = "";

                        for (int i = 0; i < itemAwakes.Count; ++i)
                            awakeAchieved += itemAwakes[i].ToString() + (i == (itemAwakes.Count - 1) ? "" : " | ");

                        if (awakeAchieved.Length <= 0)
                        {
                            awakeAchieved = "No awake found\n";
                            AppendLog(awakeAchieved);
                        }

                        AppendLog(awakeAchieved);

                        var preferredAwakesList = ConvertAwakeItemListToAwakeList(preferredAwakeItemList);

                        bool shouldBreak = false;

                        // Go through each awake group and see if any of them meets the requirements
                        foreach (var preferredAwake in preferredAwakesList)
                        {
                            // Check if awake meets the requirements
                            if (AwakeMeetsRequirements(itemAwakes, preferredAwake))
                            {
                                AppendLog("Preferred awake was successfully achieved");

                                stopWatch.Stop();

                                UpdateIterationTimeLabels(stopWatch.ElapsedMilliseconds);

                                shouldBreak = true;

                                break;
                            }
                        }

                        if (shouldBreak)
                            break;

                        AppendLog("Preferred awake was not achieved");

                        // Doubleclick reversion scroll
                        MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.ReversionPosition);
                        MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.ReversionPosition);

                        int ms = 200;

                        Thread.Sleep(ms);

                        // Click item with reversion
                        MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.ItemPosition);

                        // Wait until the reversion is done on the item
                        Thread.Sleep(_serverConfig.ScrollFinishDelay);

                        if (_ui.ComboBoxSupportAugmentation.Checked)
                        {
                            MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.AwakeScrollPosition);
                        }
                        else
                        {
                            // Doubleclick awake scroll
                            MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.AwakeScrollPosition);
                            MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.AwakeScrollPosition);
                        }

                        Thread.Sleep(ms);

                        // Click item with awake scroll
                        MouseSimulator.LeftClick(_botConfig.Process.Handle, _botConfig.ItemPosition);

                        Thread.Sleep(ms);

                        MouseSimulator.SetCursorPosition(new Point(_botConfig.ItemPosition.X + 200, _botConfig.ItemPosition.Y + 200));

                        Thread.Sleep(ms);

                        MouseSimulator.SetCursorPosition(_botConfig.ItemPosition);


                        // Wait until the awake is done on the item
                        Thread.Sleep(_serverConfig.ScrollFinishDelay);

                        bmp.Dispose();

                        stopWatch.Stop();

                        UpdateIterationTimeLabels(stopWatch.ElapsedMilliseconds);
                    }
                }

                StopBot();

                AppendLog("Bot has finished");
            }
            catch (Exception ex)
            {
                StopBot();
                AppendLog(ex.ToString());
            }
        }

        private List<Awake> CombineEqualAwakes(List<Awake> awakes)
        {
            var combinedAwakes = new List<Awake>();

            // Make a deep copy to avoid touching the original list
            awakes.ForEach((item) =>
            {
                combinedAwakes.Add((Awake)item.Clone());
            });

            for (int i = 0; i < combinedAwakes.Count; ++i)
            {
                for (int j = 0; j < combinedAwakes.Count; ++j)
                {
                    if (i >= combinedAwakes.Count)
                        return combinedAwakes;

                    // Does the awake type match the i awake?
                    if (combinedAwakes[i].TypeIndex == combinedAwakes[j].TypeIndex && i != j)
                    {
                        combinedAwakes[i].Value += combinedAwakes[j].Value;
                        combinedAwakes.RemoveAt(j);
                    }
                }
            }

            return combinedAwakes;
        }

        private bool AwakeMeetsRequirements(List<Awake> itemAwakes, List<Awake> preferredAwakes)
        {
            var combinedItemAwakes = CombineEqualAwakes(itemAwakes);
            var combinedPreferredAwakes = CombineEqualAwakes(preferredAwakes);

            bool[] preferredAwakeRequirements = new bool[combinedPreferredAwakes.Count];

            for (int i = 0; i < combinedPreferredAwakes.Count; ++i)
            {
                for (int j = 0; j < combinedItemAwakes.Count; ++j)
                {
                    if (combinedPreferredAwakes[i].TypeIndex == combinedItemAwakes[j].TypeIndex &&
                        combinedItemAwakes[j].Value >= combinedPreferredAwakes[i].Value)
                    {
                        preferredAwakeRequirements[i] = true;
                    }
                }
            }

            foreach (bool req in preferredAwakeRequirements)
            {
                if (!req)
                    return false;
            }

            return true;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }
    }
}
