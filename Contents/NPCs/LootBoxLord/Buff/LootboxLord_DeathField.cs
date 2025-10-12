using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.Buff;
internal class LootboxLord_DeathField : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		if (!NPC.AnyNPCs(ModContent.NPCType<LootBoxLord>())) {
			player.DelBuff(buffIndex);
			return;
		}
		if (player.buffTime[buffIndex] <= 2) {
			player.EasyKillPlayer($"{player.name} has fail to kill Loot box Lord in time", 1);
			player.buffTime[buffIndex] = 2;
		}
	}
}
