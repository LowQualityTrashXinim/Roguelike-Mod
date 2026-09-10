using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Intoxicate : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.GreenYellow;
		ItemTypeID = ItemID.Vilethorn;
	}
	public override int[] UpgradeAvailable() => [ItemID.FlaskofPoison];
	public override string Description2(Player player, AugmentsWeapon acc, Item item, string Extra) {
		string desc = base.Description2(player, acc, item, Extra);
		switch (Extra) {
			case "1":
				return Get_FormattedDescription(acc, desc, ItemID.FlaskofPoison);
			default:
				return desc;
		}
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				player.endurance += .1f;
				if (acc.AugmentUpgrade.Contains(ItemID.FlaskofPoison)) {
					PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Additive: 1.15f, Flat: 5);
				}
			}
		}
	}
}
