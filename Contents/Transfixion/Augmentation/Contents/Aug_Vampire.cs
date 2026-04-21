using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Augmentation.Contents;
public class Vampire : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
		int chargeNum = acc.Check_ChargeConvertToStackAmount();
		if (!player.IsHealthAbovePercentage(.6f) && chargeNum >= 1)
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
		if (!player.IsHealthAbovePercentage(.8f) && chargeNum >= 2)
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, 1.25f, Base: 3);
	}
}
