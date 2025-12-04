
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
public class BountifulHarvest : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen >= 100;
	}
}

public class SkillCheck : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count > 0;
	}
}

public class TokenOfGreed : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<TokenOfGreedArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>() && UniversalSystem.NotNormalMode();
	}
}

public class TokenOfPride : RoguelikeAchievement {
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
public class TokenOfWrath : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfSloth : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfSlothArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class TokenOfGluttony : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class BootOfSpeedManipulation : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<BootsOfSpeedManipulationArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<BootsOfSpeedManipulationArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class VampirismCrystal : RoguelikeAchievement {
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
public class HeartOfEarth : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<HeartOfEarthArtifact>();
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class ManaOverloader : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<ManaOverloaderArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class EssenceLantern : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<EssenceLanternArtifact>() && UniversalSystem.NotNormalMode();
	}
}
public class AlchemistKnowledge : RoguelikeAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Easy;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod() && Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>() && UniversalSystem.NotNormalMode();
	}
}
