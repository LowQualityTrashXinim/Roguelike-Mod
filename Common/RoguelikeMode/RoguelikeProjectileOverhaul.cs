using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode;
internal class RoguelikeProjectileOverhaul : GlobalProjectile {
	public override void SetDefaults(Projectile entity) {
		if (entity.type == ProjectileID.ChlorophyteOrb) {
			entity.penetrate = 1;
		}
		if (Main.LocalPlayer.strongBees) {
			if (entity.type == ProjectileID.BeeArrow) {
				entity.extraUpdates += 1;
			}
		}
	}
	public override bool PreAI(Projectile projectile) {
		var player = Main.player[projectile.owner];
		if (player.strongBees) {
			if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
				projectile.velocity /= 1.25f;
			}
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		var player = Main.player[projectile.owner];
		if (player.strongBees) {
			if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
				projectile.velocity *= 1.25f;
			}
		}
	}
}
