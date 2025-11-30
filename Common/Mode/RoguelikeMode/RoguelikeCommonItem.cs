using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Roguelike.Common.RoguelikeMode;

namespace Roguelike.Common.Mode.RoguelikeMode;
internal class RoguelikeCommonItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld;
	}
	public override void SetDefaults(Item entity) {
		if (entity.buffType != 0) {
			entity.maxStack = 30;
		}
		if (entity.ammo != AmmoID.None) {
			entity.maxStack = 99;
		}
		entity.maxStack = 999;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		base.ModifyTooltips(item, tooltips);
	}
	public override bool CanUseItem(Item item, Player player) {
		return base.CanUseItem(item, player);
	}
}
