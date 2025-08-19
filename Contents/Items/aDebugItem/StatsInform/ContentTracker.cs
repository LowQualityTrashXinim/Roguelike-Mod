using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Contents.Skill;
using Roguelike.Contents.Perks;
using System.Collections.Generic;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Common.Systems.SpoilSystem;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Transfixion.Arguments;
using Roguelike.Contents.Transfixion.WeaponEnchantment;

namespace Roguelike.Contents.Items.aDebugItem.StatsInform;
internal class ContentTracker : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 10;
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		int total =
			ModItemLib.ListLootboxType.Count
			+ ModItemLib.SynergyItem.Count
			+ Artifact.ArtifactCount
			+ EnchantmentLoader.TotalCount
			+ SkillModSystem.TotalCount
			+ ModPerkLoader.TotalCount
			+ ModSpoilSystem.TotalCount
			+ AugmentsLoader.TotalCount;

		var line = new TooltipLine(Mod, "StatsShowcase",
			$"LootBox amount : {ModItemLib.ListLootboxType.Count}" +
			$"\nSynergy item amount : {ModItemLib.SynergyItem.Count}" +
			$"\nArtifact amount : {Artifact.ArtifactCount}" +
			$"\nWeapon enchantment amount : {EnchantmentLoader.TotalCount}" +
			$"\nSkill amount: {SkillModSystem.TotalCount}" +
			$"\nPerk amount : {ModPerkLoader.TotalCount}" +
			$"\nSpoils amount : {ModSpoilSystem.TotalCount}" +
			$"\nRelic template amount : {RelicTemplateLoader.TotalCount}" +
			$"\nAugments amount : {AugmentsLoader.TotalCount}" +
			$"\nTotal content amount : {total}"
			);
		tooltips.Add(line);
	}
}
