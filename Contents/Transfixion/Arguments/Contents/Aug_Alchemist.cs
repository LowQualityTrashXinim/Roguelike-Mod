using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Alchemist : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.BlueViolet;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		int charge = acc.Check_ChargeConvertToStackAmount(index);
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
		if (charge >= 1) {
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: player.BuffAmount());
		}
	}
}
