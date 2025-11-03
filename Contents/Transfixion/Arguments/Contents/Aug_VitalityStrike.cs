using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class VitalityStrike : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statLifeMax2 * .0005f);
		int charge = acc.Check_ChargeConvertToStackAmount(index);
		if (charge >= 1) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statLifeMax2 * .01f);
		}
	}
}
