using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Roguelike.Common.Systems;
using Roguelike.Contents.Skill;
using Roguelike.Texture;

namespace Roguelike.Contents.Items.Chest;
internal class WeaponLootBox : ModItem {
	public override string Texture => ModTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		var entitySource = player.GetSource_OpenItem(Type);
		LootBoxBase.GetWeapon(out int Weapon, out int amount);
		if (Main.rand.NextBool(100) && UniversalSystem.LuckDepartment(UniversalSystem.CHECK_RARELOOTBOX)) {
			player.QuickSpawnItem(entitySource, Weapon, amount);
			return;
		}
		player.QuickSpawnItem(entitySource, Weapon, amount);
	}
}
internal class SkillLootBox : ModItem {
	public override string Texture => ModTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		SkillHandlePlayer skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.TotalCount));
	}
}
internal class SpecialSkillLootBox : ModItem {
	public override string Texture => ModTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		SkillHandlePlayer skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.dict_skill[SkillTypeID.Skill_Projectile].Select(i => i.Type).ToList()));
		if (player.HasItem(ModContent.ItemType<SkillOrb>())) {
			ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
		}
	}
}
