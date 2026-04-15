using Roguelike.Common.General;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Global;
internal class RoguelikeWorldProperty : ModSystem {
	public static RogueLikeConfig config => ModContent.GetInstance<RogueLikeConfig>();
	public static bool RoguelikeWorld = false;
	public static bool BossRushWorld = false;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (config.BossRushMode) {
			RoguelikeWorld = false;
			BossRushWorld = !RoguelikeWorld;
		}
		else {
			RoguelikeWorld = true;
			BossRushWorld = !RoguelikeWorld;
		}
	}
	public static bool BossRush_Set_Progression = true;
	public static bool BossRush_Set_CommandFight = true;
	public static bool TotalRNG = false;
	public static bool RareSpoils = true;
	public static bool RareLootbox = true;
	public bool DataSaved = false;
	public override void SaveWorldData(TagCompound tag) {
		if (DataSaved) {
			return;
		}
		tag["Setting_RoguelikeWorld"] = RoguelikeWorld;
		tag["Setting_BossRushWorld"] = BossRushWorld;
		tag["Setting_BossRushSet1"] = BossRush_Set_Progression;
		tag["Setting_BossRushSet2"] = BossRush_Set_CommandFight;
		tag["Setting_TotalRNG"] = TotalRNG;
		tag["Setting_RareSpoils"] = RareSpoils;
		tag["Setting_RareLootbox"] = RareLootbox;
		tag["Setting_DataSaved"] = true;
	}
	public override void LoadWorldData(TagCompound tag) {
		DataSaved = tag.Get<bool>("Setting_DataSaved");
		BossRush_Set_Progression = tag.Get<bool>("Setting_BossRushSet1");
		BossRush_Set_CommandFight = tag.Get<bool>("Setting_BossRushSet2");
		TotalRNG = tag.Get<bool>("Setting_TotalRNG");
		RareSpoils = tag.Get<bool>("Setting_RareSpoils");
		RareLootbox = tag.Get<bool>("Setting_RareLootbox");
	}
}
public class RoguelikeWorldProperty_Player : ModPlayer {
	public override void OnEnterWorld() {
		var config = RoguelikeWorldProperty.config;
		RoguelikeWorldProperty.BossRush_Set_Progression = config.BossRushMode_Setting_FightBossInProgression;
		RoguelikeWorldProperty.BossRush_Set_CommandFight = config.BossRushMode_Setting_SpawnOnPlayerCommand;
		RoguelikeWorldProperty.TotalRNG = config.TotalRNG;
		RoguelikeWorldProperty.RareSpoils = config.RareSpoils;
		RoguelikeWorldProperty.RareLootbox = config.RareLootbox;
	}
}
