﻿using Roguelike.Common.Mode.HellishEndeavour;
using Terraria;

namespace Roguelike.Common.Systems.Achievement.Mastery;
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
