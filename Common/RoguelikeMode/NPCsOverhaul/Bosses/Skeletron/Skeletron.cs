using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.NPCsOverhaul.Bosses.Skeletron;
internal class Skeletron : GlobalNPC {
	enum State {
		Skeletron,
		DungeonGuardian,
		Hover,
		Spin,
		Despawn,
	}
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.SkeletronHead;
	public override bool InstancePerEntity => true;
	public override void SetDefaults(NPC entity) {
		state = State.Skeletron;
		entity.aiStyle = -1;
	}
	State state = State.Skeletron;
	private bool DG_AI(NPC npc) {
		if (state == State.DungeonGuardian) {
			npc.damage = 1000;
			npc.defense = 9999;
			npc.rotation += (float)npc.direction * 0.3f;
			Vector2 vector21 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num176 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector21.X;
			float num177 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector21.Y;
			float num178 = (float)Math.Sqrt(num176 * num176 + num177 * num177);
			num178 = 8f / num178;
			npc.velocity.X = num176 * num178;
			npc.velocity.Y = num177 * num178;
			return true;
		}
		if (Main.IsItDay() && state != State.DungeonGuardian) {
			state = State.DungeonGuardian;
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
			return true;
		}
		return false;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		state = State.Hover;
		if (Main.netMode != NetmodeID.MultiplayerClient) {
			SoundEngine.PlaySound(SoundID.Roar, npc.position);
			npc.TargetClosest();

			int num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.SkeletronHand, npc.whoAmI);
			Main.npc[num148].ai[0] = -1f;
			Main.npc[num148].ai[1] = npc.whoAmI;
			Main.npc[num148].target = npc.target;
			Main.npc[num148].netUpdate = true;
			num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.SkeletronHand, npc.whoAmI);
			Main.npc[num148].ai[0] = 1f;
			Main.npc[num148].ai[1] = npc.whoAmI;
			Main.npc[num148].ai[3] = 150f;
			Main.npc[num148].target = npc.target;
			Main.npc[num148].netUpdate = true;

		}
	}
	private void CheckPlayerDead(NPC npc) {
		if (Main.player[npc.target].dead ||
			Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f ||
			Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f) {
			npc.TargetClosest();
			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
				state = State.Despawn;
		}
	}
	private void ShootCursedSkull(NPC npc, int handsCount) {
		Vector2 center3 = npc.Center;
		if (Collision.CanHit(center3, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
			float num155 = 3f;
			if (handsCount == 0)
				num155 += 2f;

			Vector2 playerCenter = Main.player[npc.target].Center + Main.rand.NextVector2Circular(50, 50);
			Vector2 vector19 = (playerCenter - center3).SafeNormalize(Vector2.Zero) * (num155 + Vector2.Distance(playerCenter, center3) * .1f);
			vector19 += npc.velocity;
			int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
			int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3, vector19, ProjectileID.Skull, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
			Main.projectile[num160].timeLeft = 300;
		}
	}
	private void ShootProjectile(NPC npc, Vector2 vel, int Type) {
		Vector2 center3 = npc.Center;

		Vector2 playerCenter = Main.player[npc.target].Center + Main.rand.NextVector2Circular(50, 50);
		int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
		int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3, vel, Type, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
		Main.projectile[num160].timeLeft = 300;
		Main.projectile[num160].hostile = true;
		Main.projectile[num160].friendly = false;
	}
	public override void AI(NPC npc) {
		if (DG_AI(npc)) {
			return;
		}
		npc.reflectsProjectiles = false;
		npc.defense = npc.defDefense;

		CheckPlayerDead(npc);

		int handsCount = 0;

		for (int num150 = 0; num150 < 200; num150++) {
			if (Main.npc[num150].active && Main.npc[num150].type == NPCID.SkeletronHand)
				handsCount++;
		}
		npc.defense += handsCount * 25;


		if (state == State.Hover) {
			Hover(npc, handsCount);
		}
		else if (state == State.Spin) {
			SpinAttack(npc, handsCount);
		}
		else if (state == State.Despawn) {
			Despawn(npc);
		}

		DustEffect(npc, handsCount);
	}
	private void Hover(NPC npc, int handsCount) {
		if (handsCount < 2 || npc.life < npc.lifeMax * 0.75) {
			float num151 = 80f;
			if (handsCount == 0)
				num151 /= 2f;

			if (Main.getGoodWorld)
				num151 *= 0.8f;

			if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % num151 == 0f) {
				ShootCursedSkull(npc, handsCount);
			}
		}

		npc.damage = npc.defDamage;
		if (++npc.ai[2] >= 800f) {
			npc.ai[2] = 0f;
			state = State.Spin;
			npc.TargetClosest();
			npc.netUpdate = true;
		}

		if (npc.ai[2] % 400 == 0) {
			Vector2 center3 = npc.Center;
			for (int i = 0; i < 16; i++) {
				Vector2 playerCenter = Main.player[npc.target].Center + Main.rand.NextVector2Circular(50, 50);
				Vector2 vector19 = (playerCenter - center3).SafeNormalize(Vector2.Zero);
				int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
				int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3,
					vector19.Vector2DistributeEvenlyPlus(16, 360, i) * 5
					, ProjectileID.Skull, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
				Main.projectile[num160].timeLeft = 300;
			}
		}

		npc.rotation = npc.velocity.X / 15f;

		float UpwardAcceleration = 0.03f;
		float VerticalSpeedCap = 4f;
		float HorizontalAcceleration = 0.07f;
		float HorizontalSpeedCap = 9.5f;

		if (Main.getGoodWorld) {
			UpwardAcceleration += 0.01f;
			VerticalSpeedCap += 1f;
			HorizontalAcceleration += 0.05f;
			HorizontalSpeedCap += 2f;
		}
		int VerticalDirection = npc.position.Y > Main.player[npc.target].position.Y - 250f ?
			1 : npc.position.Y < Main.player[npc.target].position.Y - 250f ? -1 : 0;
		if (VerticalDirection != 0) {
			if (npc.velocity.Y * VerticalDirection > 0f)
				npc.velocity.Y *= 0.98f;

			npc.velocity.Y -= UpwardAcceleration * VerticalDirection;
			npc.velocity.Y = Math.Clamp(npc.velocity.Y, -VerticalSpeedCap, VerticalSpeedCap);
		}
		int HorizontalDirection = npc.Center.X > Main.player[npc.target].Center.X ?
			1 : npc.Center.X < Main.player[npc.target].Center.X ? -1 : 0;
		if (VerticalDirection != 0) {
			if (npc.velocity.X * HorizontalDirection > 0f)
				npc.velocity.X *= 0.98f;

			npc.velocity.X -= HorizontalAcceleration * HorizontalDirection;
			npc.velocity.X = Math.Clamp(npc.velocity.X, -HorizontalSpeedCap, HorizontalSpeedCap);
		}
	}
	private void SpinAttack(NPC npc, int handsCount) {
		if (Main.getGoodWorld) {
			if (handsCount > 0) {
				npc.reflectsProjectiles = true;
			}
		}

		npc.defense -= 10;
		npc.ai[2] += 1f;
		if (npc.ai[2] == 2f)
			SoundEngine.PlaySound(SoundID.Roar, npc.position);

		if (npc.ai[2] % 40 == 0) {
			Vector2 vel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(60);
			ShootProjectile(npc, vel * 5, ProjectileID.Skull);
		}


		if (npc.ai[2] % 10 == 0) {
			Vector2 vel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
			ShootProjectile(npc, -vel, ProjectileID.DemonScythe);
		}

		if (npc.ai[2] % 120 == 0) {
			Vector2 velOrigin = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
			for (int i = 1; i <= 24; i++) {
				Vector2 vel = velOrigin.RotatedBy(MathHelper.TwoPi * MathHelper.Lerp(0, 1, (i % 8) / 8f));
				ShootProjectile(npc, vel * (i / 8 + 4),
					ProjectileID.ClothiersCurse);
			}
		}
		if (npc.ai[2] >= 400f) {
			npc.ai[2] = 0f;
			state = State.Hover;
		}

		npc.rotation += npc.direction * 0.3f;
		Vector2 npcCenter = npc.Center;
		Vector2 playerCenter = Main.player[npc.target].Center;
		float num174 = Vector2.Distance(npcCenter, playerCenter);
		float num175 = 3.5f;
		npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage, npc.defDamage * 1.3f);

		if (num174 > 150f)
			num175 *= 1.05f;

		if (num174 > 200f)
			num175 *= 1.1f;

		if (num174 > 250f)
			num175 *= 1.1f;

		if (num174 > 300f)
			num175 *= 1.1f;

		if (num174 > 350f)
			num175 *= 1.1f;

		if (num174 > 400f)
			num175 *= 1.1f;

		if (num174 > 450f)
			num175 *= 1.1f;

		if (num174 > 500f)
			num175 *= 1.1f;

		if (num174 > 550f)
			num175 *= 1.1f;

		if (num174 > 600f)
			num175 *= 1.1f;

		num175 *= 1.1f - .05f * handsCount;

		if (Main.getGoodWorld)
			num175 *= 1.3f;

		num174 = num175 / num174;
		npc.velocity = (playerCenter - npcCenter) * num174;
	}
	private void Despawn(NPC npc) {
		npc.velocity.Y += 0.1f;
		if (npc.velocity.Y < 0f)
			npc.velocity.Y *= 0.95f;

		npc.velocity.X *= 0.95f;
		npc.EncourageDespawn(50);
	}
	private void DustEffect(NPC npc, int handsCount) {
		if (state != State.DungeonGuardian && state != State.Despawn && handsCount != 0) {
			int num179 = Dust.NewDust(new Vector2(npc.position.X + (float)(npc.width / 2) - 15f - npc.velocity.X * 5f, npc.position.Y + (float)npc.height - 2f), 30, 10, DustID.Blood, (0f - npc.velocity.X) * 0.2f, 3f, 0, default, 2f);
			Main.dust[num179].noGravity = true;
			Main.dust[num179].velocity.X *= 1.3f;
			Main.dust[num179].velocity.X += npc.velocity.X * 0.4f;
			Main.dust[num179].velocity.Y += 2f + npc.velocity.Y;
			for (int num180 = 0; num180 < 2; num180++) {
				num179 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 120f), npc.width, 60, DustID.Blood, npc.velocity.X, npc.velocity.Y, 0, default, 2f);
				Main.dust[num179].noGravity = true;
				Dust dust = Main.dust[num179];
				dust.velocity -= npc.velocity;
				Main.dust[num179].velocity.Y += 5f;
			}
		}
	}
}
