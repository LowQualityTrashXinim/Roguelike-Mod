using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.NoneSynergy.MagicBow;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
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
		Item.BossRushDefaultRange(32, 32, 37, 3f, 12, 12, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicBowProjectile>(),
			5, true);
		Item.mana = 10;
		Item.DamageType = DamageClass.Magic;
		Item.UseSound = SoundID.Item75;
		Item.Set_InfoItem();
	}
	int Counter = 0, GemCounterAttack = 0;
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<MagicBowShootType1>()] < 1
		&& player.ownedProjectileCounts[ModContent.ProjectileType<MagicBowProjectile2>()] < 1;
	}
	public int GetGemDustID() {
		switch (GemCounterAttack + 1) {
			case 1:
				return DustID.GemAmethyst;
			case 2:
				return DustID.GemTopaz;
			case 3:
				return DustID.GemSapphire;
			case 4:
				return DustID.GemEmerald;
			case 5:
				return DustID.GemRuby;
			case 6:
				return DustID.GemDiamond;
			default:
				return 0;
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.VampireKnives)) {
			player.Heal(Main.rand.Next(1, 51));
		}
		if (Counter < 3) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI, ai2: GetGemDustID());
		}
		if (Counter == 3) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MagicBowShootType1>(), damage, knockback, player.whoAmI, 6, ai2: GetGemDustID());
		}
		if (Counter == 4) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 15, i), type, damage, knockback, player.whoAmI, 6, ai2: GetGemDustID());
			}
		}
		if (Counter == 5) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MagicBowProjectile2>(), damage, knockback, player.whoAmI, ai2: GetGemDustID());
			Counter = -1;
			GemCounterAttack = ModUtils.Safe_SwitchValue(GemCounterAttack, 5);
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
public abstract class MagicBowProjectile_Base : ModProjectile {
	public float GemType { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
	public Color GetGem_Color() {
		switch (GemType) {
			case DustID.GemAmethyst:
				return Color.Purple;
			case DustID.GemTopaz:
				return Color.Orange;
			case DustID.GemSapphire:
				return Color.DodgerBlue;
			case DustID.GemEmerald:
				return Color.Green;
			case DustID.GemRuby:
				return Color.Red;
			case DustID.GemDiamond:
				return Color.White;
			default:
				return Color.White;
		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return lightColor.MultiplyRGB(GetGem_Color()) with { A = 0 };
	}
}
public class MagicBowShootType1 : MagicBowProjectile_Base {
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
	float Counter1 = 0;
	public override void OnSpawn(IEntitySource source) {
		Player player = Main.player[Projectile.owner];
		Projectile.timeLeft = player.itemAnimationMax;
		Counter1 = Projectile.timeLeft / Projectile.ai[0];
		Projectile.ai[1] = Projectile.ai[0];
	}
	public override void AI() {
		if (Projectile.timeLeft <= Counter1 * Projectile.ai[0]) {
			Projectile.ai[0]--;
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.Vector2DistributeEvenlyPlus(Projectile.ai[1], 60, Projectile.ai[0]), ModContent.ProjectileType<MagicBowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai2: GemType);
			SoundEngine.PlaySound(SoundID.Item75, Projectile.Center);
		}
		Projectile.Center = Main.player[Projectile.owner].Center;
	}
}
internal class MagicBowProjectile2 : MagicBowProjectile_Base {
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
		if (GemType != 0) {
			int dustnumber = Dust.NewDust(Projectile.Center, 0, 0, (int)GemType,
				Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f),
				Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].fadeIn = 1f;
			if (Projectile.ai[1] >= 200) {
				Main.dust[dustnumber].velocity = Main.rand.NextVector2CircularEdge(5, 5);
				if (Main.rand.NextBool(50)) {
					Main.dust[dustnumber].scale = 4;
					Main.dust[dustnumber].velocity = Main.rand.NextVector2CircularEdge(15, 15) * Main.rand.NextFloat(.75f, 1f);
				}
				if (Projectile.ai[1] % 400 == 0) {
					for (int q = 0; q < 100; q++) {
						Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, (int)GemType,
					Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f),
					Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
						dust.noGravity = true;
						dust.fadeIn = 1f;
						dust.scale += .5f + Main.rand.NextFloat(.25f);
						dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
					}
				}
			}
		}
		if (++Projectile.ai[1] >= 200 || Projectile.Center.IsCloseToPosition(toPlayerMousePos, 175)) {
			Projectile.velocity *= .97f;
			if (++Projectile.ai[0] >= 20) {
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft)) * 3, ModContent.ProjectileType<MagicBowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai2: GemType);
				proj.penetrate = 2;
				proj.maxPenetrate = 2;
				Projectile.ai[0] = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		lightColor = Projectile.GetAlpha(lightColor);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		Projectile.DrawTrail(lightColor, 0.01f);
		return true;
	}
}
internal class MagicBowProjectile : MagicBowProjectile_Base {
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
		if (Projectile.penetrate < 3) {
			return;
		}
		switch (GemType) {
			case DustID.GemAmethyst:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Vector2.UnitY * 10 + Main.rand.NextVector2CircularEdge(3, 3), ModContent.ProjectileType<MagicBow_AmethystGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
			case DustID.GemTopaz:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.9f, 1.1f), ModContent.ProjectileType<MagicBow_TopazGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
			case DustID.GemSapphire:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<MagicBow_SapphireGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
			case DustID.GemEmerald:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<MagicBow_EmeraldGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
			case DustID.GemRuby:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<MagicBow_RubyGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
			case DustID.GemDiamond:
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(9, 9), ModContent.ProjectileType<MagicBow_DiamondGem>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				break;
		}
	}
	public override void AI() {
		if (GemType != 0) {
			int dustnumber = Dust.NewDust(Projectile.position, 0, 0, (int)GemType, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].fadeIn = 1f;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		lightColor = Projectile.GetAlpha(lightColor);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		Projectile.DrawTrail(lightColor, 0.01f);
		return false;
	}
}
internal class MagicBow_AmethystGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Amethyst);
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.width = Projectile.height = 18;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.light = 1f;
	}
	int count = 0;
	public override void AI() {
		if (Main.rand.NextBool(7)) {
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), 0, default, Main.rand.NextFloat(0.75f, 1.25f));
			Main.dust[dustnumber].noGravity = true;
		}
		if (Projectile.velocity != Vector2.Zero && count == 0) {
			Projectile.rotation += MathHelper.ToRadians(10);
			Projectile.velocity *= 0.95f;
		}
		if (!Projectile.velocity.IsLimitReached(1)) {
			Projectile.velocity = Vector2.Zero;
			count++;
		}
		if (count >= 1) {
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 30) {
				Projectile.damage += 2;
				Projectile.netUpdate = true;
				Projectile.tileCollide = true;
				Projectile.penetrate = 1;
				if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 1f;
			}
		}
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 15; i++) {
			var RandomCircular = Main.rand.NextVector2Circular(5.5f, 5.5f);
			var newVelocity = new Vector2(RandomCircular.X, RandomCircular.Y);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, newVelocity.X, newVelocity.Y, 0, default, Main.rand.NextFloat(1.75f, 2.25f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_TopazGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Topaz);
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.width = 18;
		Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.light = 1f;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 500;
	}
	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		return true;
	}
	int bouncecount = 0;
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (bouncecount < 6) {
			Projectile.netUpdate = true;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.6f);
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.6f);
			bouncecount++;
		}
		else {
			if (Projectile.velocity.IsLimitReached(.1f)) {
				Projectile.position += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
		return false;
	}

	public override void AI() {
		if (Projectile.velocity != Vector2.Zero) {
			if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}
		if (Main.rand.NextBool(7)) {
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), 0, default, Main.rand.NextFloat(0.75f, 1.25f));
			Main.dust[dustnumber].noGravity = true;
		}

		if (Projectile.timeLeft % 5 == 0) {
			Projectile.damage += 1;
		}
		Projectile.velocity *= 0.98f;
	}
	public override void OnKill(int timeLeft) {
		Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), (int)(Projectile.damage * 0.85f), 5f, Projectile.owner);
		for (int i = 0; i < 15; i++) {
			var RandomCircular = Main.rand.NextVector2Circular(4f, 4f);
			var newVelocity = new Vector2(RandomCircular.X, RandomCircular.Y);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, newVelocity.X, newVelocity.Y, 0, default, Main.rand.NextFloat(1.75f, 2.25f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_SapphireGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Sapphire);
	public override void SetDefaults() {
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.light = 1f;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.tileCollide = false;
	}
	int count = 0;
	float speedextra = .1f;
	public override bool? CanDamage() {
		return Projectile.ai[2] != 0;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.immune[Projectile.owner] = 3;
	}
	public override void AI() {
		if (Main.rand.NextBool(10)) {
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dustnumber].noGravity = true;
		}
		var player = Main.player[Projectile.owner];
		if (player.dead || !player.active) {
			Projectile.Kill();
		}
		Projectile.Center.LookForHostileNPC(out NPC npc, 2000);
		count++;
		if (count < 30) {
			Projectile.velocity -= Projectile.velocity * 0.06f;
		}
		if (count >= 30) {
			if (npc == null || player.statLife < player.statLifeMax2 * .3f) {
				Projectile.ai[2] = 0;
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speedextra;
				if (speedextra <= 10f) speedextra += .1f;
				Projectile.netUpdate = true;
				if (Projectile.Center.IsCloseToPosition(player.Center, 10)) {
					if (Main.rand.NextBool()) {
						player.Heal(1);
						player.ManaHeal(5);
					}
					Projectile.Kill();
				}

			}
			else {
				Projectile.ai[2] = 1;
				Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 3f;
				Projectile.penetrate = 1;
				if (count % 70 == 0) {
					Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 5f;
				}
			}
		}
		Projectile.velocity = Projectile.velocity.LimitedVelocity(15);
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 25; i++) {
			var RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire, RandomCircular.X, RandomCircular.Y, 0, default, Main.rand.NextFloat(1.5f, 2.25f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_EmeraldGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Emerald);
	public override void SetDefaults() {
		Projectile.width = 14;
		Projectile.height = 18;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 100;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.tileCollide = false;
	}
	public override void AI() {
		int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
		Main.dust[dustnumber].noGravity = true;
		float RotateAccordinglyToVel = Projectile.velocity.SafeNormalize(Vector2.UnitX).ToRotation();
		Projectile.rotation += MathHelper.ToRadians(10 + RotateAccordinglyToVel);
		Projectile.velocity *= .97f;
	}
	public override void OnKill(int timeLeft) {
		float rotation = MathHelper.ToRadians(Main.rand.Next(90));
		for (int l = 0; l < 3; l++) {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Vector2.One.Vector2DistributeEvenlyPlus(3, 360, l).RotatedBy(rotation), ModContent.ProjectileType<MagicBow_SmallEmerald>(), (int)(Projectile.damage * 0.65f), 1f, Projectile.owner);
		}
		for (int i = 0; i < 30; i++) {
			var Ran = Main.rand.NextVector2CircularEdge(5f, 5f);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Ran.X, Ran.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_SmallEmerald : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Emerald);
	public override void SetDefaults() {
		Projectile.width = 14;
		Projectile.height = 18;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.tileCollide = true;
		Projectile.scale = .65f;
		Projectile.timeLeft = 60;
	}
	public override void AI() {
		Projectile.ai[0]++;
		if (Projectile.ai[0] >= 20 && Projectile.velocity.Y <= 20) Projectile.velocity.Y += 0.5f;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 30; i++) {
			var Ran = Main.rand.NextVector2CircularEdge(5f, 5f);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Ran.X, Ran.Y, 0, default, Main.rand.NextFloat(.6f, 1f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_RubyGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Ruby);
	public override void SetDefaults() {
		Projectile.width = 14;
		Projectile.height = 18;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;
	}
	public override bool? CanDamage() {
		return count == 1;
	}
	int count = 0;
	public override void AI() {
		Projectile.ai[0]++;
		if (Projectile.ai[0] >= 20) {
			Projectile.velocity -= Projectile.velocity * 0.1f;
			if (Math.Abs(Projectile.velocity.X) < 1 && Math.Abs(Projectile.velocity.Y) < 1 || count == 1) {
				if (CheckNearByProjectile() && count == 0) {
					Projectile.penetrate = 1;
					Projectile.maxPenetrate = 1;
					count++;
					Projectile.damage *= 2;
					Projectile.timeLeft = 900;
				}
				if (count == 1) {
					var RandomCir = Main.rand.NextVector2Circular(5f, 5f);
					int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
					Main.dust[dustnumber].noGravity = true;
					Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 3f;
					Projectile.rotation = Projectile.velocity.ToRotation();
				}
			}
		}
	}
	public bool CheckNearByProjectile() {
		foreach (Projectile projectile in Main.ActiveProjectiles) {
			if (projectile.type == ModContent.ProjectileType<MagicBowProjectile>()) {
				if (Projectile.Center.IsCloseToPosition(projectile.Center, 30)) {
					return true;
				}
			}
		}
		return false;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 50; i++) {
			var RandomCir = Main.rand.NextVector2Circular(10f, 10f);
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dustnumber].noGravity = true;
		}
	}
}
internal class MagicBow_DiamondGem : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Diamond);
	public override void SetDefaults() {
		Projectile.width = 18;
		Projectile.height = 16;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 200;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;
	}
	public override bool? CanDamage() {
		return false;
	}
	Vector2 oldPos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		for (int i = 0; i < 25; i++) {
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].velocity = Main.rand.NextVector2Circular(4f, 4f);
		}
		oldPos = Projectile.Center;
	}
	public override void AI() {
		Projectile.velocity *= .98f;
		if (Projectile.timeLeft <= 100 && Projectile.timeLeft % 20 == 0) {
			Vector2 vel = (oldPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<MagicBowProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			proj.extraUpdates += 10;
		}
		if (Main.rand.NextBool(5)) {
			int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dustnumber].noGravity = true;
			Main.dust[dustnumber].velocity = Main.rand.NextVector2Circular(4f, 4f);
		}
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 15; i++) {
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(3f, 3f);
		}
	}
}
