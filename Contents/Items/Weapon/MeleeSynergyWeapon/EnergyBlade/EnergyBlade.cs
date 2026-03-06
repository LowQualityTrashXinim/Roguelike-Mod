using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.EnergyBlade;
internal class EnergyBlade : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
		ItemID.Sets.AnimatesAsSoul[Item.type] = true;
	}
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeCustomProjectile(64, 62, 21, 0, 2, 2, ItemUseStyleID.Shoot, ModContent.ProjectileType<EnergyBladeProjectile>(), true);
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.useTurn = false;
		Item.UseSound = SoundID.Item1;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
		if (CanShootItem) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.EnchantedSword)
			.AddIngredient(ItemID.Terragrim)
			.Register();
	}
}
public class EnergyBladeProjectile : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<EnergyBlade>();
	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = 8;
	}
	public override void SetDefaults() {
		Projectile.width = 64;
		Projectile.height = 62;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 10;
		Projectile.ignoreWater = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float rotation = Projectile.rotation - (Projectile.spriteDirection > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2);
		Vector2 vel = rotation.ToRotationVector2();
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center.IgnoreTilePositionOFFSET(vel, -20), Projectile.Center.IgnoreTilePositionOFFSET(vel, 70));
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.immune[Projectile.owner] = 3;
		Projectile.ai[0]++;
	}
	Player player;
	public override void OnSpawn(IEntitySource source) {
		player = Main.player[Projectile.owner];
	}
	public override void AI() {
		frameCounter();
		EnergySword_Code1AI();
		if (Main.mouseLeft) {
			Projectile.timeLeft = 10;
		}
		if (Projectile.ai[0] >= 5) {
			Projectile.ai[0] = 0;
			float rotation = Projectile.rotation - (Projectile.spriteDirection > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2);
			Vector2 vel = rotation.ToRotationVector2();
			int energycode1 = Projectile.NewProjectile(Projectile.GetSource_FromAI(),
				Projectile.Center.PositionOFFSET(vel, 42),
				vel * 10f,
				ModContent.ProjectileType<EnergyBladeEnergyBallProjectile>(),
				(int)(Projectile.damage * .25f),
				1f,
				Projectile.owner,
				1);
			Main.projectile[energycode1].timeLeft = 120;
		}
	}
	private void EnergySword_Code1AI() {
		Projectile.spriteDirection = player.direction;
		float rotation = (Main.MouseWorld - player.Center).ToRotation();
		Projectile.rotation = rotation;
		Projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
		Projectile.velocity.X = player.direction;
		Projectile.Center = player.Center + Vector2.UnitX.RotatedBy(rotation) * 42;
	}
	public void frameCounter() {
		if (++Projectile.frameCounter >= 3) {
			Projectile.frameCounter = 0;
			if (++Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}
}
class EnergyBladeEnergyBallProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.hide = true;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.wet = false;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = 150;
	}
	public override void AI() {
		if (Projectile.ai[0] == 2) {
			Projectile.velocity -= Projectile.velocity * .1f;
			if (Main.rand.NextBool(4)) {
				int type = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemSapphire, DustID.GemRuby });
				int dust = Dust.NewDust(Projectile.Center, 0, 0, type);
				Main.dust[dust].noGravity = true;
			}
			return;
		}
		for (int i = 0; i < 2; i++) {
			int type = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemSapphire, DustID.GemRuby });
			int dust = Dust.NewDust(Projectile.Center, 0, 0, type);
			Main.dust[dust].noGravity = true;
		}
		if (Projectile.ai[0] == 1) {
			if (Projectile.ai[1] <= 0) {
				Projectile.ai[1] = 5;
				Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(60));
			}
			else {
				Projectile.ai[1]--;
			}
			return;
		}
	}
}
