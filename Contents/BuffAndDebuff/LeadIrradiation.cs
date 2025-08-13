using Roguelike.Common.Global;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.BuffAndDebuff
{
	internal class LeadIrradiation : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 50;
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base += 20;
		}
	}
}
