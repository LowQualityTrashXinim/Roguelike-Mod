using Roguelike.Common.Global;
using Roguelike.Common.Mode.BossRushMode;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode;
internal class RoguelikeCommonNPC : GlobalNPC {
	public const int BossHP = 8000;
	public const int BossDMG = 40;
	public const int BossDef = 5;
	public override void SetDefaults(NPC entity) {
		if (!ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld && !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
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
	}
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		if (!ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld && !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
			return;
		}
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
			npc.life = npc.lifeMax;
			npc.damage += (int)(npc.damage * GetValueMulti() * .1f);
			npc.defense += (int)(npc.defense * GetValueMulti(.5f) * .1f);
		}
	}
	public float GetValueMulti(float scale = 1) {
		float extraMultiply = 0;
		if (Main.expertMode) {
			extraMultiply += .15f;
		}
		if (Main.masterMode) {
			extraMultiply += .3f;
		}
		int counter = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count;
		return (1 + counter * .5f + extraMultiply) * scale;
	}
}
