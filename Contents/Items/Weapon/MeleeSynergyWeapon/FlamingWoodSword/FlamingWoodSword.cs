using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.FlamingWoodSword;
internal class FlamingWoodSword : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(64, 68, 22, 5f, 30, 30, 1, false);
		Item.DamageType = DamageClass.Melee;

		Item.crit = 5;
		Item.useTurn = false;
		Item.UseSound = SoundID.Item1;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 50);
		Item.shoot = ModContent.ProjectileType<FlamingWoodSwordWave>();
		Item.shootSpeed = 6;

		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.Swipe;
		Item.GetGlobalItem<MeleeWeaponOverhaul>().UseSwipeTwo = true;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, 180);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		for (int i = 0; i < 7; i++) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(150, 150), -Vector2.UnitY, ProjectileID.WandOfSparkingSpark, (int)(damage * 0.45f), knockback, player.whoAmI);
			if (Main.rand.NextBool(5)) {
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(150, 150), -Vector2.UnitY, ModContent.ProjectileType<FlamingFireSpark>(), (int)(damage * .85f), knockback, player.whoAmI);
			}
		}
		Projectile.NewProjectile(source, position, velocity * 2, type, (int)(damage * .1f), 0, player.whoAmI);
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddRecipeGroup("Wood Sword")
			.AddIngredient(ItemID.WandofSparking)
			.Register();
	}
}
public class FlamingFireSpark : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.light = .5f;
		Projectile.timeLeft = 120;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.hide = true;
	}
	public override void AI() {
		for (int i = 0; i < 2; i++) {
			int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.Torch);
			Main.dust[dust].noGravity = true;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 425)) {
			Projectile.timeLeft = 120;
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
		}
		else {
			Projectile.velocity.Y += .05f;
		}

	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire3, ModUtils.ToSecond(Main.rand.Next(7, 11)));
	}
}
public class FlamingWoodSwordWave : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 112;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1250;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 10;
		Projectile.alpha = 255;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.scale = 2;
	}
	public override void AI() {
		if (Projectile.timeLeft <= 75) {
			Projectile.velocity *= .96f;
			Projectile.alpha = Math.Clamp(Projectile.alpha - 3, 0, 255);
		}
		Projectile.rotation = Projectile.velocity.ToRotation();
		float amount = 5 * Projectile.scale;
		for (int i = 0; i < amount; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch, 0, 0, 0, Color.White, Main.rand.NextFloat(0.55f, 1f));
			dust.position += Main.rand.NextVector2Circular(Projectile.width, Projectile.width) * .5f;
			dust.noGravity = true;
			dust.scale *= Projectile.scale;
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire3, ModUtils.ToSecond(15));
		target.immune[Projectile.owner] = 2;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Type].Value;
		float percentageAlpha = Math.Clamp(Projectile.alpha / 255f, 0, 1f);
		var origin = texture.Size() * .5f;
		for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
			var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin * Projectile.scale;
			var color = Color.Lerp(Color.Red, Color.Yellow, k / 100f);
			Main.EntitySpriteDraw(texture, drawPos, null, color.ScaleRGB(percentageAlpha), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}
}
