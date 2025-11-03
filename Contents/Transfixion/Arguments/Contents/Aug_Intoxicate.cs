using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Intoxicate : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		int chargenum = acc.Check_ChargeConvertToStackAmount(index);
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				player.endurance += .1f;
				if (chargenum >= 1) {
					PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Additive: 1.15f, Flat: 5);
				}
			}
		}
	}
}
