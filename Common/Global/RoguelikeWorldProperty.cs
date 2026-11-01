using Mono.Cecil.Cil;
using Roguelike.Common.General;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems.WorldSettingSystem;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Global;
public class RoguelikeWorldProperty : ModSystem {
	public bool TerrariaWorld => !RoguelikeWorld && !BossRushWorld && !ArcadeMode;
	public bool RoguelikeWorld = false;
	/// <summary>
	/// Not just a boss rush world modifier but also a mode
	/// </summary>
	public bool BossRushWorld = false;
	/// <summary>
	/// Not just a mode but also a world gen modifier.
	/// </summary>
	public bool ArcadeMode = false;

	//Extra difficulty
	public bool CursedWorld = false;
	public bool NightmareWorld = false;
	public bool HellishEndeavour = false;

	public bool TotalRNG = false;
	public bool RareSpoils = true;
	public bool RareLootbox = true;
	public bool DataSaved = false;
	public bool HasThisDataLoaded = false;
	/// <summary>
	/// Does absolutely nothing
	/// </summary>
	public bool DummyTrashSave = false;
	public override void PreSaveAndQuit() {
		//BossRushWorld = false;
		//ArcadeMode = false;
		//BossRushWorld = false;
		//CursedWorld = false;
		//NightmareWorld = false;
		//TotalRNG = false;
		//RareSpoils = false;
		//RareLootbox = false;
		//DataSaved = false;
	}
	public override void SaveWorldData(TagCompound tag) {
		if (ModUtils.Is_EnteringOrInASubWorld()) {
			return;
		}
		tag["DummySave"] = DummyTrashSave;

		//WorldFile.LoadWorld(false);
		tag["Setting_HellishEndeavour"] = HellishEndeavour;
		tag["Setting_DataSaved"] = true;
		tag["Setting_RoguelikeWorld"] = RoguelikeWorld;
		tag["Setting_BossRushWorld"] = BossRushWorld;
		tag["Setting_TotalRNG"] = TotalRNG;
		tag["Setting_RareSpoils"] = RareSpoils;
		tag["Setting_RareLootbox"] = RareLootbox;
		tag["Setting_CursedWorld"] = CursedWorld;
		tag["Setting_Nightmare"] = NightmareWorld;
		tag["Setting_ArcadeMode"] = ArcadeMode;

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
		DummyTrashSave = tag.Get<bool>("DummyTrashSave");

		if (ModUtils.Is_EnteringOrInASubWorld()) {
			return;
		}
		DataSaved = tag.Get<bool>("Setting_DataSaved");

		bool value;
		string prefix = "Setting_";

		HellishEndeavour = tag.Get<bool>("Setting_HellishEndeavour");
		RoguelikeWorld = tag.Get<bool>("Setting_RoguelikeWorld");
		BossRushWorld = tag.Get<bool>("Setting_BossRushWorld");
		TotalRNG = tag.Get<bool>("Setting_TotalRNG");
		RareSpoils = tag.Get<bool>("Setting_RareSpoils");
		RareLootbox = tag.Get<bool>("Setting_RareLootbox");
		NightmareWorld = tag.Get<bool>("Setting_Nightmare");
		if (tag.TryGet(prefix + "CursedWorld", out value)) {
			CursedWorld = value;
		}
		if (tag.TryGet(prefix + "ArcadeMode", out value)) {
			ArcadeMode = value;
		}

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
		var config = ModContent.GetInstance<RogueLikeConfig>();
		var property = ModContent.GetInstance<RoguelikeWorldProperty>();
		if (property.TerrariaWorld) {
			Main.NewText("Entering terraria world setting");
		}
		else if (property.ArcadeMode) {
			Main.NewText("Entering arcade world setting");
		}
		else if (property.BossRushWorld) {
			Main.NewText("Entering boss rush world setting");
		}
		if (!ModUtils.Is_EnteringOrInASubWorld()) {
			ModContent.GetInstance<RogueLikeWorldGen>().InitializeBiomeWorld();
		}
		property.RareSpoils = config.RareSpoils;
		property.RareLootbox = config.RareLootbox;
	}
}
