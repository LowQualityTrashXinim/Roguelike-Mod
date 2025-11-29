using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;

using Roguelike.Texture;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks.RoguelikePerk;
using Terraria.ModLoader.IO;

namespace Roguelike.Contents.Transfixion.Perks;
class WorldEssence : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.DefaultState);
		return true;
	}
}
class PerkEssence : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Perk");
	public int PerkToGive = -1;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		if (PerkToGive == -1) {
			PerkToGive = Perk.GetPerkType<SuppliesDrop>();
		}
		UniversalSystem.AddPerk(PerkToGive);
		ModUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(PerkToGive).DisplayName);
		return true;
	}
	public override void SaveData(TagCompound tag) {
		tag["PerkToGive"] = PerkToGive;
	}
	public override void LoadData(TagCompound tag) {
		PerkToGive = tag.Get<int>("PerkToGive");
	}
}
class GlitchWorldEssence : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.DefaultState, "Glitch");
		return true;
	}
}
class CelestialEssence : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}

	public override bool? UseItem(Player player) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.StarterPerkState);
		return true;
	}
}
class LuckEssence : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (player.ItemAnimationJustStarted) {
			var modplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
			var listOfPerk = new List<int>();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (modplayer.perks.ContainsKey(i)) {
					if (!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0
						|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
					continue;
				}
				if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
					continue;
				}
				listOfPerk.Add(i);
			}
			int perkType = Main.rand.Next(listOfPerk);
			UniversalSystem.AddPerk(perkType);
			ModUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
		}
		return true;
	}
}
