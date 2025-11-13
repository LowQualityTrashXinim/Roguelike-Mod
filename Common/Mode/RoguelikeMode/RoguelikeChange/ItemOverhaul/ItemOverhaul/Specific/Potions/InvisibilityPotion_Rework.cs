using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific.Potions;
internal class Roguelike_InvisibilityPotion : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.InvisibilityPotion;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
internal class Roguelike_InvisibilityPotion_Buff : GlobalBuff {
	public override void Update(int type, Player player, ref int buffIndex) {
		if (type == BuffID.Invisibility) {
			player.ModPlayerStats().UpdateCritDamage += .15f;
			player.GetCritChance<GenericDamageClass>() += 5;
		}
	}
}
