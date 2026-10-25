using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Accessories;
internal class Roguelike_ManaRegenerationBand : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ManaRegenerationBand;
	public override void UpdateEquip(Item item, Player player) {
		bool maxhealth = player.statLife >= player.statLifeMax2;
		bool maxmana = player.statMana >= player.statManaMax2;
		if (maxhealth) {
			player.ModPlayerStats().UpdateHPRegen.Base += 3;
		}
		if (maxmana) {
			player.ModPlayerStats().UpdateManaRegen.Base += 15;
		}
		if (maxhealth && maxmana) {
			player.GetModPlayer<SkillHandlePlayer>().EnergyRegen.Base += 3;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework",item.Name)));
	}
}
