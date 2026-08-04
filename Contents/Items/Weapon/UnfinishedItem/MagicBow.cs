using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.NoneSynergy.MagicBow;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
internal class MagicBow : SynergyModItem {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<DiamondBow>();
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.VampireKnives, $"[i:{ItemID.VampireKnives}] Everytime using this weapon heal you for a random amount ranging from 1 to 50");
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.PlatinumBow, $"[i:{ItemID.PlatinumBow}] Bow will shoot out burst of gem staff projectiles deal 45% weapon damage");
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.VampireKnives);
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.PlatinumBow);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 55, 3f, 12, 12, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicBowProjectile>(),
			5, true);
		Item.mana = 10;
		Item.DamageType = DamageClass.Magic;
		Item.UseSound = SoundID.Item75;
	}
	int Counter = 0;
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<MagicBowShootType1>()] < 1
		&& player.ownedProjectileCounts[ModContent.ProjectileType<MagicBowProjectile2>()] < 1;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false; 
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.VampireKnives)) {
			player.Heal(Main.rand.Next(1, 51));
		}
		if (Counter < 3) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI);
		}
		if (Counter == 3) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MagicBowShootType1>(), damage, knockback, player.whoAmI, 6);
		}
		if (Counter == 4) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MagicBowProjectile2>(), damage, knockback, player.whoAmI);
			Counter = -1;
		}
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.PlatinumBow)) {
			int amount = Main.rand.Next(3, 9);
			velocity = velocity.SafeNormalize(Vector2.Zero) * 10;
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40) * Main.rand.NextFloat(.5f, 1.1f), Main.rand.Next(TerrariaArrayID.AllGemStafProjectilePHM), (int)(damage * .45f), knockback, player.whoAmI);
			}
		}
		Counter++;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddRecipeGroup("Ore bow")
			.AddRecipeGroup("Gem staff")
			.Register();
	}
}
public class MagicBowShootType1 : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
		Projectile.light = 1f;
		Projectile.hide = true;
	}
	public override void OnSpawn(IEntitySource source) {
		Player player = Main.player[Projectile.owner];
		Projectile.timeLeft = player.itemAnimationMax;
		Projectile.ai[2] = Projectile.timeLeft / Projectile.ai[0];
		Projectile.ai[1] = Projectile.ai[0];
	}
	public override void AI() {
		if (Projectile.timeLeft <= Projectile.ai[2] * Projectile.ai[0]) {
			Projectile.ai[0]--;
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.Vector2DistributeEvenlyPlus(Projectile.ai[1], 60, Projectile.ai[0]), ModContent.ProjectileType<MagicBowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
		Projectile.Center = Main.player[Projectile.owner].Center;
	}
}
internal class MagicBowProjectile2 : ModProjectile {
	public override string Texture => ModTexture.WHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
	}
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 900;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 10;
		Projectile.light = 1f;
	}
	Vector2 toPlayerMousePos = Vector2.Zero;
	public override bool? CanDamage() => false;
	public override void OnSpawn(IEntitySource source) {
		toPlayerMousePos = Main.MouseWorld;
	}
	public override void AI() {
		int dustnumber = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
		Main.dust[dustnumber].noGravity = true;
		Main.dust[dustnumber].fadeIn = 1f;
		if (++Projectile.ai[1] >= 200 || Projectile.Center.IsCloseToPosition(toPlayerMousePos, 175)) {
			Projectile.ai[1] = 200;
			Projectile.velocity *= .97f;
			if (++Projectile.ai[0] >= 20) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft)) * 3, ModContent.ProjectileType<MagicBowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				Projectile.ai[0] = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(Projectile.GetAlpha(lightColor), 0.01f);
		return true;
	}
}

internal class MagicBowProjectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
	}
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 600;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 10;
		Projectile.light = 1f;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 5;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		var player = Main.player[Projectile.owner];
	}
	public override void AI() {
		int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
		Main.dust[dustnumber].noGravity = true;
		Main.dust[dustnumber].fadeIn = 1f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(Projectile.GetAlpha(lightColor), 0.01f);
		return false;
	}
}
