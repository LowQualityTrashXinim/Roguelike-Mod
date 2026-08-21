//EnragedStuff
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System;
using Roguelike.Common.General;
using Roguelike.Common.Systems;
using Roguelike.Contents.Items;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.Lootbox.BossLootBox;
using Roguelike.Contents.Items.Lootbox.SpecialLootbox;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Contents.Items.Lootbox.DisableLootbox;
using Roguelike.Common.Utils;

namespace Roguelike.Common.RoguelikeMode {
	class RoguelikeModifyNPCLoot : GlobalNPC {
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			var ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
			var noHit = new LeadingConditionRule(new GitGudMode());
			var dontHit = new LeadingConditionRule(new DontHitBoss());
			LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
			if (npc.type == NPCID.KingSlime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.KingSlimeBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.EyeofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.EyeOfCthulhuBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (Array.IndexOf([NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail], npc.type) > -1) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.EaterOfWorldsBossBag);

				IsABoss.OnSuccess(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.BrainofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.BrainOfCthulhuBossBag);
				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.QueenBee) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.QueenBeeBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.SkeletronHead) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronDonHitReward>()));
				npcLoot.Add(ItemDropRule.BossBagByCondition(new NoHitAndIsRakan(), ItemID.Handgun));

				npcLoot.Disable_BossBagDropRule(ItemID.SkeletronBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Deerclops) {
				var expert = new LeadingConditionRule(new Conditions.IsExpert());
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopDonHitReward>()));

				npcLoot.Disable_BossBagDropRule(ItemID.DeerclopsBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.WallofFlesh) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshNoHitReward>()));
				noHit.OnSuccess(ItemDropRule.ByCondition(new NoHitAndIsRakan(), ModContent.ItemType<WeaponBluePrint>())).OnFailedConditions(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WeaponBluePrint>(), 100));

				npcLoot.Disable_BossBagDropRule(ItemID.WallOfFleshBossBag);

				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshDonHitReward>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<WorldEssence>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.BloodNautilus) {
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodLootBox>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BloodLootBox>()));
			}
			else if (npc.type == NPCID.QueenSlimeBoss) {
				//NoHit mode drop

				npcLoot.Disable_BossBagDropRule(ItemID.QueenSlimeBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				//NoHit mode drop

				npcLoot.Disable_BossBagDropRule(ItemID.DestroyerBossBag);
				npcLoot.Disable_BossBagDropRule(ItemID.SkeletronPrimeBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) {
				var leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
				//NoHit Mode drop

				//Expert mode drop
				npcLoot.Disable_BossBagDropRule(ItemID.TwinsBossBag);

				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WoodenLootBox>()));
				npcLoot.Add(leadingConditionRule);
			}
			else if (npc.type == NPCID.Plantera) {
				//NoHit mode drop

				npcLoot.Disable_BossBagDropRule(ItemID.PlanteraBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Golem) {
				//NoHit mode drop

				npcLoot.Disable_BossBagDropRule(ItemID.GolemBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.HallowBoss) {
				//NoHit mode drop

				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.EmpressOfLightIsGenuinelyEnraged(), ModContent.ItemType<EmpressLootBox>()));

				npcLoot.Disable_BossBagDropRule(ItemID.FairyQueenBossBag);

				//Normal boss drop
				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.DukeFishron) {
				//NoHit mode drop

				//Enraged boss drop

				npcLoot.Disable_BossBagDropRule(ItemID.FishronBossBag);

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.CultistBoss) {
				//NoHit mode drop

				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>()));
			}
			else if (npc.type == NPCID.MoonLordCore) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>(), 1, 2, 2));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MoonLootBox>()));

				npcLoot.Disable_BossBagDropRule(ItemID.MoonLordBossBag);
			}
			LeadingConditionRule perkrule = new(new PerkDrop());
			perkrule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<WorldEssence>()));
			npcLoot.Add(perkrule);
			npcLoot.Add(noHit);
			npcLoot.Add(dontHit);
			npcLoot.Add(ExpertVSnormal);
			npcLoot.Add(IsABoss);
		}
		public override void OnKill(NPC npc) {
			if (npc.boss) {
				var system = ModContent.GetInstance<UniversalSystem>();
				system.ListOfBossKilled.Add(npc.type);
				system.Count_BossKill++;
			}
		}
	}
}
