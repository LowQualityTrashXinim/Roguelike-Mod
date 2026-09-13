using Roguelike.Common.Global;
using Roguelike.Common.Systems.HellishEndeavour;
using Terraria;

namespace Roguelike.Common.Systems.Achievement.Extreme;
public class GodOfChallenge : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Extreme;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod()
			&& RoguelikeWorldProperty.HellishEndeavour
			&& (Main.expertMode || Main.masterMode);
	}
}
