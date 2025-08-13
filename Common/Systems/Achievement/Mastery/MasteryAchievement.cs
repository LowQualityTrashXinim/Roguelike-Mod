using Roguelike.Common.Mode.HellishEndeavour;
using Roguelike.Common.Mode.Nightmare;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.Achievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Roguelike.Common.Systems.Achievement.Mastery;
public class TrueNightmare : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Mastery;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && NightmareSystem.IsANightmareWorld() && Main.masterMode && Main.getGoodWorld;
	}
}
public class GodOfChallenge : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Mastery;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod()
			&& HellishEndeavorSystem.Hellish()
			&& (Main.expertMode || Main.masterMode);
	}
}
