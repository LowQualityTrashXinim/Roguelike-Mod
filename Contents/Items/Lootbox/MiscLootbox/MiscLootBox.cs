using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using Roguelike.Contents.Transfixion.Skill;
using Roguelike.Texture;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Lootbox.MiscLootbox;
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
		ModUtils.GetWeapon(out int Weapon, out int amount);
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
		Item.maxStack = 999;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		var skillplayer = player.GetModPlayer<SkillHandlePlayer>();
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
		var skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.dict_skill[SkillTypeID.Skill_Summon].Select(i => i.Type).ToList()));
		if (player.HasItem(ModContent.ItemType<SkillOrb>())) {
			ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
		}
	}
}
public class StarterLootBox : LootBoxBase {
	public override string Texture => ModTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override List<int> Set_ItemPool() {
		return new List<int> { ItemPool.GetPoolType<UniversalPool>() };
	}
	public override void AbsoluteRightClick(Player player) {
		var entitySource = player.GetSource_OpenItem(Type);
		GetWeapon(entitySource, player, 1);
		ModUtils.GetArmorForPlayer(entitySource, player);
		GetAccessories(player, 1);
		GetPotions(player, 1, 1);
	}
}
