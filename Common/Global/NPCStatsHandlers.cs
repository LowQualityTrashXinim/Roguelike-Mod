using Roguelike.Common.Systems;
using Roguelike.Common.Systems.BossRushMode;
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
		if (RoguelikeWorldProperty.NightmareWorld) {
			extraMultiply = 1f;
			scale += .1f;
		}
		int counter = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count;
		if (RoguelikeWorldProperty.BossRushWorld) {
			extraMultiply *= ModContent.GetInstance<UniversalSystem>().Count_BossKill * .25f;
		}
		return (1 + counter * .3f + extraMultiply) * scale;
	}
	public override void SetDefaults(NPC entity) {
		StatModifier mod = new();
		if (Main.ActiveWorldFileData.GameMode == GameModeID.Creative) {
			return;
		}
		if (RoguelikeWorldProperty.RoguelikeWorld || RoguelikeWorldProperty.BossRushWorld) {
			if (entity.boss && entity.type != NPCID.WallofFlesh && entity.type != NPCID.WallofFleshEye) {
				if (!entity.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException) {
					entity.lifeMax = (int)(BossHP * GetValueMulti());
					entity.damage = (int)(BossDMG * GetValueMulti());
					entity.defense = (int)(BossDef * GetValueMulti(.5f));
				}
			}
			else {
				float adjustment = 1;
				if (Main.expertMode)
					adjustment = 2;
				else if (Main.masterMode)
					adjustment = 3;

				entity.lifeMax += (int)(entity.lifeMax / adjustment * GetValueMulti() * .1f);
				entity.life = entity.lifeMax;
				entity.damage += (int)(entity.damage / adjustment * GetValueMulti() * .1f);
				entity.defense += (int)(entity.defense / adjustment * GetValueMulti(.5f) * .1f);
			}
			if (RoguelikeWorldProperty.NightmareWorld) {
				mod += 2;
				entity.damage *= 2;
				//ExtraUpdate++;
				//if (entity.boss) {
				//	mod += 5;
				//	Static_Endurance += .25f;
				//	Static_PercentageDamage += .1f;
				//}
				entity.lifeMax = (int)mod.ApplyTo(entity.lifeMax);
				entity.life = entity.lifeMax;
			}
		}
	}
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		if (Main.ActiveWorldFileData.GameMode == GameModeID.Creative) {
			return;
		}
		if (RoguelikeWorldProperty.RoguelikeWorld || RoguelikeWorldProperty.BossRushWorld) {
			if (npc.boss && npc.type != NPCID.WallofFlesh && npc.type != NPCID.WallofFleshEye
			&& npc.type != NPCID.MoonLordCore && npc.type != NPCID.MoonLordHand && npc.type != NPCID.MoonLordHead && npc.type != NPCID.MoonLordLeechBlob) {
				if (!npc.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException) {
					npc.lifeMax = (int)(BossHP * GetValueMulti());
					npc.life = npc.lifeMax;
					npc.damage = (int)(BossDMG * GetValueMulti());
					npc.defense = (int)(BossDef * GetValueMulti(.5f));
				}
			}
			else {
				npc.lifeMax += (int)(npc.lifeMax * GetValueMulti() * .1f);
				if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism) {
					npc.lifeMax = (int)(npc.lifeMax * .7f);
				}
				npc.life = npc.lifeMax;
				npc.damage += (int)(npc.damage * GetValueMulti() * .1f);
				npc.defense += (int)(npc.defense * GetValueMulti(.5f) * .1f);
			}
			StatModifier mod = new();
			if (RoguelikeWorldProperty.NightmareWorld) {
				mod += 2;
				npc.damage *= 2;
				if (npc.boss) {
					mod += 5;
				}
				npc.lifeMax = (int)mod.ApplyTo(npc.lifeMax);
				npc.life = npc.lifeMax;
			}

			if (ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier4>()) {
				npc.lifeMax += npc.lifeMax * 9;
				npc.life = npc.lifeMax;
			}
		}
	}
}
