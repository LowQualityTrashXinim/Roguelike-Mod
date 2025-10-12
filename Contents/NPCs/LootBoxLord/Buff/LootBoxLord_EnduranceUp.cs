using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.Buff;
internal class LootBoxLord_EnduranceUp : ModBuff{
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		RoguelikeGlobalNPC global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.Endurance += .3f;
	}
}
