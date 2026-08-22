using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.BuffAndDebuff;
internal class Anti_Immunity : ModBuff{
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		Array.Fill(npc.buffImmune, false);
	}
	public override void Update(Player player, ref int buffIndex) {
		Array.Fill(player.buffImmune, false);
	}
}
public class AntiImmunity_ModPlayer : ModPlayer {
	public override void ResetEffects() {
		Player.buffImmune[ModContent.BuffType<Anti_Immunity>()] = false;
	}
	public override void PostUpdateBuffs() {
		if (Player.HasBuff<Anti_Immunity>()) {
			Array.Fill(Player.buffImmune, false);
		}
	}
}
public class AntiImmunity_GlobalNPC : GlobalNPC {
	public override void ResetEffects(NPC npc) {
		npc.buffImmune[ModContent.BuffType<Anti_Immunity>()] = false;
	}
}
public class AntiImmunity_ModSystem : ModSystem {
	public override void Load() {
		On_Player.DelBuff += On_Player_DelBuff;
		On_NPC.DelBuff += On_NPC_DelBuff;
	}
	private void On_NPC_DelBuff(On_NPC.orig_DelBuff orig, NPC self, int buffIndex) {
		if (self.HasBuff<Anti_Immunity>()) {
			if (self.buffImmune[self.buffType[buffIndex]]) {
				Array.Fill(self.buffImmune, false);
				return;
			}
		}
		orig(self, buffIndex);
	}

	private void On_Player_DelBuff(On_Player.orig_DelBuff orig, Player self, int b) {
		if (self.HasBuff<Anti_Immunity>()) {
			if (self.buffImmune[self.buffType[b]]) {
				Array.Fill(self.buffImmune, false);
				return;
			}
		}
		if (b < self.buffType.Length) {
			orig(self, b);
		}
	}
}
