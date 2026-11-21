using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Accessories;
internal class Roguelike_ArmorBracing : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ArmorBracing;
	public override void UpdateEquip(Item item, Player player) {
		player.endurance += .15f;
		player.ModPlayerStats().UpdateDefenseBase.Base += 10;
		int missingHealth = player.statLifeMax2 - player.statLife;
		player.ModPlayerStats().UpdateDefenseBase += .05f * missingHealth / 100;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
