﻿using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpeditionListPlugin
{
    public class ExpeditionInfo
    {
        /// <summary>
        /// 駆逐艦
        /// </summary>
        public static readonly string DESTROYER = "(?<駆>駆逐艦)";

        /// <summary>
        /// 軽巡洋艦
        /// </summary>
        public static readonly string LIGHTCRUISER = "(?<軽>軽巡洋艦)";

        /// <summary>
        /// 重巡洋艦
        /// </summary>
        public static readonly string HEAVYCRUISER = "(?<重>重巡洋艦)";

        /// <summary>
        /// 航空戦艦
        /// </summary>
        public static readonly string AVIATIONBATTLESHIP = "(?<航戦>航空戦艦)";

        /// <summary>
        /// 潜水艦
        /// </summary>
        public static readonly string SUBMARINE = "(?<潜>潜水艦|潜水空母)";

        /// <summary>
        /// 練習巡洋艦
        /// </summary>
        public static readonly string TRAININGCRUISER = "(?<練>練習巡洋艦)";

        /// <summary>
        /// 水上機母艦
        /// </summary>
        public static readonly string SEAPLANETENDER = "(?<水母>水上機母艦)";

        /// <summary>
        /// 潜水母艦
        /// </summary>
        public static readonly string SUBMARINETENDER = "(?<潜母艦>潜水母艦)";

        /// <summary>
        /// 軽空母
        /// </summary>
        public static readonly string LIGHTAIRCRAFTCARRIER = "(?<軽空母>軽空母)";

        /// <summary>
        /// 空母
        /// </summary>
        public static readonly string AIRCRAFTCARRIER = "(?<空母>正規空母|装甲空母|軽空母|水上機母艦|護衛空母)";

        /// <summary>
        /// 海防艦
        /// </summary>
        public static readonly string ESCORT = "(?<海防>海防艦)";

        /// <summary>
        /// 護衛空母
        /// </summary>
        public static readonly string ESCORTECARRIER = "(?<護母>護衛空母)";

        /// <summary>
        /// 軽空母 護衛空母
        /// </summary>
        public static readonly string LIGHTESCORTECARRIER = "(?<軽母/護母>軽空母|護衛空母)";

        /// <summary>
        /// ドラム缶
        /// </summary>
        public static readonly string DRUMCANISTER = "ドラム缶(輸送用)";

        public string Area { get; set; }
        public string EName { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string Time => $"{(int)(TimeSpan.TotalMinutes / 60):00}:{(int)(TimeSpan.TotalMinutes % 60):00}";
        public bool isSuccess2 { get; set; } = false;
        public bool isSuccess3 { get; set; } = false;
        public bool isSuccess4 { get; set; } = false;
        public int Lv { get; set; }
        public int? SumLv { get; set; }
        public int ShipNum { get; set; }

        /// <summary>
        /// 必要艦種
        /// </summary>
        /// <value>
        /// 必要艦種名と数
        /// </value>
        public Dictionary<Tuple<string, int>[], Tuple<string, int>[]> RequireShipType { get; set; }
        public Dictionary<string, int> RequireItemNum { get; set; }
        public Dictionary<string, int> RequireItemShipNum { get; set; }
        public string FlagShipType { get; set; }
        public string FlagShipTypeText
        {
            get
            {
                if (null == FlagShipType)
                {
                    return string.Empty;
                }

                var regexp = new Regex("<(.+)>(.+)");
                var match = regexp.Match(FlagShipType);

                return match.Success ? match.Groups[1].ToString() : string.Empty;
            }
        }

        /// <summary>
        /// 必要艦種テキスト
        /// </summary>
        public string RequireShipTypeText => string.Join("/", new[] { RequireShipTypeTextInner, RequireSumShipTypeText }.Where(x => x != string.Empty));

        public string RequireShipTypeTextInner => RequireShipType == null ? string.Empty :
            string.Join(" ",
                RequireShipType.Select(x =>
                    string.Join("", x.Key.Select(y => $"{Regex.Replace(y.Item1, @".+<(.+)>.+", "$1")}{y.Item2}"))
                    + (x.Value == null ? string.Empty : string.Join("or", x.Value.Select(z => $"{Regex.Replace(z.Item1, @".+<(.+)>.+", "$1")}{z.Item2}")))));

        public string RequireDrum => (null == RequireItemNum || null == RequireItemShipNum) ? string.Empty : $"{RequireItemShipNum[DRUMCANISTER]}隻 {RequireItemNum[DRUMCANISTER]}個";

        /// <summary>
        /// 必要合算艦種
        /// </summary>
        public string[] RequireSumShipType { get; set; }

        /// <summary>
        /// 必要合算艦種数
        /// </summary>
        public int RequireSumShipTypeNum { get; set; }

        /// <summary>
        /// 必要合算艦種テキスト
        /// </summary>
        public string RequireSumShipTypeText => null == RequireSumShipType ? string.Empty : $"{string.Join(",", RequireSumShipType.Select(x => $"{Regex.Replace(x, @".+<(.+)>.+", "$1")}"))}合計{RequireSumShipTypeNum}";

        /// <summary>
        /// 合計火力値
        /// </summary>
        public int? SumFirepower { get; set; }

        /// <summary>
        /// 合計対空値
        /// </summary>
        public int? SumAA { get; set; }

        /// <summary>
        /// 合計対潜値
        /// </summary>
        public int? SumASW { get; set; }

        /// <summary>
        /// 合計索敵値
        /// </summary>
        public int? SumViewRange { get; set; }

        public static readonly string FIREPOWER = "火力";
        public static readonly string AA = "対空";
        public static readonly string ASW = "対潜";
        public static readonly string VIEWRANGE = "索敵";

        /// <summary>
        /// 第二～四艦隊パラメータ
        /// </summary>
        /// <value>
        /// 対空、対潜、索敵が要件を満たしているか。条件がない場合はnull
        /// </value>
        public Dictionary<int, Dictionary<string, bool?>> isParameter { get; set; } = new Dictionary<int, Dictionary<string, bool?>>();
        public Dictionary<int, Dictionary<string, string>> isParameterString { get; set; } = new Dictionary<int, Dictionary<string, string>>();

        /// <summary>
        /// 第二～四艦隊パラメータの要件を満たしているか
        /// </summary>
        /// <value>
        /// 対空、対潜、索敵がひとつでもfalseの場合はfalseそうでない場合はtrue
        /// </value>
        public Dictionary<int,bool> isParameterValid
        {
            get
            {
                for (var i = 2; i <= 4; i++)
                {
                    _isParameterValid[i] = isParameter[i].Any(p => p.Value == false) == true ? false : true;
                }

                return _isParameterValid;
            }
        }

        private Dictionary<int, bool> _isParameterValid = new Dictionary<int, bool>();

        /// <summary>
        /// 必須合計パラメータ
        /// </summary>
        public string RequireSumParamText
        {
            get
            {
                var buf = new string[] {
                    SumFirepower != null ? FIREPOWER + SumFirepower.ToString() : string.Empty,
                    SumAA != null ? AA + SumAA.ToString() : string.Empty,
                    SumASW != null ? ASW + SumASW.ToString() : string.Empty,
                    SumViewRange != null ? VIEWRANGE + SumViewRange.ToString() : string.Empty};

                return string.Join("/", buf.Where(s => s.Length > 0));
            }
        }

        public int? Fuel { get; set; }
        public int? Ammunition { get; set; }
        public int? Bauxite { get; set; }
        public int? Steel { get; set; }
        public int? InstantBuildMaterials { get; set; }
        public int? InstantRepairMaterials { get; set; }
        public int? DevelopmentMaterials { get; set; }

        /// <summary>
        /// 家具箱
        /// </summary>
        public string FurnitureBox { get; set; }

        public bool isFuelNull { get { return Fuel == null; } }
        public bool isAmmunitionNull { get { return Ammunition == null; } }
        public bool isSteelNull { get { return Steel == null; } }
        public bool isBauxiteNull { get { return Bauxite == null; } }

        public static ExpeditionInfo[] _ExpeditionTable = new ExpeditionInfo[]
        {
            new ExpeditionInfo {Area="鎮守", EName="1 練習航海", TimeSpan=new TimeSpan(00, 15, 00), Lv=1, ShipNum=2, Ammunition=30},
            new ExpeditionInfo {Area="鎮守", EName="2 長距離練習航海", TimeSpan=new TimeSpan(00, 30, 00), Lv=2, ShipNum=4, Ammunition=100, Steel=30, InstantRepairMaterials=1},
            new ExpeditionInfo {Area="鎮守", EName="3 警備任務", TimeSpan=new TimeSpan(00, 20, 00), Lv=3, ShipNum=3, Fuel=30, Ammunition=30, Steel=40},
            new ExpeditionInfo {Area="鎮守", EName="4 対潜警戒任務", TimeSpan=new TimeSpan(00, 50, 00), Lv=3, RequireShipType= new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                    { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null },
                    { new Tuple<string, int>[] {Tuple.Create(DESTROYER, 1), Tuple.Create(ESCORT, 3)}, null },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORT, 2)}, new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(TRAININGCRUISER, 1) } },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORTECARRIER, 1)}, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2), Tuple.Create(ESCORT, 2) } },
                }, ShipNum=3, Ammunition=70, InstantRepairMaterials=1, FurnitureBox="小1"},
            new ExpeditionInfo {Area="鎮守", EName="5 海上護衛任務", TimeSpan=new TimeSpan(01, 30, 00), Lv=3, RequireShipType = new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                    { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null },
                    { new Tuple<string, int>[] {Tuple.Create(DESTROYER, 1), Tuple.Create(ESCORT, 3)}, null },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORT, 2)}, new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(TRAININGCRUISER, 1) } },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORTECARRIER, 1)}, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2), Tuple.Create(ESCORT, 2) } },
                }, ShipNum=4, Fuel=200, Ammunition=200, Steel=20, Bauxite=20, },
            new ExpeditionInfo {Area="鎮守", EName="6 防空射撃演習", TimeSpan=new TimeSpan(00, 40, 00), Lv=4, ShipNum=4, Bauxite=80, FurnitureBox="小1"},
            new ExpeditionInfo {Area="鎮守", EName="7 観艦式予行", TimeSpan=new TimeSpan(01, 00, 00), Lv=5,ShipNum=6, Steel=50, Bauxite=30, InstantBuildMaterials=1},
            new ExpeditionInfo {Area="鎮守", EName="8 観艦式", TimeSpan=new TimeSpan(03, 00, 00), Lv=6, ShipNum=6, Fuel=50, Ammunition=100, Steel=50, Bauxite=50, InstantBuildMaterials=2, DevelopmentMaterials=1},

            new ExpeditionInfo {Area="鎮守Ex", EName="A1 兵站強化任務", TimeSpan=new TimeSpan(00, 25, 00), Lv=5, ShipNum=4, Fuel=45, Ammunition=45, RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 3 },
            new ExpeditionInfo {Area="鎮守Ex", EName="A2 海峡警備行動", TimeSpan=new TimeSpan(00, 55, 00), Lv=20, ShipNum=4, Fuel=70, Ammunition=40, Bauxite=10,DevelopmentMaterials=1, InstantRepairMaterials=1, RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 4 , SumAA=70, SumASW=180},
            new ExpeditionInfo {Area="鎮守Ex", EName="A3 長時間対潜警戒", TimeSpan=new TimeSpan(02, 15, 00), Lv=35, SumLv=185, ShipNum=5, Fuel=120, Steel=60,Bauxite=60, InstantRepairMaterials=1, DevelopmentMaterials=2, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1) }, null } }, RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum =3 ,SumASW=280 },
            
            new ExpeditionInfo {Area="鎮守Ex", EName="A4 旗-南西方面連絡線哨戒", TimeSpan=new TimeSpan(01, 50, 00), Lv=40, SumLv=200, ShipNum=5, 
                                SumFirepower=300, SumAA=200, SumASW=200, SumViewRange=120, 
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null } }
                                },
            new ExpeditionInfo {Area="鎮守Ex", EName="A5 旗-小笠原沖哨戒線", TimeSpan=new TimeSpan(03, 00, 00), Lv=45, SumLv=230, ShipNum=5,
                                SumFirepower=280, SumAA=220, SumASW=240, SumViewRange=150,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 3)}, null } }
                                },
            new ExpeditionInfo {Area="鎮守Ex", EName="A6 旗-小笠原沖戦闘哨戒", TimeSpan=new TimeSpan(03, 30, 00), Lv=55, SumLv=290, ShipNum=6,
                                SumFirepower=330, SumAA=300, SumASW=270, SumViewRange=180,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 3)}, null } }
                                },

            new ExpeditionInfo {Area="南西諸島", EName="9 タンカー護衛任務", TimeSpan=new TimeSpan(04, 00, 00), Lv=3, RequireShipType= new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                    { new Tuple<string, int>[] {Tuple.Create(LIGHTCRUISER, 1)}, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2)} },
                    { new Tuple<string, int>[] {Tuple.Create(DESTROYER, 1)}, new Tuple<string, int>[] { Tuple.Create(ESCORT, 3) } },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORT, 2)}, new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(TRAININGCRUISER, 1) } },
                    { new Tuple<string, int>[] {Tuple.Create(ESCORTECARRIER, 1)}, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2), Tuple.Create(ESCORT, 2) } },
            }, ShipNum=4, Fuel=350, InstantRepairMaterials=2, FurnitureBox="小1"},
            new ExpeditionInfo {Area="南西諸島", EName="10 強行偵察任務", TimeSpan=new TimeSpan(01, 30, 00), Lv=3, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 2) }, null } }, ShipNum=3, Ammunition=50, Bauxite=30, InstantRepairMaterials=1, InstantBuildMaterials=1},
            new ExpeditionInfo {Area="南西諸島", EName="11 ボーキサイト輸送任務", TimeSpan=new TimeSpan(05, 00, 00), Lv=6, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=4, Bauxite=250, InstantRepairMaterials=1, FurnitureBox="小1"},
            new ExpeditionInfo {Area="南西諸島", EName="12 資源輸送任務", TimeSpan=new TimeSpan(08, 00, 00), Lv=4, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=4, Ammunition=250, Steel=200, DevelopmentMaterials=1, FurnitureBox="中1"},
            new ExpeditionInfo {Area="南西諸島", EName="13 鼠輸送作戦", TimeSpan=new TimeSpan(04, 00, 00), Lv=5, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 4) }, null } }, ShipNum=6, Fuel=240, Ammunition=300, InstantRepairMaterials=2, FurnitureBox="小1"},
            new ExpeditionInfo {Area="南西諸島", EName="14 包囲陸戦隊撤収作戦", TimeSpan=new TimeSpan(06, 00, 00), Lv=6, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1) }, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 3) } } }, ShipNum=6, Ammunition=240, Steel=200, InstantRepairMaterials=1, DevelopmentMaterials=1},
            new ExpeditionInfo {Area="南西諸島", EName="15 囮機動部隊支援作戦", TimeSpan=new TimeSpan(12, 00, 00), Lv=8, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AIRCRAFTCARRIER, 2) }, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2) } } }, ShipNum=6, Steel=300, Bauxite=400, DevelopmentMaterials=1, FurnitureBox="大1"},
            new ExpeditionInfo {Area="南西諸島", EName="16 艦隊決戦援護作戦", TimeSpan=new TimeSpan(15, 00, 00), Lv=10, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1) }, new Tuple<string, int>[] { Tuple.Create(DESTROYER, 2) } } }, ShipNum=6, Fuel=500, Ammunition=500, Steel=200, Bauxite=200, InstantBuildMaterials=2, DevelopmentMaterials=2},

            new ExpeditionInfo {Area="南西諸島Ex", EName="B1 南西方面航空偵察作戦", TimeSpan=new TimeSpan(00, 35, 00), Lv=40,SumLv=150, ShipNum=6, Steel=10,Bauxite=30,InstantRepairMaterials=1,FurnitureBox="小1", RequireShipType =new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SEAPLANETENDER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2) }, null } }, SumAA=200,SumASW=200,SumViewRange=140 },
            new ExpeditionInfo {Area="南西諸島Ex", EName="B2 全-敵泊地強襲反撃作戦", TimeSpan=new TimeSpan(08, 40, 00), Lv=45, SumLv=220, ShipNum=6,
                                SumFirepower=360, SumAA=160, SumASW=160, SumViewRange=140,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 3)}, null } }
                                },
            new ExpeditionInfo {Area="南西諸島Ex", EName="B3 旗-南西諸島離島哨戒作戦", TimeSpan=new TimeSpan(02, 50, 00), Lv=50, SumLv=250, ShipNum=6,
                                SumFirepower=360, SumAA=160, SumASW=160, SumViewRange=140,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(SEAPLANETENDER, 1), Tuple.Create(LIGHTCRUISER, 1)}, null } },
                                RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 4
                                },
            new ExpeditionInfo {Area="南西諸島Ex", EName="B4 旗-南西諸島離島防衛作戦", TimeSpan=new TimeSpan(07, 30, 00), Lv=55, SumLv=300, ShipNum=6,
                                SumFirepower=500, SumAA=280, SumASW=280, SumViewRange=170,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 2), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2), Tuple.Create(SUBMARINE, 1)}, null } }
                                },
            new ExpeditionInfo {Area="南西諸島Ex", EName="B5 旗-南西諸島捜索撃滅戦", TimeSpan=new TimeSpan(06, 30, 00), Lv=60, SumLv=330, ShipNum=6,
                                SumFirepower=510, SumAA=400, SumASW=285, SumViewRange=385,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(SEAPLANETENDER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null } }
                                },
            new ExpeditionInfo {Area="南西諸島Ex", EName="B6 旗-精鋭水雷戦隊夜襲", TimeSpan=new TimeSpan(05, 50, 00), Lv=60, SumLv=330, ShipNum=6,
                                SumFirepower=410, SumAA=390, SumASW=410, SumViewRange=340,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 5)}, null } }, 
                                FlagShipType = LIGHTCRUISER
                                },

            new ExpeditionInfo {Area="北方", EName="17 敵地偵察作戦", TimeSpan=new TimeSpan(00, 45, 00), Lv=20, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 3) }, null } }, ShipNum=6, Fuel=70, Ammunition=70, Steel=50},
            new ExpeditionInfo {Area="北方", EName="18 航空機輸送作戦", TimeSpan=new TimeSpan(05, 00, 00), Lv=15, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AIRCRAFTCARRIER, 3), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=6, Steel=300, Bauxite=100, InstantRepairMaterials=1},
            new ExpeditionInfo {Area="北方", EName="19 北号作戦", TimeSpan=new TimeSpan(06, 00, 00), Lv=20, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AVIATIONBATTLESHIP, 2), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=6, Fuel=400, Steel=50, Bauxite=30, DevelopmentMaterials=1, FurnitureBox="小1"},
            new ExpeditionInfo {Area="北方", EName="20 潜水艦哨戒任務",  TimeSpan=new TimeSpan(02, 00, 00), RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 1), Tuple.Create(LIGHTCRUISER, 1) }, null } }, Lv=1, ShipNum=2, Steel=150, DevelopmentMaterials=1, FurnitureBox="小1"},
            new ExpeditionInfo {Area="北方", EName="21 北方鼠輸送作戦", TimeSpan=new TimeSpan(02, 20, 00), Lv=15, SumLv=30, RequireItemNum=new Dictionary<string, int> { { DRUMCANISTER, 3 } }, RequireItemShipNum=new Dictionary<string, int>() { { DRUMCANISTER, 3 } }, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 4) }, null } }, ShipNum=5, Fuel=320, Ammunition=270, FurnitureBox="小1"},
            new ExpeditionInfo {Area="北方", EName="22 艦隊演習", TimeSpan=new TimeSpan(03, 00, 00), Lv=30, SumLv=45, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=6, Ammunition=10},
            new ExpeditionInfo {Area="北方", EName="23 航空戦艦運用演習", TimeSpan=new TimeSpan(04, 00, 00), Lv=50, SumLv=200, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AVIATIONBATTLESHIP, 2), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=6, Ammunition=20, Bauxite=100},
            new ExpeditionInfo {Area="北方", EName="24 北方航路海上護衛", TimeSpan=new TimeSpan(08, 20, 00), Lv=50, SumLv=200, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 4) }, null } },ShipNum=6, Fuel=500, Bauxite=150, InstantRepairMaterials=1, DevelopmentMaterials=2, FlagShipType = LIGHTCRUISER},

            new ExpeditionInfo {Area="南西海域", EName="41 旗-ブルネイ泊地沖哨戒", TimeSpan=new TimeSpan(01, 00, 00), Lv=30, SumLv=100, ShipNum=3,
                                SumFirepower=60, SumAA=80, SumASW=210,
                                RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 3
                                },
            new ExpeditionInfo {Area="南西海域", EName="42 全-ミ船団護衛(一号船団)", TimeSpan=new TimeSpan(08, 00, 00), Lv=45, SumLv=200, ShipNum=4,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null } }
                                },
            new ExpeditionInfo {Area="南西海域", EName="43 旗-ミ船団護衛(二号船団)", TimeSpan=new TimeSpan(12, 00, 00), Lv=55, SumLv=300, ShipNum=6,
                                SumFirepower=500, SumAA=280, SumASW=280, SumViewRange=170,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(ESCORTECARRIER, 1), Tuple.Create(DESTROYER, 2)}, null },
                                   { new Tuple<string, int>[] { Tuple.Create(ESCORTECARRIER, 1), Tuple.Create(ESCORT, 2)}, null },
                                   { new Tuple<string, int>[] { Tuple.Create(LIGHTAIRCRAFTCARRIER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 4)}, null }}, 
                                FlagShipType = LIGHTESCORTECARRIER
                                },
            new ExpeditionInfo {Area="南西海域", EName="44 缶-航空装備輸送任務", TimeSpan=new TimeSpan(10, 00, 00), Lv=35, SumLv=210, ShipNum=6,
                                SumAA=200, SumASW=200, SumViewRange=150,
                                RequireItemNum=new Dictionary<string, int>() { { DRUMCANISTER, 8 } }, 
                                RequireItemShipNum=new Dictionary<string, int>() { { DRUMCANISTER, 3 } },
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(AIRCRAFTCARRIER, 1), Tuple.Create(SEAPLANETENDER, 1), Tuple.Create(LIGHTCRUISER, 2)}, null }},
                                RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 2
                                },
            new ExpeditionInfo {Area="南西海域", EName="45 旗-ボーキサイト船団護衛", TimeSpan=new TimeSpan(03, 20, 00), Lv=50, SumLv=240, ShipNum=5,
                                SumFirepower=500, SumAA=280, SumASW=280, SumViewRange=170,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(ESCORTECARRIER, 1)}, null },
                                   { new Tuple<string, int>[] { Tuple.Create(LIGHTAIRCRAFTCARRIER, 1)}, null }},
                                RequireSumShipType=new string[]{ DESTROYER , ESCORT }, RequireSumShipTypeNum = 4,
                                FlagShipType = LIGHTESCORTECARRIER
                                },
            new ExpeditionInfo {Area="南西海域", EName="46 旗-南西海域戦闘哨戒", TimeSpan=new TimeSpan(03, 30, 00), Lv=60, SumLv=300, ShipNum=5,
                                SumFirepower=350, SumAA=250, SumASW=220, SumViewRange=190,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 2), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2)}, null }}
                                },

            new ExpeditionInfo {Area="西方", EName="25 通商破壊作戦", TimeSpan=new TimeSpan(40, 00, 00), Lv=25, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 2), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=4, Fuel=900, Steel=500, },
            new ExpeditionInfo {Area="西方", EName="26 敵母港空襲作戦", TimeSpan=new TimeSpan(80, 00, 00), Lv=30, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AIRCRAFTCARRIER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 2) }, null } },ShipNum=4, Bauxite=900, InstantRepairMaterials=3},
            new ExpeditionInfo {Area="西方", EName="27 潜水艦通商破壊作戦", TimeSpan=new TimeSpan(20, 00, 00), Lv=1, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 2) }, null } }, ShipNum=2, Steel=800, DevelopmentMaterials=1, FurnitureBox="小2"},
            new ExpeditionInfo {Area="西方", EName="28 西方海域封鎖作戦", TimeSpan=new TimeSpan(25, 00, 00), Lv=30, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 3) }, null } }, ShipNum=3, Steel=900, Bauxite=350, DevelopmentMaterials=2, FurnitureBox="中2"},
            new ExpeditionInfo {Area="西方", EName="29 潜水艦派遣演習", TimeSpan=new TimeSpan(24, 00, 00), Lv=50, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 3) }, null } }, ShipNum=3, Bauxite=100, DevelopmentMaterials=1, FurnitureBox="小1"},
            new ExpeditionInfo {Area="西方", EName="30 潜水艦派遣作戦", TimeSpan=new TimeSpan(48, 00, 00), Lv=55, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 4) }, null } }, ShipNum=4, Bauxite=100, DevelopmentMaterials=3},
            new ExpeditionInfo {Area="西方", EName="31 海外艦との接触", TimeSpan=new TimeSpan(02, 00, 00), Lv=60,  SumLv=200, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINE, 4) }, null } }, ShipNum=4, Ammunition=30, FurnitureBox="小1"},
            new ExpeditionInfo {Area="西方", EName="32 遠洋練習航海", TimeSpan=new TimeSpan(24, 00, 00), Lv=5, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(TRAININGCRUISER, 1), Tuple.Create(DESTROYER, 2) }, null } }, ShipNum=3, Fuel=50, Ammunition=50, Steel=50, Bauxite=50, FurnitureBox="大1", FlagShipType = TRAININGCRUISER},

            new ExpeditionInfo {Area="西方Ex", EName="D1 旗-西方海域偵察作戦", TimeSpan=new TimeSpan(02, 00, 00), Lv=35, ShipNum=5,
                                SumAA=240, SumASW=240, SumViewRange=300,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(SEAPLANETENDER, 1), Tuple.Create(DESTROYER, 3)}, null }},
                                FlagShipType = SEAPLANETENDER
                                },
            new ExpeditionInfo {Area="西方Ex", EName="D2 旗-西方潜水艦作戦", TimeSpan=new TimeSpan(10, 00, 00), ShipNum=5,
                                SumFirepower=60, SumAA=80, SumASW=50, SumViewRange=70,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(SUBMARINETENDER, 1), Tuple.Create(SUBMARINE, 3)}, null }},
                                FlagShipType = SUBMARINETENDER
                                },
            new ExpeditionInfo {Area="西方Ex", EName="D3 旗-欧州方面友軍との接触", TimeSpan=new TimeSpan(12, 00, 00), Lv=65, SumLv=350, ShipNum=5,
                                SumFirepower=115, SumAA=90, SumASW=70, SumViewRange=95,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(SUBMARINETENDER, 1), Tuple.Create(SUBMARINE, 3)}, null }},
                                FlagShipType = SUBMARINETENDER
                                },

            new ExpeditionInfo {Area="南方", EName="35 MO作戦", TimeSpan=new TimeSpan(07, 00, 00), Lv=40, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(AIRCRAFTCARRIER, 2), Tuple.Create(HEAVYCRUISER, 1), Tuple.Create(DESTROYER, 1) }, null } }, ShipNum=6, Fuel=0, Ammunition=0, Steel=240, Bauxite=280, DevelopmentMaterials=1, FurnitureBox="小2"},
            new ExpeditionInfo {Area="南方", EName="36 水上機基地建設", TimeSpan=new TimeSpan(09, 00, 00), Lv=30, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SEAPLANETENDER, 2), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 1) }, null } }, ShipNum=6, Fuel=480, Ammunition=0, Steel=200, Bauxite=200, InstantRepairMaterials=1, FurnitureBox="中1"},
            new ExpeditionInfo {Area="南方", EName="37 東京急行", TimeSpan=new TimeSpan(02, 45, 00), Lv=50, SumLv=200, RequireItemNum=new Dictionary<string, int>() { { DRUMCANISTER, 4 } } , RequireItemShipNum=new Dictionary<string, int>() { { DRUMCANISTER, 3 } }, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 5) }, null } }, ShipNum=6, Fuel=0, Ammunition=380, Steel=270, Bauxite=0, FurnitureBox="小1"},
            new ExpeditionInfo {Area="南方", EName="38 東京急行(弐)", TimeSpan=new TimeSpan(02, 55, 00), Lv=65, SumLv=240, RequireItemNum=new Dictionary<string, int>() { { DRUMCANISTER, 8 } }, RequireItemShipNum=new Dictionary<string, int>() { { DRUMCANISTER, 4 } }, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(DESTROYER, 5) }, null } },ShipNum=6, Fuel=420, Ammunition=0, Steel=200, Bauxite=0, FurnitureBox="小1"},
            new ExpeditionInfo {Area="南方", EName="39 遠洋潜水艦作戦", TimeSpan=new TimeSpan(30, 00, 00), Lv=3, SumLv=180, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(SUBMARINETENDER, 1), Tuple.Create(SUBMARINE, 4), }, null } }, ShipNum=5, Fuel=0, Ammunition=0, Steel=300, Bauxite=0, InstantRepairMaterials=2, FurnitureBox="中1"},
            new ExpeditionInfo {Area="南方", EName="40 水上機前線輸送", TimeSpan=new TimeSpan(06, 50, 00), Lv=25, SumLv=150, RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> { { new Tuple<string, int>[] { Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(SEAPLANETENDER, 2), Tuple.Create(DESTROYER, 2) }, null } },ShipNum=6, Fuel=300, Ammunition=300, Steel=0, Bauxite=100, InstantRepairMaterials=1, FurnitureBox="小3", FlagShipType = LIGHTCRUISER},

            new ExpeditionInfo {Area="南方Ex", EName="E1 旗-ラバウル方面艦隊進出", TimeSpan=new TimeSpan(07, 30, 00), Lv=55, SumLv=290, ShipNum=6,
                                SumFirepower=450, SumAA=350, SumASW=330, SumViewRange=250,
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(HEAVYCRUISER, 1), Tuple.Create(LIGHTCRUISER, 1), Tuple.Create(DESTROYER, 3)}, null }},
                                FlagShipType = HEAVYCRUISER
                                },
            new ExpeditionInfo {Area="南方Ex", EName="E2 缶-強行鼠輸送作戦", TimeSpan=new TimeSpan(03, 05, 00), Lv=70, SumLv=320, ShipNum=5,
                                SumFirepower=280, SumAA=240, SumASW=200, SumViewRange=160,
                                RequireItemNum=new Dictionary<string, int>() { { DRUMCANISTER, 6 } },
                                RequireItemShipNum=new Dictionary<string, int>() { { DRUMCANISTER, 3 } },
                                RequireShipType=new Dictionary<Tuple<string, int>[], Tuple<string, int>[]> {
                                   { new Tuple<string, int>[] { Tuple.Create(DESTROYER, 5)}, null }}
                                },
        };

        public ExpeditionInfo()
        {
            for (var i = 2; i <= 4; i++)
            {
                isParameter[i] = new Dictionary<string, bool?>();
                isParameter[i].Add(FIREPOWER, null);
                isParameter[i].Add(AA, null);
                isParameter[i].Add(ASW, null);
                isParameter[i].Add(VIEWRANGE, null);

                isParameterString[i] = new Dictionary<string, string>();
                isParameterString[i].Add(FIREPOWER, "");
                isParameterString[i].Add(AA, "");
                isParameterString[i].Add(ASW, "");
                isParameterString[i].Add(VIEWRANGE, "");
            }
        }

        public void Check()
        {
            isSuccess2 = CheckAll(KanColleClient.Current.Homeport.Organization.Fleets[2]);
            isSuccess3 = CheckAll(KanColleClient.Current.Homeport.Organization.Fleets[3]);
            isSuccess4 = CheckAll(KanColleClient.Current.Homeport.Organization.Fleets[4]);

            CheckParam();
        }

        private void CheckParam()
        {
            for (var i = 2; i <= 4; i++)
            {
                var flags = new Dictionary<string, bool?>();
                flags[FIREPOWER] = SumFirepower != null ? SumFirepowerCheck(KanColleClient.Current.Homeport.Organization.Fleets[i]) : (bool?)null;
                flags[AA] = SumAA != null ? SumAACheck(KanColleClient.Current.Homeport.Organization.Fleets[i]) : (bool?)null;
                flags[ASW] = SumASW != null ? SumASWCheck(KanColleClient.Current.Homeport.Organization.Fleets[i]) : (bool?)null;
                flags[VIEWRANGE] = SumViewRange != null ? SumViewRangeCheck(KanColleClient.Current.Homeport.Organization.Fleets[i]) : (bool?)null;
                isParameter[i] = flags;

                var flagsString = new Dictionary<string, string>();
                flagsString[FIREPOWER] = SumFirepower != null ? GetSumFirepower(KanColleClient.Current.Homeport.Organization.Fleets[i]) : "";
                flagsString[AA] = SumAA != null ? GetSumAA(KanColleClient.Current.Homeport.Organization.Fleets[i]) : "";
                flagsString[ASW] = SumASW != null ? GetSumASW(KanColleClient.Current.Homeport.Organization.Fleets[i]) : "";
                flagsString[VIEWRANGE] = SumViewRange != null ? GetSumViewRange(KanColleClient.Current.Homeport.Organization.Fleets[i]) : "";

                isParameterString[i] = flagsString;
            }
        }

        public static ExpeditionInfo[] ExpeditionList
        {
            get { return _ExpeditionTable; }
            set { _ExpeditionTable = value; }
        }

        public bool CheckAll(Fleet fleet) => CheckShipNum(fleet) &&
                        FlagshipLvCheck(fleet) &&
                        SumLvCheck(fleet) &&
                        RequireShipTypeCheck(fleet) &&
                        RequireItemCheck(fleet) &&
                        FlagShipTypeCheck(fleet) &&
                        RequireSumShipTypeCheck(fleet) &&
                        SumFirepowerCheck(fleet) &&
                        SumAACheck(fleet) &&
                        SumASWCheck(fleet) &&
                        SumViewRangeCheck(fleet);

        /// <summary>
        /// 艦数チェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool CheckShipNum(Fleet fleet) => fleet.Ships.Length >= ShipNum;

        /// <summary>
        /// 旗艦Lvチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool FlagshipLvCheck(Fleet fleet) => fleet.Ships.Length == 0 ? false : fleet.Ships.First().Level >= Lv;

        /// <summary>
        /// 合計Lvチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool SumLvCheck(Fleet fleet) => null == SumLv ? true : fleet.Ships.Select(s => s.Level).Sum() >= SumLv;

        /// <summary>
        /// 艦種取得
        /// </summary>
        /// <param name="ship">艦娘</param>
        /// <returns>艦種</returns>
        private string GetShipType(Ship ship) => ship.Info.Name.StartsWith("大鷹") ? "護衛空母" : ship.Info.ShipType.Name;

        /// <summary>
        /// 艦種チェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool RequireShipTypeCheck(Fleet fleet) => null == RequireShipType ? true : RequireShipType.Any(x =>
                                                                        x.Key.All(y => fleet.Ships.Count(i => Regex.IsMatch(GetShipType(i), y.Item1)) >= y.Item2) &&
                                                                        (x.Value == null ? true : x.Value.Any(z => fleet.Ships.Count(j => Regex.IsMatch(z.Item1, GetShipType(j))) >= z.Item2)));

        /// <summary>
        /// 装備のチェック
        /// </summary{
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool RequireItemCheck(Fleet fleet)
        {
            if (null == RequireItemNum || null == RequireItemShipNum)
            {
                return true;
            }

            foreach (var pair in RequireItemShipNum)
            {
                var shipNum = fleet.Ships.Where(
                    ship => ship.EquippedItems.Any(
                        item => pair.Key.Equals(item.Item.Info.Name))).Count();

                if (shipNum < pair.Value)
                {
                    return false;
                }
            }
            foreach (var pair in RequireItemNum)
            {
                var itemNum = fleet.Ships.Select(
                    ship => ship.EquippedItems.Where(
                        item => pair.Key.Equals(item.Item.Info.Name)).Count()).Sum();

                if (itemNum < pair.Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 旗艦の艦種チェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool FlagShipTypeCheck(Fleet fleet)
        {
            if (null == FlagShipType)
            {
                return true;
            }

            if (0 == fleet.Ships.Length)
            {
                return false;
            }

            var shiptype = fleet.Ships.First().Info.ShipType;
            var regexp = new Regex(FlagShipType);
            var match = regexp.Match(shiptype.Name);

            return match.Success;
        }

        /// <summary>
        /// 必要合算艦種のチェック（駆または海防合わせて3隻というようなのに使用）
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool RequireSumShipTypeCheck(Fleet fleet)
        {
            //必要合算艦種が設定されていない場合は自動成功
            if (null == RequireSumShipType)
            {
                return true;
            }

            var sum = 0;
            foreach (var siptype in RequireSumShipType)
            {
                var re = new Regex(siptype);
                sum += fleet.Ships
                     .Where(f => re.Match(f.Info.ShipType.Name).Success).Count();

                if (sum >= RequireSumShipTypeNum)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 合計火力値のチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string GetSumFirepower(Fleet fleet)
        {
            if (null == SumFirepower)
            {
                return "";
            }
            //var sum = fleet.Ships.Sum(s => s.Firepower.Current + s.EquippedItems.Sum(i => i.Item.Info.Firepower));
            var sum = fleet.Ships.Sum(s => s.RawData.api_karyoku[0]);
            var require = SumFirepower;
            return $"{sum}/{require}";
        }
        private bool SumFirepowerCheck(Fleet fleet)
        {
            if (null == SumFirepower)
            {
                return true;
            }
            //return fleet.Ships.Sum(s => s.Firepower.Current + s.EquippedItems.Sum(i => i.Item.Info.Firepower)) >= SumFirepower;
            return fleet.Ships.Sum(s => s.RawData.api_karyoku[0]) >= SumFirepower;
        }

        /// <summary>
        /// 合計対空値のチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string GetSumAA(Fleet fleet)
        {
            if (null == SumAA)
            {
                return "";
            }
            var sum = fleet.Ships.Sum(s => s.RawData.api_taiku[0]);
            var require = SumAA;
            return $"{sum}/{require}";
        }
        private bool SumAACheck(Fleet fleet) => null == SumAA ? true : fleet.Ships.Sum(s => s.RawData.api_taiku[0]) >= SumAA;

        /// <summary>
        /// 合計対潜値のチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string GetSumASW(Fleet fleet)
        {
            if (null == SumASW)
            {
                return "";
            }
            var sum = fleet.Ships.Sum(s => s.RawData.api_taisen[0]);
            var require = SumASW;
            return $"{sum}/{require}";
        }
        private bool SumASWCheck(Fleet fleet) => null == SumASW ? true : fleet.Ships.Sum(s => s.RawData.api_taisen[0]) >= SumASW;

        /// <summary>
        /// 合計索敵値のチェック
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string GetSumViewRange(Fleet fleet)
        {
            if (null == SumViewRange)
            {
                return "";
            }
            var sum = fleet.Ships.Sum(s => s.RawData.api_sakuteki[0]);
            var require = SumViewRange;
            return $"{sum}/{require}";
        }
        private bool SumViewRangeCheck(Fleet fleet) => null == SumViewRange ? true : fleet.Ships.Sum(s => s.RawData.api_sakuteki[0]) >= SumViewRange;
    }
}
