using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.NPCs.LootBoxLord;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.Achievement.Hard;
public class OceanOfFortune : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 1000;
	}
}

public class LordOfLootBox : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override bool Condition() {
		return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(ModContent.NPCType<LootBoxLord>());
	}
}
public class SpeedRunner : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 20 && UniversalSystem.DidPlayerBeatTheMod();
		}
		return false;
	}
}
public class BossRushRunnerI : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.BossRush;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 10 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
		}
		return false;
	}
}
public class BossRushRunnerII : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.BossRush;
	}
	public override bool Condition() {
		if (Main.ActivePlayerFileData != null) {
			return Main.ActivePlayerFileData.GetPlayTime().TotalMinutes <= 5 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
		}
		return false;
	}
}
public class StraightForTheWall : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh) || ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFleshEye) && ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count <= 1;
	}
}
