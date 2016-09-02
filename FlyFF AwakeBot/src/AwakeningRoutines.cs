using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FlyFF_AwakeBot.src {
    class AwakeningRoutines {
        private Point ItemPosition { get; set; }
        private Point AwakeScrollPosition { get; set; }
        private Point ReversionPosition { get; set; }
        private Rectangle InventoryRectangle { get; set; }
        private ServerConfigManager ConfigManager { get; }

        private AwakeBotUserInterface Ui { get; }
        private List<Awake> PreferredAwakes;

        public AwakeningRoutines(AwakeBotUserInterface ui, Point itemPosition, Point awakeScollPosition, Point reversionPosition, Rectangle inventoryRectangle, ServerConfigManager awakeManager) {
            Ui = ui;

            ItemPosition = itemPosition;
            AwakeScrollPosition = awakeScollPosition;
            ReversionPosition = reversionPosition;
            InventoryRectangle = inventoryRectangle;

            ConfigManager = awakeManager;
        }

        /// <summary>
        /// Changes the .Text value of a control from any thread.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public void CrossThreadChangeControlText(Control control, string text) {
            control.Parent.Invoke(new Action(() => {
                control.Text = text;
            }));
        }

        /// <summary>
        /// Appends text to the log box from any thread.
        /// </summary>
        /// <param name="message"></param>
        public void CrossThreadAppendLog(string message) {
            string currentTime = DateTime.Now.ToString("[HH:mm:ss] ");

            Ui.Invoke(new Action(() => {
                Ui.tbLog.AppendText(currentTime + message);
            }));
        }

        /// <summary>
        /// Turn the bot on or off.
        /// </summary>
        public void ToggleBotStatus() {
            if (IsStopped()) {
                PreferredAwakes = AwakeningParser.GetPreferredAwakes(Ui.lviAwakes);

                Thread panicThread = new Thread(() => PanicKeyThread());
                panicThread.Start();

                Thread awakeThread = new Thread((awakes) => AwakeningLoopThread(PreferredAwakes));
                awakeThread.Start(PreferredAwakes);

                CrossThreadChangeControlText(Ui.btnStartBot, "Stop");
            }
            else {
                CrossThreadChangeControlText(Ui.btnStartBot, "Start");
            }
        }

        /// <summary>
        /// Executes the bot's awakening routine.
        /// </summary>
        /// <param name="preferredAwakes"></param>
        private void AwakeningLoopThread(List<Awake> preferredAwakes) {
            CrossThreadAppendLog("Bot has started\n");

            Win32API.SetForegroundWindow(Ui.Process.Handle);

            while (true) {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // Hover over the item to check the awake
                BotHelper.SetCursorPosition(ItemPosition);

                using (AwakeningResolver awakeResolver = new AwakeningResolver(ConfigManager)) {

                    if (IsStopped())
                        break;

                    // Add specified delay to compensate for laggy server
                    int msValue = 0;

                    Ui.Invoke(new Action(() => {
                        msValue = Ui.trackBarMsDelay.Value;
                    }));

                    Thread.Sleep(msValue);

                    Bitmap bmp = awakeResolver.SnapshotRectangle(InventoryRectangle);
                    bmp = awakeResolver.DifferentiateAwakeText(bmp);
                    bmp = awakeResolver.IncreaseBitmapSize(bmp, 300);
                    string awakeText = awakeResolver.GetAwakening(bmp);

                    AwakeningParser awakeParser = new AwakeningParser(Ui, ConfigManager, awakeText);
                    List<Awake> itemAwakes = awakeParser.GetCompletedAwakes();

                    string awakeAchieved = "";

                    for (int i = 0; i < itemAwakes.Count; ++i)
                        awakeAchieved += itemAwakes[i].ToString() + (i == (itemAwakes.Count - 1) ? "" : " | ");

                    if (awakeAchieved.Length <= 0) {
                        awakeAchieved = "No awake found\n";
                        CrossThreadAppendLog(awakeAchieved + "\n");
                    }

                    CrossThreadAppendLog(awakeAchieved + "\n");

                    // Check if awake meets the requirements
                    if (AwakeMeetsRequirements(itemAwakes, preferredAwakes)) {

                        CrossThreadAppendLog("Preferred awake was successfully achieved\n");

                        stopWatch.Stop();
                        CrossThreadChangeControlText(Ui.lblIterationTime, stopWatch.ElapsedMilliseconds.ToString() + " ms");
                        CrossThreadChangeControlText(Ui.lblTotalTries, (Convert.ToInt32(Ui.lblTotalTries.Text) + 1).ToString());

                        break;
                    }

                    CrossThreadAppendLog("Preferred awake was not achieved\n");

                    // Doubleclick reversion scroll
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, ReversionPosition);
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, ReversionPosition);

                    int ms = 200;

                    Thread.Sleep(ms);

                    // Click item with reversion
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, ItemPosition);

                    // Wait until the reversion is done on the item
                    Thread.Sleep(ConfigManager.ScrollDelay);

                    // Doubleclick awake scroll
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, AwakeScrollPosition);
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, AwakeScrollPosition);

                    Thread.Sleep(ms);

                    // Click item with awake scroll
                    BotHelper.SimulateMouseClick(Ui.Process.Handle, ItemPosition);

                    //// Wait until the awake is done on the item
                    //Thread.Sleep(ConfigManager.ScrollDelay);
                    Thread.Sleep(ms);

                    BotHelper.SetCursorPosition(new Point(ItemPosition.X + 200, ItemPosition.Y + 200));
                    Thread.Sleep(ms);
                    BotHelper.SetCursorPosition(ItemPosition);

                    // Wait until the awake is done on the item
                    Thread.Sleep(ConfigManager.ScrollDelay);

                    bmp.Dispose();

                    if (IsStopped())
                        break;
                }

                stopWatch.Stop();
                CrossThreadChangeControlText(Ui.lblIterationTime, stopWatch.ElapsedMilliseconds.ToString() + " ms");
                CrossThreadChangeControlText(Ui.lblTotalTries, (Convert.ToInt32(Ui.lblTotalTries.Text) + 1).ToString());
            }

            CrossThreadChangeControlText(Ui.btnStartBot, "Start");

            CrossThreadAppendLog("Bot has finished\n");
        }

        /// <summary>
        /// Checks whether the END key is down to turn the bot off.
        /// </summary>
        private void PanicKeyThread() {
            CrossThreadAppendLog("Panic key thread started\n");

            while (true) {

                if (BotHelper.IsKeyDown(System.Windows.Forms.Keys.End))
                    ToggleBotStatus();

                if (IsStopped())
                    break;

                Thread.Sleep(20);
            }

            CrossThreadAppendLog("Panic key thread stopped\n");
        }

        /// <summary>
        /// If an awake contains multiple lines of the same type, it'll combine them into one.
        /// </summary>
        /// <param name="awakes"></param>
        /// <returns></returns>
        List<Awake> CombineEqualAwakes(List<Awake> awakes) {
            for (int i = 0; i < awakes.Count; ++i) {
                for (int j = 0; j < awakes.Count; ++j) {
                    if (awakes[i].TypeIndex == awakes[j].TypeIndex && i != j) {
                        awakes[i].Value += awakes[j].Value;
                        awakes.RemoveAt(j);
                    }
                }
            }

            return awakes;
        }

        /// <summary>
        /// Checks whether an awake meets the requirements.
        /// </summary>
        /// <param name="itemAwakes"></param>
        /// <param name="preferredAwakes"></param>
        /// <returns></returns>
        private bool AwakeMeetsRequirements(List<Awake> itemAwakes, List<Awake> preferredAwakes) {
            itemAwakes = CombineEqualAwakes(itemAwakes);
            preferredAwakes = CombineEqualAwakes(preferredAwakes);

            bool[] preferredAwakeRequirements = new bool[preferredAwakes.Count];

            for (int i = 0; i < preferredAwakes.Count; ++i) {
                
                for (int j = 0; j < itemAwakes.Count; ++j) {

                    if (preferredAwakes[i].TypeIndex == itemAwakes[j].TypeIndex && 
                        itemAwakes[j].Value >= preferredAwakes[i].Value) {
                        preferredAwakeRequirements[i] = true;
                    }

                }

            }

            foreach (bool req in preferredAwakeRequirements) {
                if (!req)
                    return false;
            }

            return true;
        }

        public bool IsStopped() {
            if (Ui.btnStartBot.Text == "Start")
                return true;
            return false;
        }
    }
}
