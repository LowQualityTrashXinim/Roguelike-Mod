using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class MarkOfWizard : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 5;
	}
	readonly int[] spells = [ProjectileID.BallofFire, ProjectileID.SkyFracture, ProjectileID.MagicMissile, ProjectileID.DemonScythe, ProjectileID.Blizzard, ProjectileID.StarCannonStar];
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		if (!player.Center.LookForAnyHostileNPC(1575f) || modplayer.synchronize_Counter % 60 != 0) {
			return;
		}
		int stack = StackAmount(player);
		int damage = 31 + (int)(player.GetWeaponDamage(player.HeldItem) * .42f);
		Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
		for (int i = 0; i < stack; i++) {
			int type = Main.rand.Next(spells);
			switch (type) {
				case ProjectileID.SkyFracture:
					SkyFacture(player, vel, type, damage);
					break;
				case ProjectileID.BallofFire:
					BallofFire(player, type, damage);
					break;
				case ProjectileID.StarCannonStar:
					StarCannonStar(player, vel, type, damage);
					break;
				case ProjectileID.DemonScythe:
					DemonScythe(player, vel, type, damage);
					break;
				case ProjectileID.Blizzard:
					Blizzard(player, vel, type, damage);
					break;
				case ProjectileID.MagicMissile:
					MagicMissile(player, vel, type, damage);
					break;
			}

			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_FromThis(),
				player.Center,
				vel * 12,
				type,
				damage,
				2,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void MagicMissile(Player player, Vector2 vel, int type, int damage) {
		Projectile proj;
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center + vel.Vector2DistributeEvenlyPlus(2, 30, i), vel * 12, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void Blizzard(Player player, Vector2 vel, int type, int damage) {
		Projectile proj;
		for (int i = 0; i < 16; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, vel.Vector2RotateByRandom(60) * Main.rand.NextFloat(10, 16), type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void DemonScythe(Player player, Vector2 vel, int type, int damage) {
		Projectile proj;
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center + vel.RotatedBy(MathHelper.PiOver2) * 20 * (i + 1), vel * 12, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center + vel.RotatedBy(-MathHelper.PiOver2) * 20 * (i + 1), vel * 12, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void SkyFacture(Player player, Vector2 vel, int type, int damage) {
		Projectile proj;
		float rotation = Main.rand.NextFloat(1);
		for (int i = 0; i < 6; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, vel.Vector2DistributeEvenly(6, 360, i).RotatedBy(rotation) * 15, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void StarCannonStar(Player player, Vector2 vel, int type, int damage) {
		Projectile proj;
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, vel.Vector2DistributeEvenlyPlus(3, 30, i).RotatedBy(MathHelper.PiOver4) * 12, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
		for (int i = 0; i < 3; i++) {
			proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, vel.Vector2DistributeEvenlyPlus(3, 30, i).RotatedBy(-MathHelper.PiOver4) * 12, type, damage, 2, player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	private void BallofFire(Player player, int type, int damage) {
		Projectile proj;
		for (int i = 0; i < 5; i++) {
			proj = Projectile.NewProjectileDirect(
		player.GetSource_FromThis(),
		player.Center,
		-Vector2.UnitY.Vector2RotateByRandom(10) * Main.rand.NextFloat(9, 12),
		type,
		damage,
		2,
		player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage -= .1f;
	}
}
