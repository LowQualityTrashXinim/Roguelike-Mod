using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Alchemist : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
		ItemTypeID = ItemID.Bottle;
	}
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.AlchemyTable);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.AlchemyLantern);
			case "3":
				return Get_FormattedDescription(acc, desc, ItemID.AHorribleNightforAlchemy);
			default:
				return desc;
		}
	}
	public override int[] UpgradeAvailable() => [ItemID.AlchemyTable, ItemID.AlchemyLantern, ItemID.AHorribleNightforAlchemy];
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		var modplayer = player.ModPlayerStats();
		modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
		int BuffAmount = player.BuffAmount();
		if (acc.AugmentUpgrade.Contains(ItemID.AlchemyTable)) {
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: BuffAmount);
		}
		if (acc.AugmentUpgrade.Contains(ItemID.AlchemyLantern)) {
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, BuffAmount * .12f);
		}
		if (acc.AugmentUpgrade.Contains(ItemID.AHorribleNightforAlchemy)) {
			modplayer.DebuffDamage += .24f * BuffAmount;
		}
	}
}
