 
using Roguelike.Common.ChallengeMode;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.Mutation;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Chest;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Contents.Perks;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.RoguelikePerk;
public class SuppliesDrop : Perk {
	public override void SetDefaults() {
		textureString = ModTexture.SUPPILESDROP;
		CanBeStack = true;
		StackLimit = -1;
		CanBeChoosen = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void OnChoose(Player player) {
		LootBoxBase.GetWeapon(out int weapon, out int amount);
		player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
	}
}
public class GiftOfRelic : Perk {
	public override void SetDefaults() {
		textureString = ModTexture.Get_MissingTexture("Perk");
		CanBeStack = true;
		StackLimit = -1;
		CanBeChoosen = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void OnChoose(Player player) {
		player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Relic>());
	}
}
public class WeaponDismantle : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_DismantleWeapon = true;
	}
}
public class EssenceExtraction : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 999;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_DismantleWeapon = true;
	}
}
public class PeaceWithGod : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<PeaceWithGod>();
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonusBlock = true;
		player.GetModPlayer<PlayerStatsHandle>().CanDropSynergyEnergy = true;
	}
}
public class LostInWonderLand : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		ModContent.GetInstance<MutationSystem>().MutationChance += .1f * StackAmount(player);
		modplayer.AugmentationChance += .05f * StackAmount(player);
		modplayer.RandomizeChanceEnchantment += .05f * StackAmount(player);
	}
}
public class AlchemistEmpowerment : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAs<AlchemistEmpowerment>("PotionExpert");
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MysteriousPotionEffectiveness, Base: 3);
		player.GetModPlayer<PerkPlayer>().perk_AlchemistPotion = true;
		player.GetModPlayer<PerkPlayer>().perk_PotionCleanse = true;
		player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
		player.GetModPlayer<PlayerStatsHandle>().LootboxCanDropSpecialPotion = true;
	}
}
