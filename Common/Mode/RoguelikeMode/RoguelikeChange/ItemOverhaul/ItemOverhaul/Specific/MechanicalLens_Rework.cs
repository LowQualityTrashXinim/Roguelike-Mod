using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_MechanicalLens : GlobalItem {
	public override void UpdateEquip(Item item, Player player) {
		if (item.type == ItemID.MechanicalLens) {
			PlayerStatsHandle handler = player.ModPlayerStats();
			handler.AddStatsToPlayer(PlayerStats.CritChance, 1, 1.33f, 5);
			handler.UpdateCritDamage -= .15f;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.MechanicalLens) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
