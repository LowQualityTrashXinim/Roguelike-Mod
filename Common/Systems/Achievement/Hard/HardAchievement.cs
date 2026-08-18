using Roguelike.Common.Systems.BossRushMode;
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
			return ModContent.GetInstance<BossRushStructureHandler>().Get_Timer.TotalHours <= 1 && UniversalSystem.DidPlayerBeatTheMod();
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
			return ModContent.GetInstance<BossRushStructureHandler>().Get_Timer.TotalMinutes <= 40 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
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
			return ModContent.GetInstance<BossRushStructureHandler>().Get_Timer.TotalMinutes <= 25 && UniversalSystem.DidPlayerBeatTheMod() && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
		}
		return false;
	}
}
public class GuardianNow : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.Fun;
	}
	public override bool Condition() => Main.ActivePlayerFileData != null && Main.LocalPlayer.statDefense >= 9999;
}

public class Over9000 : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.Fun;
	}
	public override bool Condition() => Main.ActivePlayerFileData != null && Main.LocalPlayer.GetWeaponDamage(Main.LocalPlayer.HeldItem) >= 9000;
}

public class InfinitePlusOne : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Hard;
		CategoryTag = AchievementTag.Fun;
	}
	public override bool Condition() => Main.ActivePlayerFileData != null && Main.LocalPlayer.getDPS() < 0;
}
