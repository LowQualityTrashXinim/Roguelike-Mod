using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Arguments.Contents;
public class ReactiveDefenses : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Microsoft.Xna.Framework.Color.MediumBlue;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (chargeNum >= 1 && Main.rand.NextFloat() <= .15f && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (chargeNum >= 2 && Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (chargeNum >= 3 && Main.rand.NextBool(10) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (chargeNum >= 1 && Main.rand.NextFloat() <= .15f && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (chargeNum >= 2 && Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
		if (chargeNum >= 3 && Main.rand.NextBool(10) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), ModUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
}
public class ReactiveHealingBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 10);
	}
}

public class ReactiveDefenseBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.endurance += .1f;
	}
}
public class ReactiveDefenseIIBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, 1.1f, Flat: 6);
	}
}
