using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class VitalityStrike : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
		ItemTypeID = ItemID.LifeCrystal;
	}
	public override int[] UpgradeAvailable() {
		return [ItemID.LifeFruit, ItemID.Ruby];
	}
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.LifeCrystal);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.Ruby);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.AddStatsToPlayer( PlayerStats.PureDamage, 1 + player.statLifeMax2 * .0005f);
		if (acc.AugmentUpgrade.Contains(ItemID.LifeFruit)) {
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: player.statLifeMax2 * .01f);
		}
		if (acc.AugmentUpgrade.Contains(ItemID.Ruby)) {
			modplayer.UpdateDefenseBase.Base += player.statLifeMax2 * .1f;
		}
	}
}
