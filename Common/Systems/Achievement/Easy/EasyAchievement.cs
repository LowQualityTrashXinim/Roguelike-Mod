 
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Contents.Transfixion.Artifacts;
using Terraria;
using Terraria.UI;

namespace Roguelike.Common.Systems.Achievement.Easy;
public class BountifulHarvest : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 100;
	}
}

public class SkillCheck : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}

public class TokenOfGreed : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<TokenOfGreedArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>() && UniversalSystem.NotNormalMode();
	}
}

public class TokenOfPride : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override void Draw(UIElement element, SpriteBatch spriteBatch) {
		Artifact.GetArtifact(Artifact.ArtifactType<TokenOfPrideArtifact>()).DrawInUI(spriteBatch, element.GetDimensions());
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfWrath : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfSloth : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfSlothArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfGluttony : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class BootOfSpeedManipulation : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<BootsOfSpeedManipulationArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<BootsOfSpeedManipulationArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class VampirismCrystal : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool SpecialDraw => true;
	public override void Draw(UIElement element, SpriteBatch spriteBatch) {
		Artifact.GetArtifact(Artifact.ArtifactType<VampirismCrystalArtifact>()).DrawInUI(spriteBatch, element.GetDimensions());
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<VampirismCrystalArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class HeartOfEarth : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<HeartOfEarthArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class GamblerSoul : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<GamblerSoulArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class ManaOverloader : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<ManaOverloaderArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class EssenceLantern : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<EssenceLanternArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class AlchemistKnowledge : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class Elite : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		if (Main.CurrentPlayer.TryGetModPlayer(out PlayerStatsHandle modplayer)) {
			return modplayer.EliteKillCount > 0;
		}
		return false;
	}
}
