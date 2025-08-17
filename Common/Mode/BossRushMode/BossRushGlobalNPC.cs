using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;

namespace Roguelike.Common.Mode.BossRushMode
{
	internal class BossRushGlobalNPC : GlobalNPC {
		public override void SetDefaults(NPC entity) {
			//if(entity.boss && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			//	float multiplier = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count * .5f;
			//	if(Main.hardMode) {
			//		multiplier *= 2;
			//	}
			//	entity.lifeMax = (int)(3000 + (3000 * multiplier));
			//	entity.damage = (int)(30 + (30 * multiplier));
			//}
		}
		public override bool PreAI(NPC npc) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				return base.PreAI(npc);
			}
			if (npc.type == NPCID.OldMan) {
				return false;
			}
			if (npc.type == NPCID.TravellingMerchant) {
				return false;
			}
			return true;
		}
		public override void PostAI(NPC npc) {
		}
		public override void OnKill(NPC npc) {
			if (npc.type == NPCID.WallofFlesh && !Main.hardMode) {
				ModContent.GetInstance<UniversalSystem>().defaultUI.TurnOnEndOfDemoMessage();
			}
		}
	}
}
