//EnragedStuff
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System;
using Roguelike.Common.General;
using Roguelike.Common.Systems;
using Roguelike.Contents.Items;
using Roguelike.Contents.Items.Consumable;
using Roguelike.Contents.Items.Consumable.Spawner;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.Lootbox.BossLootBox;
using Roguelike.Contents.Items.Lootbox.SpecialLootbox;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Contents.Items.Lootbox.DisableLootbox;

namespace Roguelike.Common.Global {
	class GlobalNPCMod : GlobalNPC {
		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (!npc.boss && Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.type != NPCID.Creeper) {
				npc.damage += Main.rand.Next((int)(npc.damage * .5f) + 1);
				npc.lifeMax += Main.rand.Next((int)(npc.lifeMax * .5f) + 1);
				npc.defense += Main.rand.Next((int)(npc.defense * .5f) + 1);
				npc.life = npc.lifeMax;
			}
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff<Marked>()) {
				modifiers.CritDamage += 1;
			}
		}
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff<Marked>()) {
				modifiers.CritDamage += 1;
			}
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			var ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
			var noHit = new LeadingConditionRule(new GitGudMode());
			var dontHit = new LeadingConditionRule(new DontHitBoss());
			LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
			if (npc.boss) {
				npcLoot.Add(ItemDropRule.ByCondition(new LootBoxLordDrop(), ModContent.ItemType<LootboxLordSummon>()));
			}
			if (npc.type == NPCID.KingSlime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSDonHitReward>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.EyeofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCDonHitReward>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (Array.IndexOf([NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail], npc.type) > -1) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWDonHitReward>()));

				IsABoss.OnSuccess(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.BrainofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCDonHitReward>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.QueenBee) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeDonHitReward>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.SkeletronHead) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronDonHitReward>()));
				npcLoot.Add(ItemDropRule.BossBagByCondition(new NoHitAndIsRakan(), ItemID.Handgun));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Deerclops) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopDonHitReward>()));

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.WallofFlesh) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshNoHitReward>()));
				noHit.OnSuccess(ItemDropRule.ByCondition(new NoHitAndIsRakan(), ModContent.ItemType<WeaponBluePrint>())).OnFailedConditions(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WeaponBluePrint>(), 100));

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

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				//NoHit mode drop

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) {
				var leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
				//NoHit Mode drop

				//Expert mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechLootBox>()));
				npcLoot.Add(leadingConditionRule);
			}
			else if (npc.type == NPCID.Plantera) {
				//NoHit mode drop

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.Golem) {
				//NoHit mode drop

				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.HallowBoss) {
				//NoHit mode drop

				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.EmpressOfLightIsGenuinelyEnraged(), ModContent.ItemType<EmpressLootBox>()));

				//Normal boss drop
				npcLoot.Add(ItemDropRule.ByCondition(new IsInBossRushMode(), ModContent.ItemType<WoodenLootBox>()));
			}
			else if (npc.type == NPCID.DukeFishron) {
				//NoHit mode drop

				//Enraged boss drop


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
			}
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new LifeCrystalDrop(), ItemID.LifeCrystal));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new ManaCrystalDrop(), ItemID.ManaCrystal));
			LeadingConditionRule perkrule = new(new PerkDrop());
			perkrule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<WorldEssence>()));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new SkillUnlockRule(), ModContent.ItemType<SkillSlotUnlock>()));
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
