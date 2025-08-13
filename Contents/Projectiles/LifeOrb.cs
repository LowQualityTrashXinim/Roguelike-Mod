﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
 
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Projectiles
{
	internal class LifeOrb : ModProjectile {
		public override string Texture => ModTexture.SMALLWHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}
		public override bool? CanDamage() => false;
		public override Color? GetAlpha(Color lightColor) {
			return new Color(0, 255, 0);
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.timeLeft == 150) {
				for (int i = 0; i < 50; i++) {
					int startdust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemEmerald);
					Main.dust[startdust].velocity = Main.rand.NextVector2CircularEdge(6, 6);
					Main.dust[startdust].noGravity = true;
				}
			}
			int dust = Dust.NewDust(Projectile.Center - new Vector2(5, 5) + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemEmerald);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			if (Projectile.Center.IsCloseToPosition(player.Center, 225)) {
				Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .5f;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(6);
			}
			else {
				Projectile.velocity *= .98f;
			}
			if (player is not null & Projectile.Center.IsCloseToPosition(player.Center, 25)) {
				Projectile.timeLeft = 150;
				int healing = player.statLifeMax2 % 100 + 5;
				player.Heal(healing);
				player.AddBuff(ModContent.BuffType<LifeForce>(), ModUtils.ToSecond(2));
				Projectile.Kill();
			}
		}
	}
	public class LifeForce : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override bool ReApply(Player player, int time, int buffIndex) {
			if (player.buffTime[buffIndex] < ModUtils.ToSecond(10)) {
				player.buffTime[buffIndex] = Math.Clamp(time + player.buffTime[buffIndex], 0, ModUtils.ToSecond(10));
			}
			return base.ReApply(player, time, buffIndex);
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MaxHP, Additive: 1.1f);
		}
	}
}
