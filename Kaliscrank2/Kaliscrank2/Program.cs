using System;
using System.Linq;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using LeagueSharp.Console;
using Collision = LeagueSharp.Common.Collision;

namespace Kaliscrank2
{
    class Program
    {
        static Spell Q, W, E, R;
        internal static Menu Menu;
        
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("Kaliscrank", "kaliscrank", true);

            Menu.SubMenu("Misc").AddItem(new MenuItem("kaliscrank", "Use Kaliscrank", true).SetValue(true));
            LeagueSharp.Console.Console.WriteLine("Kaliscrank Loaded");
        }
        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            Q = new Spell(SpellSlot.Q, 1150f);
            W = new Spell(SpellSlot.W, 5000f);
            E = new Spell(SpellSlot.E, 1000f);
            R = new Spell(SpellSlot.R, 1500f);


            foreach (var unit in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsEnemy && h.IsHPBarRendered))
            {
                // Get buffs
                for (int i = 0; i < unit.Buffs.Count(); i++)
                {
                    // Check if the Soulbound is in a good range
                    var soulboundhero = HeroManager.Allies.FirstOrDefault(hero => hero.HasBuff("kalistacoopstrikeally", true) && args.Target.NetworkId == hero.NetworkId && hero.HealthPercent <= 15);
                    var enemy = HeroManager.Enemies.Where(x => soulboundhero.Distance(x.Position) > 800);
                    // Check if the Soulbound is a Blitzcrank
                    // Check if the enemy is hooked
                    // Check if target was far enough for ult
                    if (soulboundhero.ChampionName == "Blitzcrank" && unit.Buffs[i].Name == "rocketgrab2" && unit.Buffs[i].IsActive && enemy.Count() > 0 && Menu.Item("kaliscrank", true).GetValue<Boolean>())
                    {
                        R.Cast();
                    }
                }
            }
        }
    }
}
