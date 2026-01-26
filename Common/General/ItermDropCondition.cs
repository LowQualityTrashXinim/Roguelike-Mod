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
				return ModContent.GetInstance<RogueLikeConfig>().BossRushMode && ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
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
	public class Droprule_GhostNPC : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation && info.npc.TryGetGlobalNPC(out RoguelikeGlobalNPC npc)) {
				return !npc.IsAGhostEnemy;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => false;
		public string GetConditionDescription() => "You should not be able to see this";
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
	public class QueenBeeEnranged : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.ZoneJungle;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drops if player enrage queen bee";
	}
	public class DukeIsEnrage : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return !info.player.ZoneBeach;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drops if player aren't at beach";
	}
	public class DeerclopHateYou : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return Main.raining && !Main.dayTime && info.player.ZoneSnow;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player is fighting in snow biome, in night and is snowing";
	}
	public class IsNotABossAndBossIsAlive : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModUtils.IsAnyVanillaBossAlive() && !info.npc.boss;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop only when npc is not a boss and boss is alive";
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
	public class LootBoxLordDrop : IItemDropRuleCondition {
		public bool CanDrop(DropAttemptInfo info) {
			if (!info.IsInSimulation) {
				return ModUtils.HasPlayerKillThisNPC(NPCID.KingSlime) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.EyeofCthulhu) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.BrainofCthulhu) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.EaterofWorldsHead) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.SkeletronHead) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.QueenBee) &&
					ModUtils.HasPlayerKillThisNPC(NPCID.Deerclops) && !Main.hardMode;
			}
			return false;
		}
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => "Drop if player beat all of pre HM bosses in pre HM";
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
