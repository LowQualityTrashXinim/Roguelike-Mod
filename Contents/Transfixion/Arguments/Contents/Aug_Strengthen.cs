using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Strengthen : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		int charge = acc.Check_ChargeConvertToStackAmount(index);
		PlayerStatsHandle stathandle = player.ModPlayerStats();
		if (charge >= 3) {
			stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.2f);
			stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.25f);
			stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 10);
			stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 30);
			stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 30);
			stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 7);
			stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 10);
			stathandle.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
			stathandle.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
			if (charge < 4) {
				return;
			}
		}
		if (charge >= 2) {
			stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.13f);
			stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.15f);
			stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 6);
			stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 17);
			stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 17);
			stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 4);
			stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 6);
			if (charge < 4) {
				return;
			}
		}
		if (charge >= 1) {
			stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.07f);
			stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.13f);
			stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 3);
			stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 10);
			stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 10);
			stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 2);
			stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 3);
			if (charge < 4) {
				return;
			}
		}
		stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.03f);
		stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.06f);
		stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 2);
		stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1);
		stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 1);
	}
}
