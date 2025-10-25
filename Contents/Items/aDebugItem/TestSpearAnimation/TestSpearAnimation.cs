using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion;

namespace Roguelike.Contents.Items.aDebugItem.TestSpearAnimation;
public class TestSpearAnimation : ModItem {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<BurningPassion>();
	public override void SetDefaults() {
		Item.BossRushSetDefault(74, 74, 25, 6.7f, 28, 28, ItemUseStyleID.Shoot, true);
		Item.BossRushSetDefaultSpear(ModContent.ProjectileType<TestSpearAnimationP>(), 3.7f);
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(silver: 1000);
		Item.UseSound = SoundID.Item1;
	}
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<TestSpearAnimationP>()] < 1;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		return true;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Spear)
			.AddIngredient(ItemID.WandofSparking)
			.Register();
	}
}
public class TestSpearAnimationP : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<BurningPassionP>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 136;
		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.alpha = 0;

		Projectile.hide = true;
		Projectile.ownerHitCheck = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	protected virtual float HoldoutRangeMin => 50f;
	protected virtual float HoldoutRangeMax => 200f;
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float length = Projectile.Size.Length();
		Vector2 rotationVector = Projectile.rotation.ToRotationVector2();
		if (ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, length / 2f), Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, -length / 2))) {
			return true;
		}
		return false;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;
		player.heldProj = Projectile.whoAmI;
		if (Projectile.timeLeft > duration) {
			Projectile.timeLeft = duration;
			Projectile.direction = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
			Projectile.rotation = -MathHelper.PiOver4 * Projectile.direction;
			Projectile.rotation += Projectile.direction == -1 ? 0 :  MathHelper.PiOver2;
		}
		Projectile.Center = player.MountedCenter;
		Projectile.rotation += MathHelper.ToRadians(180 / (float)duration * Projectile.direction);
		if (Projectile.ai[2] == 1) {
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			float halfDuration = duration * 0.5f;
			float progress;
			if (Projectile.timeLeft < halfDuration) {
				progress = Projectile.timeLeft / halfDuration;
			}
			else {
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.Center += Vector2.SmoothStep(Projectile.velocity * -50, Projectile.velocity * 100, progress);
			Projectile.rotation += Projectile.direction == -1 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.damage = (int)(Projectile.damage * .9f);
		target.AddBuff(BuffID.OnFire, 90);
		target.immune[Projectile.owner] = 5;
	}
}
