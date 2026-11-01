using Newtonsoft.Json.Linq;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.BossRushMode;
using Roguelike.Common.Systems.WorldSettingSystem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
internal class NPCStatsHandlers : GlobalNPC {
	public const int BossHP = 6500;
	public const int BossDMG = 30;
	public const int BossDef = 5;
	public float GetValueMulti(float scale = 1) {
		float extraMultiply = .05f;
		if (Main.expertMode) {
			extraMultiply += .15f;
		}
		if (Main.masterMode) {
			extraMultiply += .3f;
		}
		if (ModContent.GetInstance<RoguelikeWorldProperty>().NightmareWorld) {
			extraMultiply = 1f;
			scale += .1f;
		}
		int counter = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count;
		if (ModContent.GetInstance<RoguelikeWorldProperty>().BossRushWorld) {
			extraMultiply *= ModContent.GetInstance<UniversalSystem>().Count_BossKill * .25f;
		}
		return (1 + counter * .3f + extraMultiply) * scale;
	}
	public override void SetDefaults(NPC entity) {
		DifficultySettingSystem difficulty = ModContent.GetInstance<DifficultySettingSystem>();
		if (entity.boss) {
			entity.GetGlobalNPC<RoguelikeGlobalNPC>().Static_PercentageDamage += .025f * difficulty.Boss_DMGPercentage;
		}
		if (ModContent.GetInstance<RoguelikeWorldProperty>().NightmareWorld) {
			entity.GetGlobalNPC<RoguelikeGlobalNPC>().ExtraUpdate++;
			if (entity.boss) {
				entity.GetGlobalNPC<RoguelikeGlobalNPC>().Static_Endurance += .25f;
				entity.GetGlobalNPC<RoguelikeGlobalNPC>().Static_PercentageDamage += .1f;
			}
		}
	}
	//Most creatively bankrupt code
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		DifficultySettingSystem difficulty = ModContent.GetInstance<DifficultySettingSystem>();

		StatModifier HPmod = new();
		StatModifier DMGmod = new();
		HPmod += .1f * difficulty.Enemy_HP;
		DMGmod += .05f * difficulty.Enemy_DMG;
		if (npc.boss) {
			HPmod += .25f * difficulty.Boss_HP;
			DMGmod += .025f * difficulty.Boss_DMG;

		}
		if (Main.rand.NextFloat() <= difficulty.World_EnemyToElite * .2f && !npc.boss) {
			npc.lifeMax += 1000 + npc.lifeMax;
			npc.defense += 30;
			npc.damage += 50 + npc.damage / 2;
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().ExtraUpdate++;
		}
		if (ModContent.GetInstance<RoguelikeWorldProperty>().RoguelikeWorld
			|| ModContent.GetInstance<RoguelikeWorldProperty>().BossRushWorld
			|| ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			float value = GetValueMulti();
			if (npc.boss && npc.type != NPCID.WallofFlesh && npc.type != NPCID.WallofFleshEye) {
				if (!npc.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException) {
					HPmod += value;
					DMGmod += value;
					npc.lifeMax = (int)HPmod.ApplyTo(BossHP);
					if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism) {
						npc.lifeMax = (int)(npc.lifeMax * .7f);
					}
					npc.damage = (int)DMGmod.ApplyTo(BossDMG);
					npc.defense = (int)(BossDef * GetValueMulti(.5f));
				}
			}
			else {
				float adjustment = 1;
				if (Main.expertMode)
					adjustment = 2;
				else if (Main.masterMode)
					adjustment = 3;

				HPmod += adjustment * value * .1f;
				DMGmod += adjustment * value * .1f;
				npc.defense += (int)(npc.defense / adjustment * GetValueMulti(.5f) * .1f);
				npc.lifeMax = (int)HPmod.ApplyTo(npc.lifeMax);
				npc.damage = (int)DMGmod.ApplyTo(npc.damage);
			}
		}
		if (ModContent.GetInstance<RoguelikeWorldProperty>().NightmareWorld) {
			StatModifier mod = new();
			mod += 2;
			npc.damage *= 2;
			if (npc.boss) {
				mod += 5;
			}
			npc.lifeMax = (int)mod.ApplyTo(npc.lifeMax);
			npc.life = npc.lifeMax;
		}
		if (npc.boss) {
			npc.lifeMax /= 2;
			npc.damage /= 2;
			if (npc.type == NPCID.BrainofCthulhu) {
				npc.lifeMax = (int)(npc.lifeMax * .27f);
			}
		}
		if (ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier4>()) {
			npc.lifeMax += npc.lifeMax * 9;
			npc.life = npc.lifeMax;
		}
	}
}
