using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class Vampire : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.DarkRed;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += "\n" + Description2("1");
					break;
				case 2:
					desc += "\n" + Description2("2");
					break;
				case 3:
				case 4:
				case 5:
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (!player.IsHealthAbovePercentage(.6f) && chargeNum >= 1)
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
		if (!player.IsHealthAbovePercentage(.6f) && chargeNum >= 2)
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, 1.25f, Base: 1);
	}
}
