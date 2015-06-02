using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Collision = LeagueSharp.Common.Collision;

//Noob code, if you can help me improve it, thank you. Just learning. 


namespace SimpleEconomist
{

    internal class Program
    {
        public static int tempo = 0;
        public static List<Obj_AI_Hero> allies = HeroManager.Allies;
        public static List<Obj_AI_Hero> enemies = HeroManager.Enemies;
        //public static Dictionary<Obj_AI_Hero, double> enemyDictionary = new Dictionary<Obj_AI_Hero, double>()
        public static Menu Menu;
        public static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnStart += Game_OnStart;


        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("SimpleEconomist", "simpleecnomist", true);
            Menu.AddItem(new MenuItem("Enable", "Enable", true).SetValue(true));
            Menu.AddToMainMenu();

        }


        public static void Drawing_OnDraw(EventArgs args)
        {

            foreach (var unit in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValid && h.IsHPBarRendered))
            {

                int minion = unit.MinionsKilled * 19;
                int supermonster = unit.SuperMonsterKilled * 300;
                int neutralminion = unit.NeutralMinionsKilled * 35;
                int wards = unit.WardsKilled * 30;

                float ouroinicial = 475;
                float ouroptempo = 0;
                float ouropsegundo = 19;
                float ourototal = 0;

                foreach (var m in unit.Masteries)
                {

                    if (m.Id == 114 && m.Page.Equals(MasteryPage.Utility))
                    {
                        ouroinicial = 515;
                    }

                    if (m.Id == 97 && m.Points == 1 && m.Page.Equals(MasteryPage.Utility))
                    {
                        ouropsegundo = 19.5f;
                    }
                    if (m.Id == 97 && m.Points == 2 && m.Page.Equals(MasteryPage.Utility))
                    {
                        ouropsegundo = 20;
                    }
                    if (m.Id == 97 && m.Points == 3 && m.Page.Equals(MasteryPage.Utility))
                    {
                        ouropsegundo = 20.5f;
                    }

                }


                if (tempo >= 22130)
                {
                    ouroptempo = (((tempo - 22130) / 10) * ouropsegundo) + ouroinicial;
                }
                ourototal = (ouroptempo) + (unit.ChampionsKilled * 300) + (unit.Assists * 75) + (minion + supermonster + neutralminion + wards);

                string msg = "L$: " + (int)Math.Ceiling(ourototal);
                if (unit.Name == Player.Name)
                {
                    msg = "L$: " + (int)Math.Ceiling(Player.GoldTotal);
                }

                var wts = Drawing.WorldToScreen(unit.Position);
                Drawing.DrawText(wts[0] - (msg.Length) * 10, wts[1], System.Drawing.Color.Yellow, msg);

            }

        }

        public static void Game_OnMinionSpawn(Object sender)
        {
            tempo = Environment.TickCount / 1000;

        }


    }

}
