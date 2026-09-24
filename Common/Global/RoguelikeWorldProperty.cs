using Microsoft.Xna.Framework;
using Roguelike.Common.General;
using Roguelike.Common.RoguelikeMode;
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
