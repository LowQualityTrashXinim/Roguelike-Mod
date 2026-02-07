using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles;
internal class ElectricChainBolt : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 2000;
		Projectile.penetrate = 10;
		Projectile.extraUpdates = 100;
	}
	NPC npc = null;
	public override void AI() {
		int electic = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric);
		Main.dust[electic].noGravity = true;
		Main.dust[electic].velocity = Vector2.Zero;
		if (npc == null) {
			float distance = 1000 * 1000;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC mainnpc = Main.npc[i];
				if (mainnpc.active
					&& ModUtils.CompareSquareFloatValue(Projectile.Center, mainnpc.Center, distance, out float dis)
					&& mainnpc.CanBeChasedBy()
					&& !mainnpc.friendly
					&& Collision.CanHitLine(Projectile.position, 10, 10, mainnpc.position, mainnpc.width, mainnpc.height)
					&& mainnpc.immune[Projectile.owner] <= 0
					) {
					distance = dis;
					npc = mainnpc;
				}
			}
			return;
		}
		Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Electrified, ModUtils.ToSecond(Main.rand.Next(13, 17)));
		Main.player[Projectile.owner].GetModPlayer<SkillHandlePlayer>().Modify_EnergyAmount(5);
		npc = null;
	}
}

public class MagnetOrbProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.MagnetSphereBall);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 40;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Main.projFrames[Type] = 5;
	}
	public override void AI() {
		if (Projectile.ai[0] == 0f) {
			Projectile.ai[0] = Projectile.velocity.X;
			Projectile.ai[1] = Projectile.velocity.Y;
		}
		if(Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
			dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(.2f, .6f);
		}

		if (Projectile.velocity.X > 0f)
			Projectile.rotation += (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;
		else
			Projectile.rotation -= (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * 0.001f;

		Projectile.frameCounter++;
		if (Projectile.frameCounter > 6) {
			Projectile.frameCounter = 0;
			Projectile.frame++;
			if (Projectile.frame > 4)
				Projectile.frame = 0;
		}

		if (Projectile.velocity.Length() > 2f)
			Projectile.velocity *= 0.98f;
		if (++Projectile.ai[2] < 10) {
			return;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 450)) {
			Vector2 vel = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.MagnetSphereBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
			Projectile.ai[2] = 0;
		}
	}
}
