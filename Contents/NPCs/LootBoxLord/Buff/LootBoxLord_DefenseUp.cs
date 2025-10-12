using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.Buff;
internal class LootBoxLord_DefenseUp : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		 global.StatDefense.Base += 200;
		global.Endurance += .1f;
	}
}
