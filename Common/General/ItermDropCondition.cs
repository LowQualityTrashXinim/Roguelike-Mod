using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Roguelike.Common.Systems;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;
using Roguelike.Common.Mode.BossRushMode;

namespace Roguelike.Common.General {
	public class IsInBossRushMode : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation && info.npc.TryGetGlobalNPC(out RoguelikeGlobalNPC npc)) {
				return ModContent.GetInstance<RogueLikeConfig>().BossRushMode && RoguelikeWorldProperty.BossRushWorld;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "";
	}
	public class DenyYouFromLoot : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation && info.npc.TryGetGlobalNPC(out RoguelikeGlobalNPC npc)) {
				return !npc.CanDenyYouFromLoot && !npc.IsAGhostEnemy;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => false;
		public string GetConditionDescription() => "deny you from loot regardless";
	}
	public class NoHitAndIsRakan : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation)
				return info.player.GetModPlayer<ModdedPlayer>().Secret_MrRakan;
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "";
	}
	public class GitGudMode : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return info.player.GetModPlayer<ModdedPlayer>().amountOfTimeGotHit == 0
					&& (
					info.player.difficulty == PlayerDifficultyID.Hardcore
					|| info.player.IsDebugPlayer());
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player beat boss without getting hit";
	}
	public class DontHitBoss : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.GetModPlayer<ModdedPlayer>().ItemIsUsedDuringBossFight
					&& (
					info.player.difficulty == PlayerDifficultyID.Hardcore
					|| info.player.IsDebugPlayer());
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player beat boss in no hit aka git gud mode";
	}
	public class PerkDrop : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count < 1
					|| ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count == 5;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => false;
		public string GetConditionDescription() => "";
	}
	public class SkillUnlockRule : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count >= 3 && info.player.GetModPlayer<SkillHandlePlayer>().AvailableSkillActiveSlot <= 9 &&
					UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "";
	}
	public class LifeCrystalDrop : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return info.player.ConsumedLifeCrystals < Player.LifeCrystalMax &&
					UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
			}
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "";
	}
	public class ManaCrystalDrop : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return info.player.ConsumedManaCrystals < Player.ManaCrystalMax &&
					UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE);
			}
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "";
	}
}
