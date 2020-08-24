using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Awabot.UI.Helpers;
using Awabot.Bot.Structures;
using Awabot.UI.Forms;
using Awabot.Core.Structures;
using Awabot.Core.Helpers;
using System.IO;
using System.Drawing.Imaging;

namespace Awabot.Bot.Bot
{
    public class AwakeningRoutine
    {
        private ServerConfig _serverConfig { get; }
        private AwakeBotUserInterface _ui { get; }
        private BotConfiguration _botConfig { get; }
        private Thread _awakeningThread { get; set; }

        private bool _isRunning = false;

        public AwakeningRoutine(AwakeBotUserInterface ui, BotConfiguration botConfig, ServerConfig serverConfig)
        {
            _ui = ui;
            _botConfig = botConfig;
            _serverConfig = serverConfig;
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
            ThreadSafeControlHelper.EnableForm(_ui, false);

            _awakeningThread.Join();

            // The awakening thread has finished
            ThreadSafeControlHelper.EnableForm(_ui, true);
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
                awakeListSortedByGroup[i] = GetAllAwakesInGroup(awakeItemList, groupList[i]);
            }

            return awakeListSortedByGroup;
        }

        private void AwakeningLoopThread(List<AwakeItem> preferredAwakeItemList)
        {
            Queue<List<Awake>> latestAwakes = new Queue<List<Awake>>();

            try
            {
                LogHelper.AppendLog(_ui, "Bot has started");

                Win32.SetForegroundWindow(_botConfig.Process.MainWindowHandle);

                AwakeningResolver awakeResolver = new AwakeningResolver(_serverConfig);

                while (_isRunning)
                {
                    if (latestAwakes.Count >= 3)
                    {
                        // Check if the 3 awakes in the queue are all the same
                        var firstAwakes = latestAwakes.ElementAt(0);

                        bool anyNotEqual = false;

                        for (int i = 1; i < latestAwakes.Count; ++i)
                        {
                            var awakes = latestAwakes.ElementAt(i);

                            bool sameSize = awakes.Count == firstAwakes.Count;

                            if (!sameSize)
                            {
                                anyNotEqual = true;
                                break;
                            }

                            for (int j = 0; j < awakes.Count; ++j)
                            {
                                bool equalText = awakes[j].Text == firstAwakes[j].Text;
                                bool equalValue = awakes[j].Value == firstAwakes[j].Value;
                                if (!equalText || !equalValue)
                                {
                                    anyNotEqual = true;
                                    break;
                                }
                            }
                        }

                        if (!anyNotEqual)
                        {
                            LogHelper.AppendLog(_ui, "Same awake the last 3 times, stopping the bot.");
                            StopBot();
                            return;
                        }
                    }

                    var stopWatch = Stopwatch.StartNew();

                    MouseHelper.SetCursorPosition(new Point(_botConfig.ItemPosition.X + 3, _botConfig.ItemPosition.Y));

                    Thread.Sleep(50);

                    MouseHelper.SetCursorPosition(new Point(_botConfig.ItemPosition.X - 3, _botConfig.ItemPosition.Y));

                    Thread.Sleep(50);

                    // Hover over the item to check the awake
                    MouseHelper.SetCursorPosition(_botConfig.ItemPosition);

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

                    float newDPI = 300;

                    Size newSize = bmp.Size;

                    float inchesWidth = (float)bmp.Width / bmp.HorizontalResolution;
                    float inchesHeight = (float)bmp.Height / bmp.VerticalResolution;

                    newSize.Width = (int)(newDPI * inchesWidth);
                    newSize.Height = (int)(newDPI * inchesHeight);

                    bmp = awakeResolver.ResizeImage(bmp, newSize);
                    bmp.SetResolution(newDPI, newDPI);

                    string awakeText = awakeResolver.GetAwakening(bmp);

                    LogHelper.AppendLog(_ui, "Raw awake In-game text: \"" + awakeText + "\"");

                    List<Awake> itemAwakes = new AwakeningParser(_serverConfig, awakeText).GetCompletedAwakes();

                    if (latestAwakes.Count >= 3)
                    {
                        // Remove the object that was the first one in
                        latestAwakes.Dequeue();
                    }

                    latestAwakes.Enqueue(itemAwakes);

                    string awakeAchieved = "";

                    for (int i = 0; i < itemAwakes.Count; ++i)
                        awakeAchieved += itemAwakes[i].ToString() + (i == (itemAwakes.Count - 1) ? "" : " | ");

                    if (awakeAchieved.Length <= 0)
                    {
                        awakeAchieved = "No awake found\n";
                        LogHelper.AppendLog(_ui, awakeAchieved);
                    }

                    LogHelper.AppendLog(_ui, awakeAchieved);

                    bool shouldBreak = false;

                    if (_ui.CheckboxStopIfAwakeUnrecognized.Checked)
                    {
                        foreach (var awake in itemAwakes)
                        {
                            if (awake.TypeIndex == -1)
                            {
                                StopBot();

                                LogHelper.AppendLog(_ui, "An awake was not recognized, stopping the bot.");

                                shouldBreak = true;

                                break;
                            }
                        }
                    }

                    if (shouldBreak)
                        break;

                    var preferredAwakesList = ConvertAwakeItemListToAwakeList(preferredAwakeItemList);

                    // Go through each awake group and see if any of them meets the requirements
                    foreach (var preferredAwake in preferredAwakesList)
                    {
                        // Check if awake meets the requirements
                        if (AwakeMeetsRequirements(itemAwakes, preferredAwake))
                        {
                            LogHelper.AppendLog(_ui, "Preferred awake was successfully achieved");

                            stopWatch.Stop();

                            UpdateIterationTimeLabels(stopWatch.ElapsedMilliseconds);

                            shouldBreak = true;

                            break;
                        }
                    }

                    if (shouldBreak)
                        break;

                    LogHelper.AppendLog(_ui, "Preferred awake was not achieved");

                    // Doubleclick reversion scroll
                    MouseHelper.LeftClick(_botConfig.ReversionPosition);
                    MouseHelper.LeftClick(_botConfig.ReversionPosition);

                    const int ms = 200;

                    Thread.Sleep(ms);

                    // Click item with reversion
                    MouseHelper.LeftClick(_botConfig.ItemPosition);

                    // Wait until the reversion is done on the item
                    Thread.Sleep(_serverConfig.ScrollFinishDelay);

                    if (_ui.ComboBoxSupportAugmentation.Checked)
                    {
                        MouseHelper.LeftClick(_botConfig.AwakeScrollPosition);
                    }
                    else
                    {
                        // Doubleclick awake scroll
                        MouseHelper.LeftClick(_botConfig.AwakeScrollPosition);
                        MouseHelper.LeftClick(_botConfig.AwakeScrollPosition);
                    }

                    Thread.Sleep(ms);

                    // Click item with awake scroll
                    MouseHelper.LeftClick(_botConfig.ItemPosition);

                    Thread.Sleep(ms);

                    MouseHelper.SetCursorPosition(new Point(_botConfig.ItemPosition.X + 200, _botConfig.ItemPosition.Y + 200));

                    Thread.Sleep(ms);

                    MouseHelper.SetCursorPosition(_botConfig.ItemPosition);

                    // Wait until the awake is done on the item
                    Thread.Sleep(_serverConfig.ScrollFinishDelay);

                    bmp.Dispose();

                    stopWatch.Stop();

                    UpdateIterationTimeLabels(stopWatch.ElapsedMilliseconds);
                }

                StopBot();

                LogHelper.AppendLog(_ui, "Bot has finished");
            }
            catch (Exception ex)
            {
                StopBot();
                LogHelper.AppendLog(_ui, ex.ToString());
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
