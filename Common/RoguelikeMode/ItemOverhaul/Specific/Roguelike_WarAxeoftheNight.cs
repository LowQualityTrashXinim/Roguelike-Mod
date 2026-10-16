using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Roguelike.Common.RoguelikeMode.ItemOverhaul.Common;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class Roguelike_WarAxeoftheNight : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.WarAxeoftheNight;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 66;
		entity.scale += .45f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff<NightInfection>()) {
			modifiers.SourceDamage += .5f;
			modifiers.ArmorPenetration += 30;
		}
	}
}
