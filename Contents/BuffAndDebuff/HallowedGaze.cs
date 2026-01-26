using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.BuffAndDebuff;
internal class HallowedGaze : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.buffTime[buffIndex] = time;
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().HallowedGaze_Count++;
		return base.ReApply(npc, time, buffIndex);
	}
}
