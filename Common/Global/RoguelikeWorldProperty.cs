using Microsoft.Xna.Framework;
using Roguelike.Common.General;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems.DifficultySettingSystem;
using Roguelike.Common.Utils;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Global;
internal class RoguelikeWorldProperty : ModSystem {
	public static RogueLikeConfig config => ModContent.GetInstance<RogueLikeConfig>();
	public static bool RoguelikeWorld = false;
	public static bool BossRushWorld = false;
	public static bool NightmareWorld = false;
	public static bool HellishEndeavour = false;

	public override void Load() {
		On_WorldGen.GenerateWorld += On_WorldGen_GenerateWorld;
	}
	private void On_WorldGen_GenerateWorld(On_WorldGen.orig_GenerateWorld orig, int seed, GenerationProgress customProgressObject) {
		if (config.TerrariaMode) {
			RoguelikeWorld = BossRushWorld = false;
		}
		else if (config.BossRushMode) {
			RoguelikeWorld = false;
			BossRushWorld = !RoguelikeWorld;
		}
		else {
			RoguelikeWorld = true;
			BossRushWorld = !RoguelikeWorld;
		}
		HellishEndeavour = config.HellishEndeavour;
		NightmareWorld = config.NightmareMode;
		orig(seed, customProgressObject);
	}

	public static bool BossRush_Set_Progression = true;
	public static bool BossRush_Set_CommandFight = true;
	public static bool TotalRNG = false;
	public static bool RareSpoils = true;
	public static bool RareLootbox = true;
	public static bool DataSaved = false;
	public override void SaveWorldData(TagCompound tag) {
		if (ModUtils.Is_EnteringOrInASubWorld()) {
			return;
		}
		if (DataSaved) {
			return;
		}
		tag["Setting_HellishEndeavour"] = HellishEndeavour;
		tag["Setting_DataSaved"] = true;
		tag["Setting_RoguelikeWorld"] = RoguelikeWorld;
		tag["Setting_BossRushWorld"] = BossRushWorld;
		tag["Setting_BossRushSet1"] = BossRush_Set_Progression;
		tag["Setting_BossRushSet2"] = BossRush_Set_CommandFight;
		tag["Setting_TotalRNG"] = TotalRNG;
		tag["Setting_RareSpoils"] = RareSpoils;
		tag["Setting_RareLootbox"] = RareLootbox;
		tag["Setting_Nightmare"] = NightmareWorld;

		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		tag["Difficulty_Enemy_HP"] = system.Enemy_HP;
		tag["Difficulty_Enemy_DMG"] = system.Enemy_DMG;
		tag["Difficulty_Boss_DMG"] = system.Boss_DMG;
		tag["Difficulty_Boss_HP"] = system.Boss_HP;
		tag["Difficulty_Boss_DMGPercentage"] = system.Boss_DMGPercentage;
		tag["Difficulty_Boss_ProgressionLock"] = system.Boss_ProgressionLock;
		tag["Difficulty_Player_ReviveCurse"] = system.Player_ReviveCurse;
		tag["Difficulty_Player_HitTakenEffectiveness"] = system.Player_HitTakenEffectiveness;
		tag["Difficulty_Player_TimeRestriction"] = system.Player_TimeRestriction;
		tag["Difficulty_Player_TimeRestriction_Scale"] = system.Player_TimeRestriction_Scale;
		tag["Difficulty_World_ReduceHouseLoot"] = system.World_ReduceHouseLoot;
		tag["Difficulty_World_IncreasesSpawnRate"] = system.World_IncreasesSpawnRate;
		tag["Difficulty_World_EnemyToElite"] = system.World_EnemyToElite;
		tag["Difficulty_World_EnemyRevive"] = system.World_EnemyRevive;
		tag["Difficulty_World_BiomeModifier"] = system.World_BiomeModifier;
	}
	public override void LoadWorldData(TagCompound tag) {
		if (ModUtils.Is_EnteringOrInASubWorld()) {
			return;
		}
		HellishEndeavour = tag.Get<bool>("Setting_HellishEndeavour");
		RoguelikeWorld = tag.Get<bool>("Setting_RoguelikeWorld");
		BossRushWorld = tag.Get<bool>("Setting_BossRushWorld");
		DataSaved = tag.Get<bool>("Setting_DataSaved");
		BossRush_Set_Progression = tag.Get<bool>("Setting_BossRushSet1");
		BossRush_Set_CommandFight = tag.Get<bool>("Setting_BossRushSet2");
		TotalRNG = tag.Get<bool>("Setting_TotalRNG");
		RareSpoils = tag.Get<bool>("Setting_RareSpoils");
		RareLootbox = tag.Get<bool>("Setting_RareLootbox");
		NightmareWorld = tag.Get<bool>("Setting_Nightmare");

		DifficultySettingSystem system = ModContent.GetInstance<DifficultySettingSystem>();
		system.Enemy_HP = tag.Get<byte>("Difficulty_Enemy_HP");
		system.Enemy_DMG = tag.Get<byte>("Difficulty_Enemy_DMG");
		system.Boss_DMG = tag.Get<byte>("Difficulty_Boss_DMG");
		system.Boss_HP = tag.Get<byte>("Difficulty_Boss_HP");
		system.Boss_DMGPercentage = tag.Get<byte>("Difficulty_Boss_DMGPercentage");
		system.Boss_ProgressionLock = tag.Get<byte>("Difficulty_Boss_ProgressionLock");
		system.Player_ReviveCurse = tag.Get<bool>("Difficulty_Player_ReviveCurse");
		system.Player_HitTakenEffectiveness = tag.Get<bool>("Difficulty_Player_HitTakenEffectiveness");
		system.Player_TimeRestriction = tag.Get<bool>("Difficulty_Player_TimeRestriction");
		system.Player_TimeRestriction_Scale = tag.Get<byte>("Difficulty_Player_TimeRestriction_Scale");
		system.World_ReduceHouseLoot = tag.Get<byte>("Difficulty_World_ReduceHouseLoot");
		system.World_IncreasesSpawnRate = tag.Get<byte>("Difficulty_World_IncreasesSpawnRate");
		system.World_EnemyToElite = tag.Get<byte>("Difficulty_World_EnemyToElite");
		system.World_EnemyRevive = tag.Get<bool>("Difficulty_World_EnemyRevive");
		system.World_BiomeModifier = tag.Get<bool>("Difficulty_World_BiomeModifier");
	}
}
public class RoguelikeWorldProperty_Player : ModPlayer {
	public override void OnEnterWorld() {
		var config = RoguelikeWorldProperty.config;
		if (!ModUtils.Is_EnteringOrInASubWorld()) {
			ModContent.GetInstance<RogueLikeWorldGen>().InitializeBiomeWorld();
		}
		RoguelikeWorldProperty.BossRush_Set_Progression = config.BossRushMode_Setting_FightBossInProgression;
		RoguelikeWorldProperty.BossRush_Set_CommandFight = config.BossRushMode_Setting_SpawnOnPlayerCommand;
		RoguelikeWorldProperty.TotalRNG = config.TotalRNG;
		RoguelikeWorldProperty.RareSpoils = config.RareSpoils;
		RoguelikeWorldProperty.RareLootbox = config.RareLootbox;
		RoguelikeWorldProperty.RareLootbox = config.NightmareMode;
	}
}
