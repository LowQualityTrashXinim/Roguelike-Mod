﻿using Roguelike.Common.Utils;
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
