using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.SummonerSynergyWeapon.StickySlime;
internal class GhostSlime : ModProjectile {
	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = 8;
		ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		Main.projPet[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
	}

	public override void SetDefaults() {
		Projectile.width = 30;
		Projectile.height = 52;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.minion = true;
		Projectile.minionSlots = 1;
	}

	public override bool MinionContactDamage() {
		return true;
	}

	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
		if (!CheckActive(player)) {
			return;
		}
		Vector2 idlePosition = player.Center;

		float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
		idlePosition.X += minionPositionOffsetX; // Go behind the player


		Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
		float distanceToIdlePosition = vectorToIdlePosition.Length();

		if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f) {
			Projectile.position = idlePosition;
			Projectile.velocity *= 0.1f;
		}

		float overlapVelocity = 0.04f;
		Projectile.localAI[2] = 0;
		for (int i = 0; i < Main.maxProjectiles; i++) {
			Projectile other = Main.projectile[i];

			if (i != Projectile.whoAmI
				&& other.active
				&& other.owner == Projectile.owner) {
				if (Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
					if (Projectile.position.X < other.position.X) {
						Projectile.velocity.X -= overlapVelocity;
					}
					else {
						Projectile.velocity.X += overlapVelocity;
					}

					if (Projectile.position.Y < other.position.Y) {
						Projectile.velocity.Y -= overlapVelocity;
					}
					else {
						Projectile.velocity.Y += overlapVelocity;
					}
				}
				if (Projectile.minionPos == 0) {
					Projectile.localAI[2]++;
				}
			}
		}
		// Starting search distance
		float distanceFromTarget = 1500f;
		Vector2 targetCenter = Projectile.position;
		NPC npc = null;
		bool foundTarget;
		// This code is required if your minion weapon has the targeting feature
		if (player.HasMinionAttackTargetNPC) {
			NPC npc1 = Main.npc[player.MinionAttackTargetNPC];
			float between = Vector2.Distance(npc1.Center, player.Center);
			npc = npc1;
			if (between < 2000f) {
				distanceFromTarget = between;
				targetCenter = npc1.Center;
			}
		}
		else {
			Projectile.Center.LookForHostileNPC(out List<NPC> targetlist, distanceFromTarget);
			foreach (NPC target in targetlist) {
				if (ModUtils.CompareSquareFloatValue(target.Center, Projectile.Center, distanceFromTarget, out float dis)) {
					distanceFromTarget = dis;
					npc = target;
					targetCenter = npc.Center;
				}
			}
			if (npc != null) {
				distanceFromTarget = Vector2.Distance(npc.Center, player.Center);
			}
		}
		foundTarget = npc != null;
		Projectile.friendly = foundTarget;
		//Movement
		// Default movement parameters (here for attacking)
		float speed = 8f + Projectile.localAI[2] * .25f;
		float inertia = 20f;
		if (foundTarget) {
			bool AnyBossAlive = UniversalSystem.AnyVanillaBossAlive;
			if (distanceFromTarget < 650f && (Projectile.minionPos < 1 && AnyBossAlive || !AnyBossAlive)) {
				// Minion has a target: attack (here, fly towards the enemy)

				// The immediate range around the target (so it doesn't latch onto it when close)
				Vector2 direction = targetCenter - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;

			}
			else {
				if (distanceToIdlePosition > 600f) {
					speed = 12f;
					inertia = 60f;
				}
				else {
					speed = 4f;
					inertia = 80f;
				}

				if (distanceToIdlePosition > 20f) {
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero) {
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}
			if (distanceFromTarget >= 650f || Projectile.ai[0] >= 40 || Projectile.minionPos >= 1 && AnyBossAlive) {
				if (++Projectile.ai[0] >= 40) {
					if (Projectile.ai[0] % 3 == 0) {
						Projectile.netUpdate = true;
						Vector2 AimTo = (targetCenter - Projectile.Center).SafeNormalize(Vector2.UnitX) * 20f;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, AimTo.Vector2RotateByRandom(10), ModContent.ProjectileType<GooP>(), (int)(Projectile.damage * .45f), 0, Projectile.owner);
					}
					if (Projectile.ai[0] >= 48) {
						Projectile.ai[0] = Main.rand.Next(-2, 2) * 10;
					}
				}
			}
		}
		else {
			if (distanceToIdlePosition > 600f) {
				speed = 12f;
				inertia = 60f;
			}
			else {
				speed = 4f;
				inertia = 80f;
			}

			if (distanceToIdlePosition > 20f) {
				// The immediate range around the player (when it passively floats about)

				// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
				vectorToIdlePosition.Normalize();
				vectorToIdlePosition *= speed;
				Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
			}
			else if (Projectile.velocity == Vector2.Zero) {
				Projectile.velocity.X = -0.15f;
				Projectile.velocity.Y = -0.05f;
			}
		}
		SelectFrame();
		Projectile.rotation = Projectile.velocity.X * 0.05f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Projectile.minionPos == 0) {
			modifiers.SourceDamage += .1f * Projectile.localAI[2];
			Projectile.localAI[2] = 0;
		}
	}
	private bool CheckActive(Player owner) {
		if (owner.dead || !owner.active) {
			owner.ClearBuff(ModContent.BuffType<StickyFriend>());

			return false;
		}

		if (owner.HasBuff(ModContent.BuffType<StickyFriend>())) {
			Projectile.timeLeft = 2;
		}
		return true;
	}
	public void SelectFrame() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= Main.projFrames[Projectile.type]) {
				Projectile.frame = 0;
			}
		}
	}
}
internal class GooP : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = 40;
		Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 200;
		Projectile.scale = .5f;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.timeLeft <= 160) {
			if (Projectile.velocity.Y > 15f) Projectile.velocity.Y += 0.05f;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
