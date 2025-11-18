using System;
using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Contents.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.DrainingVeilBlade;

public class DrainingVeilBlade : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(80, 83, 100, 1, 50, 50, ItemUseStyleID.Swing, ModContent.ProjectileType<DrainingVeilBlade_Wave_Projectile>(), 10, false);
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee)) {
			melee.SwingType = BossRushUseStyle.Swipe;
		}
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 20);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulDrain)
			.AddIngredient(ItemID.ClingerStaff)
			.AddRecipeGroup("Wood Sword")
			.Register();
	}
}
public class DrainingVeilBlade_Hidden_Projectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 70;
		Projectile.hide = true;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		if (Projectile.timeLeft == 70) {
			Projectile.ai[1] = Projectile.velocity.X;
			Projectile.ai[2] = Projectile.velocity.Y;
		}
		if (++Projectile.ai[0] >= 60) {
			Projectile.ai[0] = 0;
			var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
			if (proj.ModProjectile is FlameProjectile flame3) {
				flame3.FlameColor = new Color(100, 255, 100, 0);
				flame3.DebuffType = BuffID.CursedInferno;
			}
		}
		if (Projectile.timeLeft == 1) {
			Projectile.velocity = new(Projectile.ai[1], Projectile.ai[2]);
			var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity.RotatedBy(MathHelper.PiOver2), ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
			if (proj.ModProjectile is FlameProjectile flame2) {
				flame2.FlameColor = new Color(100, 255, 100, 0);
				flame2.DebuffType = BuffID.CursedInferno;
			}
			proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver2), ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
			if (proj.ModProjectile is FlameProjectile flame) {
				flame.FlameColor = new Color(100, 255, 100, 0);
				flame.DebuffType = BuffID.CursedInferno;
			}
		}
		Projectile.velocity = Vector2.Zero;
	}
}
public class DrainingVeilBlade_Wave_Projectile : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = 68;
		Projectile.height = 112;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 180;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 10;
		Projectile.alpha = 255;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
	}
	public override void AI() {
		if (Projectile.timeLeft <= 75) {
			Projectile.velocity *= .96f;
			Projectile.alpha = Math.Clamp(Projectile.alpha - 3, 0, 255);
		}
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (++Projectile.ai[0] >= 5) {
			Projectile.ai[0] = 0;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<DrainingVeilBlade_Hidden_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
		}
	}
	public override void OnKill(int timeLeft) {
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SetMaxDamage(1);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(1);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Type].Value;
		var origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
			var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + new Vector2(Projectile.gfxOffY) + origin;
			var color = new Color(255, 0, 0, 0).ScaleRGB(1 - k * .01f);
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}
}
