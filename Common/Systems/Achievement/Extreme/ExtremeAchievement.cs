using Roguelike.Common.Global;
using Roguelike.Contents.Items.Weapon;
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
public class SynergyFever : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Extreme;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		Player player = Main.LocalPlayer;
		int synergyCount = 0;
		for (int i = 0; i < 50; i++) {
			Item item = player.inventory[i];
			if (item.ModItem is SynergyModItem) {
				synergyCount++;
			}
		}
		return synergyCount >= 49;
	}
}
public class Nightmarish : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Extreme;
		CategoryTag = AchievementTag.Mastery;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod()
			&& RoguelikeWorldProperty.NightmareWorld;
	}
}
