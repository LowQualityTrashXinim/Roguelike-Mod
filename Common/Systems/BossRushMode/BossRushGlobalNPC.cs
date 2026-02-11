using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems;

namespace Roguelike.Common.Systems.BossRushMode
{
	internal class BossRushGlobalNPC : GlobalNPC {
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
