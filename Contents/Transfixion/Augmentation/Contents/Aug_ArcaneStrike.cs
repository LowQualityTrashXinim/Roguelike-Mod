using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class ArcaneStrike : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkBlue;
		ItemTypeID = ItemID.ManaCrystal;
	}
	public override int[] UpgradeAvailable() => [ItemID.FallenStar, ItemID.ArcaneCrystal];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.FallenStar);
			case "2":
				return Get_FormattedDescription(acc, desc, ItemID.ArcaneCrystal);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.AddStatsToPlayer(PlayerStats.PureDamage, 1 + player.statManaMax2 * .0005f);
		if (acc.AugmentUpgrade.Contains(ItemID.FallenStar)) {
			handler.AddStatsToPlayer(PlayerStats.CritChance, Base: player.statManaMax2 * .01f);
		}
		if (acc.AugmentUpgrade.Contains(ItemID.ArcaneCrystal)) {
			handler.UpdateDefenseBase.Base += player.statManaMax2 * .1f;
		}
	}
}
