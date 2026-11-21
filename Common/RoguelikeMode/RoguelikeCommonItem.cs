using Roguelike.Common.Global;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode;
internal class RoguelikeCommonItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return ModContent.GetInstance<RoguelikeWorldProperty>().RoguelikeWorld;
	}
	public override void SetDefaults(Item entity) {
		if (entity.buffType != 0) {
			entity.maxStack = 30;
		}
		if (entity.ammo != AmmoID.None) {
			entity.maxStack = 99;
		}
		entity.maxStack = 999;
		Set_ItemRarity(entity);
		Set_ItemValue(entity);
	}

	private void Set_ItemRarity(Item item) {

	}
	private void Set_ItemValue(Item item) {

	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		base.ModifyTooltips(item, tooltips);
	}
	public override bool CanUseItem(Item item, Player player) {
		return base.CanUseItem(item, player);
	}
}
