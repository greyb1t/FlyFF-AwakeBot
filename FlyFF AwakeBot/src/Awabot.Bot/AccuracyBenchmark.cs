using Awabot.Bot.Bot;
using Awabot.Bot.Structures;
using Awabot.Core.Helpers;
using Awabot.Core.Structures;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Awabot.Bot
{
    public static class AccuracyBenchmark
    {
        private class BenchmarkItem
        {
            public List<Awake> Awakes { get; set; }
            public string AwakeImageFilename { get; set; }
        }

        private static List<BenchmarkItem> _awakeBenchmarkItems = new List<BenchmarkItem>() {
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\107362.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\111807.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased DEF",
                        Text = "Increased DEF +20%",
                        TypeIndex = 10,
                        Value = 20,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +23",
                        TypeIndex = 0,
                        Value = 23,
                    },
                },
                AwakeImageFilename = "configs\\images\\116616.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\13402.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\134838.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +9%",
                        TypeIndex = 14,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +7%",
                        TypeIndex = 6,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\139277.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\142289.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +5%",
                        TypeIndex = 9,
                        Value = 5,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +5%",
                        TypeIndex = 5,
                        Value = 5,
                    },
                },
                AwakeImageFilename = "configs\\images\\16395.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +33%",
                        TypeIndex = 4,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                },
                AwakeImageFilename = "configs\\images\\179490.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +17%",
                        TypeIndex = 4,
                        Value = 17,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +20%",
                        TypeIndex = 4,
                        Value = 20,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +22%",
                        TypeIndex = 4,
                        Value = 22,
                    },
                },
                AwakeImageFilename = "configs\\images\\180439.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\183233.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +6%",
                        TypeIndex = 9,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "Max MP",
                        Text = "Max. MP +400",
                        TypeIndex = 18,
                        Value = 400,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\183729.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +8%",
                        TypeIndex = 6,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\19190.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\204141.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\209935.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +31",
                        TypeIndex = 1,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\210491.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\225805.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "INT",
                        Text = "INT +19",
                        TypeIndex = 0,
                        Value = 19,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +17%",
                        TypeIndex = 4,
                        Value = 17,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +15",
                        TypeIndex = 3,
                        Value = 15,
                    },
                },
                AwakeImageFilename = "configs\\images\\22969.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +33%",
                        TypeIndex = 4,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\233740.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +23",
                        TypeIndex = 2,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\237393.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +33",
                        TypeIndex = 1,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +28",
                        TypeIndex = 1,
                        Value = 28,
                    },
                },
                AwakeImageFilename = "configs\\images\\242352.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +6%",
                        TypeIndex = 9,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                },
                AwakeImageFilename = "configs\\images\\245823.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +7%",
                        TypeIndex = 6,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\248122.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\248352.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +19%",
                        TypeIndex = 13,
                        Value = 19,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\250868.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "INT",
                        Text = "INT +33",
                        TypeIndex = 0,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\278603.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "INT",
                        Text = "INT +25",
                        TypeIndex = 0,
                        Value = 25,
                    },
                    new Awake() {
                        Name = "Increased DEF",
                        Text = "Increased DEF +20%",
                        TypeIndex = 10,
                        Value = 20,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +8%",
                        TypeIndex = 6,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\284385.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +24",
                        TypeIndex = 0,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\288606.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\300751.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "Max MP",
                        Text = "Max. MP +500",
                        TypeIndex = 18,
                        Value = 500,
                    },
                },
                AwakeImageFilename = "configs\\images\\302347.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STA",
                        Text = "STA +28",
                        TypeIndex = 3,
                        Value = 28,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\305631.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +11%",
                        TypeIndex = 7,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\307984.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\319765.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +15%",
                        TypeIndex = 4,
                        Value = 15,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +5%",
                        TypeIndex = 9,
                        Value = 5,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +8%",
                        TypeIndex = 14,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\33220.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +9%",
                        TypeIndex = 14,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +15%",
                        TypeIndex = 14,
                        Value = 15,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +7%",
                        TypeIndex = 14,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\33614.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +23",
                        TypeIndex = 2,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "Critical Chance",
                        Text = "Critical Chance +10%",
                        TypeIndex = 12,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\343316.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\343709.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\360063.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\365385.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\367937.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\36892.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +6%",
                        TypeIndex = 9,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\36898.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +5%",
                        TypeIndex = 6,
                        Value = 5,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +15%",
                        TypeIndex = 4,
                        Value = 15,
                    },
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +17%",
                        TypeIndex = 13,
                        Value = 17,
                    },
                },
                AwakeImageFilename = "configs\\images\\377989.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +23",
                        TypeIndex = 2,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\385645.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\385911.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\405156.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +31.",
                        TypeIndex = 3,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\407980.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\410079.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +5%",
                        TypeIndex = 5,
                        Value = 5,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\41071.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +8%",
                        TypeIndex = 6,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\419671.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\42014.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\420258.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +35",
                        TypeIndex = 0,
                        Value = 35,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\420566.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +9%",
                        TypeIndex = 7,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +31.",
                        TypeIndex = 3,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\423076.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +35",
                        TypeIndex = 3,
                        Value = 35,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\469155.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                },
                AwakeImageFilename = "configs\\images\\481281.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +8%",
                        TypeIndex = 14,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Max MP",
                        Text = "Max. MP +350",
                        TypeIndex = 18,
                        Value = 350,
                    },
                },
                AwakeImageFilename = "configs\\images\\481335.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +31.",
                        TypeIndex = 3,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\485224.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\488272.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\491284.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +15%",
                        TypeIndex = 13,
                        Value = 15,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\491514.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "INT",
                        Text = "INT +35",
                        TypeIndex = 0,
                        Value = 35,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +33",
                        TypeIndex = 0,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\497380.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "EXP",
                        Text = "EXP +10%",
                        TypeIndex = 15,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\498935.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +13%",
                        TypeIndex = 5,
                        Value = 13,
                    },
                },
                AwakeImageFilename = "configs\\images\\504070.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STA",
                        Text = "STA +28",
                        TypeIndex = 3,
                        Value = 28,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\505914.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\514817.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +26%",
                        TypeIndex = 4,
                        Value = 26,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\518972.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\522189.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +13%",
                        TypeIndex = 13,
                        Value = 13,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +9%",
                        TypeIndex = 6,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +8%",
                        TypeIndex = 6,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\530790.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                },
                AwakeImageFilename = "configs\\images\\533566.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +10%",
                        TypeIndex = 7,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\546665.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                },
                AwakeImageFilename = "configs\\images\\553910.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +11%",
                        TypeIndex = 7,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +8%",
                        TypeIndex = 7,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\554068.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +10%",
                        TypeIndex = 7,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +7%",
                        TypeIndex = 7,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\567125.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\574105.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +22%",
                        TypeIndex = 4,
                        Value = 22,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +26%",
                        TypeIndex = 4,
                        Value = 26,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\596404.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\599917.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +25",
                        TypeIndex = 3,
                        Value = 25,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\600577.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +6%",
                        TypeIndex = 9,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\604127.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STA",
                        Text = "STA +19",
                        TypeIndex = 3,
                        Value = 19,
                    },
                },
                AwakeImageFilename = "configs\\images\\619954.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\62493.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\625536.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\627139.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\631578.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\654155.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +23",
                        TypeIndex = 0,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +6%",
                        TypeIndex = 7,
                        Value = 6,
                    },
                },
                AwakeImageFilename = "configs\\images\\65898.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +31",
                        TypeIndex = 2,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +17%",
                        TypeIndex = 13,
                        Value = 17,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\671119.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +31",
                        TypeIndex = 2,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                },
                AwakeImageFilename = "configs\\images\\676472.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STA",
                        Text = "STA +24",
                        TypeIndex = 3,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +31.",
                        TypeIndex = 3,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +24",
                        TypeIndex = 3,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\686771.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "Attack Speed",
                        Text = "Attack Speed +19%",
                        TypeIndex = 13,
                        Value = 19,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\689445.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +28",
                        TypeIndex = 2,
                        Value = 28,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +31.",
                        TypeIndex = 2,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\689941.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STA",
                        Text = "STA +19",
                        TypeIndex = 3,
                        Value = 19,
                    },
                    new Awake() {
                        Name = "Speed",
                        Text = "Speed +8%",
                        TypeIndex = 16,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\704147.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "INT",
                        Text = "INT +35",
                        TypeIndex = 0,
                        Value = 35,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +33",
                        TypeIndex = 0,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\719001.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\720464.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\736008.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased MP",
                        Text = "Increased MP +11%",
                        TypeIndex = 7,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +12%",
                        TypeIndex = 6,
                        Value = 12,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\739522.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +8%",
                        TypeIndex = 6,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +10%",
                        TypeIndex = 6,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\740465.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\742624.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +31",
                        TypeIndex = 1,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\760060.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +33",
                        TypeIndex = 1,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +12%",
                        TypeIndex = 5,
                        Value = 12,
                    },
                },
                AwakeImageFilename = "configs\\images\\766320.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Max MP",
                        Text = "Max. MP +500",
                        TypeIndex = 18,
                        Value = 500,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\76929.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Critical Chance",
                        Text = "Critical Chance +20%",
                        TypeIndex = 12,
                        Value = 20,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +17",
                        TypeIndex = 0,
                        Value = 17,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                },
                AwakeImageFilename = "configs\\images\\769827.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\78078.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +10%",
                        TypeIndex = 14,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Max MP",
                        Text = "Max. MP +500",
                        TypeIndex = 18,
                        Value = 500,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +15%",
                        TypeIndex = 4,
                        Value = 15,
                    },
                },
                AwakeImageFilename = "configs\\images\\780986.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "INT",
                        Text = "INT +33",
                        TypeIndex = 0,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                },
                AwakeImageFilename = "configs\\images\\788165.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                },
                AwakeImageFilename = "configs\\images\\797926.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\81586.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +7%",
                        TypeIndex = 9,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\819118.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\820025.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "STR",
                        Text = "STR +27",
                        TypeIndex = 2,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +6%",
                        TypeIndex = 5,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +7%",
                        TypeIndex = 5,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\855557.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +31",
                        TypeIndex = 1,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +33",
                        TypeIndex = 1,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +31",
                        TypeIndex = 1,
                        Value = 31,
                    },
                },
                AwakeImageFilename = "configs\\images\\865893.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +8%",
                        TypeIndex = 5,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +10%",
                        TypeIndex = 14,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\879609.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +7%",
                        TypeIndex = 6,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +6%",
                        TypeIndex = 9,
                        Value = 6,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +19",
                        TypeIndex = 1,
                        Value = 19,
                    },
                },
                AwakeImageFilename = "configs\\images\\885367.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +24%",
                        TypeIndex = 4,
                        Value = 24,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\894512.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Critical Chance",
                        Text = "Critical Chance +7%",
                        TypeIndex = 12,
                        Value = 7,
                    },
                    new Awake() {
                        Name = "DEX",
                        Text = "DEX +19",
                        TypeIndex = 1,
                        Value = 19,
                    },
                    new Awake() {
                        Name = "Attack",
                        Text = "Attack +95",
                        TypeIndex = 8,
                        Value = 95,
                    },
                },
                AwakeImageFilename = "configs\\images\\895135.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +21%",
                        TypeIndex = 4,
                        Value = 21,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\900789.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +9%",
                        TypeIndex = 14,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +15%",
                        TypeIndex = 14,
                        Value = 15,
                    },
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +7%",
                        TypeIndex = 14,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\90374.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +23%",
                        TypeIndex = 4,
                        Value = 23,
                    },
                },
                AwakeImageFilename = "configs\\images\\917542.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +32",
                        TypeIndex = 2,
                        Value = 32,
                    },
                },
                AwakeImageFilename = "configs\\images\\942587.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +11%",
                        TypeIndex = 6,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +10%",
                        TypeIndex = 5,
                        Value = 10,
                    },
                },
                AwakeImageFilename = "configs\\images\\943609.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +8%",
                        TypeIndex = 9,
                        Value = 8,
                    },
                },
                AwakeImageFilename = "configs\\images\\948973.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +11%",
                        TypeIndex = 9,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\953086.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "DCT",
                        Text = "Decreased Casting Time +9%",
                        TypeIndex = 14,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "STA",
                        Text = "STA +33",
                        TypeIndex = 3,
                        Value = 33,
                    },
                    new Awake() {
                        Name = "Increased HP",
                        Text = "Increased HP +7%",
                        TypeIndex = 6,
                        Value = 7,
                    },
                },
                AwakeImageFilename = "configs\\images\\953763.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +31%",
                        TypeIndex = 4,
                        Value = 31,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "STR",
                        Text = "STR +33",
                        TypeIndex = 2,
                        Value = 33,
                    },
                },
                AwakeImageFilename = "configs\\images\\955215.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +9%",
                        TypeIndex = 5,
                        Value = 9,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +10%",
                        TypeIndex = 9,
                        Value = 10,
                    },
                    new Awake() {
                        Name = "PvE Damage",
                        Text = "PvE Damage Increase +9%",
                        TypeIndex = 9,
                        Value = 9,
                    },
                },
                AwakeImageFilename = "configs\\images\\962666.bmp",
            },
            new BenchmarkItem {
                Awakes = new List < Awake > {
                    new Awake() {
                        Name = "Increased Attack",
                        Text = "Increased Attack +11%",
                        TypeIndex = 5,
                        Value = 11,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +27%",
                        TypeIndex = 4,
                        Value = 27,
                    },
                    new Awake() {
                        Name = "ADOCH",
                        Text = "ADOCH +29%",
                        TypeIndex = 4,
                        Value = 29,
                    },
                },
                AwakeImageFilename = "configs\\images\\973165.bmp",
            },
        };

        public static void BeginBenchmark()
        {
            ServerConfig serverConfig = ServerConfigManager.ReadConfig("Insanity Flyff");

            AwakeningResolver awakeResolver = new AwakeningResolver(serverConfig);

            string output = string.Empty;

            var files = Directory.GetFiles("configs\\images\\");
            foreach (var file in files)
            {
                var bmp = (Bitmap)Image.FromFile(file);

                bmp = awakeResolver.DifferentiateAwakeText(bmp);

                bmp = awakeResolver.CropBitmapSmart(bmp);

                string awakeString = awakeResolver.GetAwakening(bmp);

                AwakeningParser parser = new AwakeningParser(serverConfig, awakeString);

                var awakes = parser.GetCompletedAwakes();
                List<Awake> correctAwakes = null;

                foreach (var benchmarkAwake in _awakeBenchmarkItems)
                {
                    if (benchmarkAwake.AwakeImageFilename == file)
                    {
                        correctAwakes = benchmarkAwake.Awakes;
                    }
                }

                if (correctAwakes == null)
                {
                    ErrorDisplayer.Error("benchmark image not found");
                    return;
                }

                if (awakes.Count != correctAwakes.Count)
                {
                    ErrorDisplayer.Error("bad");
                    return;
                }

                for (int i = 0; i < awakes.Count; ++i)
                {
                    bool areEqual = awakes[i].Name == correctAwakes[i].Name && awakes[i].Value == correctAwakes[i].Value;
                    if (!areEqual)
                    {
                        ErrorDisplayer.Error("not equal, something has been read incorrectly");
                    }
                }

                bmp.Dispose();
            }

            ErrorDisplayer.Info("done");
        }
    }
}
