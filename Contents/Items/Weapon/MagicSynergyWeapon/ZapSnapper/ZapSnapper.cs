using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.MagicSynergyWeapon.ZapSnapper {
	public class ZapSnapper : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.WeatherPain, $"[i:{ItemID.WeatherPain}] You sometime shoot out a super charge thunder shot");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.ThunderStaff, $"[i:{ItemID.ThunderStaff}] You shoot out thunder bolt");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMagic(56, 16, 12, 2f, 5, 5, ItemUseStyleID.Shoot, ProjectileID.ThunderSpearShot, 22, 4, true);

			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item9;
		}
		public int Counter = 0;
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.WeatherPain);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.ThunderStaff);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 30);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			Counter += Main.rand.Next(1, 3);
			CanShootItem = true;
			if (Counter < 15) {
				if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ThunderStaff)) {
					int projectile = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30).Vector2RandomSpread(5, Main.rand.NextFloat(1, 1.2f)) * .15f, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
					Main.projectile[projectile].extraUpdates += 3;
				}
				return;
			}
			Counter = 0;
			int amount = Main.rand.Next(20, 30);
			for (int i = 0; i < amount; i++) {
				Vector2 newVec = velocity.Vector2DistributeEvenly(amount, 30, i).Vector2RotateByRandom(10).Vector2RandomSpread(2, Main.rand.NextFloat(.5f, 1.5f));
				int proj = Projectile.NewProjectile(source, position, newVec, type, damage, knockback, player.whoAmI);
				Main.projectile[proj].DamageType = DamageClass.Magic;
				if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ThunderStaff) && Main.rand.NextBool(3)) {
					int projectile = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30).Vector2RandomSpread(5, Main.rand.NextFloat(1, 1.2f)) * .15f, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
					Main.projectile[projectile].extraUpdates += 3;
				}
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WeatherPain)) {
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LightningStrike>(), damage * 4, knockback, player.whoAmI, 10);
				float[] variant = Array.Empty<float>();
				Array.Resize(ref variant, SoundID.Thunder.Variants.Length);
				variant[0] = 1f;
				SoundEngine.PlaySound(SoundID.Thunder with { Pitch = -1, MaxInstances = 5, VariantsWeights = variant.AsSpan() }, player.Center);
			}
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-10, 2);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.RedRyder)
				.AddIngredient(ItemID.ThunderSpear)
				.Register();
		}
	}
	public class LightningStrike : ModProjectile {
		public override string Texture => ModTexture.SMALLWHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.hide = true;
			Projectile.timeLeft = 999;
			Projectile.penetrate = -1;
		}
		List<Vector2> lightningPreSetPath = new();
		Vector2 finalPosition = Vector2.Zero;
		public override bool? CanDamage() => null;
		Vector2 initialVelocity = Vector2.Zero;
		public override void OnSpawn(IEntitySource source) {
			initialVelocity = Projectile.velocity;
		}
		public override void AI() {
			if (Projectile.timeLeft > 1) {
				Projectile.timeLeft = 1;
				Vector2 path = Projectile.Center;
				Vector2 toward = initialVelocity.SafeNormalize(Vector2.Zero) * 1500;
				float len = Projectile.ai[0];
				if (len <= 1) {
					return;
				}
				for (int i = 0; i < len; i++) {
					lightningPreSetPath.Add(path);
					if (Main.rand.NextBool(5) && Projectile.ai[1] == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), lightningPreSetPath[i], Projectile.velocity.RotatedByRandom(10), Type, (int)(Projectile.damage * .25f), 0, Projectile.owner, 4, 1);
					}

					path = path.PositionOFFSET(Projectile.velocity, Main.rand.NextFloat(75, 150));
					float length = Vector2.Distance(lightningPreSetPath[i], path);
					for (int l = 0; l < 50; l++) {
						int dust = Dust.NewDust(lightningPreSetPath[i].PositionOFFSET(Projectile.velocity, Main.rand.NextFloat(length)), 0, 0, DustID.Electric);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
						Main.dust[dust].fadeIn = .1f;
						Main.dust[dust].velocity = Main.rand.NextVector2Circular(1, 1);
					}
					if (i == len - 1) {
						finalPosition = path;
					}
					int direction = (i % 2 == 0).ToDirectionInt();
					Vector2 toCursorDirection = Projectile.Center + toward - path;
					float rotation = toCursorDirection.ToRotation();
					float rotationAfter = (Projectile.velocity.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(4, 7) * 10 * direction)).AngleTowards(rotation, MathHelper.PiOver4);
					Projectile.velocity = rotationAfter.ToRotationVector2();
				}
				Projectile.velocity = Vector2.Zero;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (lightningPreSetPath.Count < 1) {
				return false;
			}
			for (int i = 0; i < Projectile.ai[0]; i++) {
				if (i == 0) {
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, lightningPreSetPath[i]))
						return true;
					continue;
				}
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lightningPreSetPath[i - 1], lightningPreSetPath[i]))
					return true;
			}
			return base.Colliding(projHitbox, targetHitbox);
		}
		public override void OnKill(int timeLeft) {
			finalPosition.LookForHostileNPC(out List<NPC> npclist, 200);
			for (int i = 0; i < 150; i++) {
				int dust = Dust.NewDust(finalPosition, 0, 0, DustID.Electric);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = .1f;
				Main.dust[dust].scale = Main.rand.NextFloat(.5f, 1.5f);
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(10, 10);
			}
			foreach (var npc in npclist) {
				Main.player[Projectile.owner].StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage * 2, 0));
			}
		}
	}
}
