using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.LootBoxLord.Buff;
internal class LootboxLord_FinalDomain : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
		Main.buffNoTimeDisplay[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex) {
		if (!NPC.AnyNPCs(ModContent.NPCType<LootBoxLord>())) {
			player.DelBuff(buffIndex);
			return;
		}
		else {
			player.buffTime[buffIndex] = 2;
			player.ModPlayerStats().HealEffectiveness *= 0;
			player.ModPlayerStats().Rapid_LifeRegen = 0;
			player.ModPlayerStats().UpdateHPRegen *= 0;
			player.lifeRegenTime = 0;
		}
	}
}
