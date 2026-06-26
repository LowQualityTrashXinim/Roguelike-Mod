using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Alchemist : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.BlueViolet;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		int charge = acc.Check_ChargeConvertToStackAmount();
		var modplayer = player.ModPlayerStats();
		modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
		int BuffAmount = player.BuffAmount();
		if (charge >= 1) {
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: BuffAmount);
		}
		if (charge >= 2) {
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, BuffAmount * .04f);
		}
	}
}
