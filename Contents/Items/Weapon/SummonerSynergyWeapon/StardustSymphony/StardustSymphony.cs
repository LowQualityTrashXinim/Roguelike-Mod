using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.SummonerSynergyWeapon.StardustSymphony;
internal class StardustSymphony : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		DataStorer.AddContext("Synergy_StardustSymphony", new(700, Vector2.Zero, false, Color.Cyan));
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(32, 32, 44, 2f, 22, 22, ItemUseStyleID.Shoot, true);
		Item.DamageType = Roguelike_DamageClass.Summon;
		Item.crit = 4;
		Item.shoot = ModContent.ProjectileType<StardustSymphony_Projectile>();
		Item.shootSpeed = 1f;
		Item.mana = 10;
		Item.noUseGraphic = true;
		Item.noMelee = true;
	}
	public override bool AltFunctionUse(Player player) => true;
	public int Counter = 0;
	public int UseCounter = 0;
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		if (UseCounter == 29) {
			Dust dust;
			for (int i = 0; i < 36; i++) {
				dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
				dust.position += Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * 112.5f;
				dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
				dust.velocity = Vector2.Zero;
				dust.color = Color.Cyan;
				dust.noGravity = true;
				for (int a = 0; a < 20; a++) {
					dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
					dust.position += Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * MathHelper.Lerp(112.5f, 225f, a / 20f);
					dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
					dust.velocity = Vector2.Zero;
					dust.color = Color.White;
					dust.noGravity = true;
				}
				dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
				dust.position += Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * 225;
				dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
				dust.velocity = Vector2.Zero;
				dust.color = Color.Cyan;
				dust.noGravity = true;
			}
			dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_StarDustBig>());
			dust.scale = Main.rand.NextFloat(2.7f, 3.2f);
			dust.color = Color.Cyan;
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
		}
		if (player.altFunctionUse == 2) {
			if (!player.HasBuff<StardustSymphony_Buff>()) {
				if (UseCounter >= 30) {
					for (int i = 0; i < 100; i++) {
						Dust dust = Dust.NewDustDirect(player.Center + Vector2.One.Vector2DistributeEvenlyPlus(100, 360, i) * 750, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
						dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
						dust.velocity = Vector2.Zero;
						dust.color = Color.Cyan;
						dust.noGravity = true;
					}
					player.AddBuff<StardustSymphony_Buff>(ModUtils.ToSecond(40));
					UseCounter = 0;
				}
			}
			if (!player.HasBuff<StardustSymphony_CoolDown>()) {
				player.AddBuff<StardustSymphony_CoolDown>(ModUtils.ToSecond(15));
				player.Heal(100);
				for (int i = 0; i < 10; i++) {
					Vector2 pos = position + Main.rand.NextVector2Circular(50, 50);
					Vector2 vel = (pos - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(.4f, .9f);
					Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), damage, knockback, player.whoAmI);
				}
			}
		}
		else {
			if (player.GetModPlayer<StardustSymphony_ModPlayer>().IsIntheFieldAndActive) {
				Counter++;
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(150, 150), velocity, ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), damage, knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item9 with { PitchRange = (.5f, 1f), Volume = .5f });
			if (Counter >= 15) {
				Counter = 0;
				for (int i = 0; i < 5; i++) {
					Projectile.NewProjectile(source, position + Vector2.One.Vector2DistributeEvenly(5, 360, i) * 900, Vector2.Zero, ModContent.ProjectileType<StardustSymphony_StarProjectile_Special>(), damage, knockback, player.whoAmI);
				}
			}
			if (player.GetModPlayer<StardustSymphony_ModPlayer>().AttackCoolDown <= 0) {
				player.GetModPlayer<StardustSymphony_ModPlayer>().AttackCoolDown = ModUtils.ToSecond(10);
				for (int i = 0; i < 6; i++) {
					Projectile.NewProjectile(source, Main.MouseWorld + Vector2.One.Vector2DistributeEvenlyPlus(6, 360, i) * 300, Vector2.Zero, ModContent.ProjectileType<StardustSymphony_SmallStarProjectile>(), damage, knockback, player.whoAmI, 0, 300, MathHelper.ToRadians(360 * i / 5f));
				}
				Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<StardustSymphony_SmallStarProjectile_Ghost>(), damage / 10, 0, player.whoAmI, 60);
			}
			int countB = Counter % 3;
			for (int i = 0; i < countB; i++) {
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(50, 50), velocity, type, damage, knockback, player.whoAmI);
			}
			Counter++;
			if(!player.GetModPlayer<StardustSymphony_ModPlayer>().BuffActive) {
				UseCounter++;
			}
		}
		for (int i = 0; i < 3; i++) {
			Dust dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
			dust.velocity = velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.9f, 1.5f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.AbigailsFlower)
			.AddIngredient(ItemID.FairyQueenMagicItem)
			.Register();
	}
}
public class StardustSymphony_Dust : ModDust {
	public override string Texture => ModTexture.Glow_Small;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		dust.color.A = 200;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class StardustSymphony_StarDustBig : ModDust {
	public override string Texture => ModTexture.Glow_Big;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .1f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		Vector2 drawpos = dust.position - Main.screenPosition;
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		dust.color.A = 200;
		Main.spriteBatch.Draw(texture, drawpos, null, dust.color, 0, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture, drawpos, null, Color.White, 0, texture.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		Texture2D texture2 = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin2 = texture2.Size() * .5f;
		float scale = dust.scale;
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.Cyan with { A = 200 }, dust.rotation, origin2, new Vector2(scale * 2, scale), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.Cyan with { A = 200 }, dust.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale), SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture2, drawpos, null, Color.White with { A = 200 }, dust.rotation, origin2, new Vector2(scale * 2, scale) * .75f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.White with { A = 200 }, dust.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale) * .75f, SpriteEffects.None, 0);
		return false;
	}
}
public class StardustSymphony_DustBig : ModDust {
	public override string Texture => ModTexture.Glow_Big;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {

		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		dust.color.A = 200;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, Color.White, dust.rotation, texture.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class StardustSymphony_DustStar : ModDust {
	public override string Texture => ModTexture.Glow_Small;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .01f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

			Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
			Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		}

		Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.FOURSTAR).Value;
		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture3.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, Color.White with { A = 0 }, dust.rotation, texture3.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);
		return false;
	}
}
public class StardustSymphony_ModPlayer : ModPlayer {
	public Vector2 StardustSymphonyField_Pos = Vector2.Zero;
	public int HealDelay = 0;
	public int Dust_Counter = 0;
	public bool IsIntheFieldAndActive = false;
	public int FinalEffect_Timer = 0;
	public int FinalEffect_Delay = 0;
	public bool FinalEffect_Finally = false;
	public bool FinalEffect_Exit = true;
	public int AttackCoolDown = 0;
	public bool BuffActive = false;
	public override void ResetEffects() {
		BuffActive = false;
		AttackCoolDown = ModUtils.CountDown(AttackCoolDown);
	}
	public override void UpdateEquips() {
		if (BuffActive) {
			if (StardustSymphonyField_Pos == Vector2.Zero) {
				StardustSymphonyField_Pos = Player.Center;
			}
			DataStorer.ActivateContext(StardustSymphonyField_Pos, "Synergy_StardustSymphony");
			if (Player.Center.IsCloseToPosition(StardustSymphonyField_Pos, 700f)) {
				if (++HealDelay >= 60) {
					HealDelay = 0;
					Player.ManaHeal(45 + (int)(Player.statManaMax2 * .05f));
					Player.Heal(34 + (int)(Player.statLifeMax2 * .05f));
				}
				IsIntheFieldAndActive = true;
			}
			if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
				Dust dust;
				Dust_Counter = ModUtils.Safe_SwitchValue(Dust_Counter, 100);
				dust = Dust.NewDustDirect(StardustSymphonyField_Pos + Vector2.UnitY.Vector2DistributeEvenlyPlus(100, 360, Dust_Counter) * 700, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
				dust.scale = 1;
				dust.velocity = Vector2.Zero;
				dust.color = Color.Cyan;
				dust.noGravity = true;
				dust = Dust.NewDustDirect(StardustSymphonyField_Pos + Vector2.UnitY.Vector2DistributeEvenlyPlus(100, 360, -Dust_Counter) * 700, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
				dust.scale = 1;
				dust.velocity = Vector2.Zero;
				dust.color = Color.Cyan;
				dust.noGravity = true;
			}
		}
		else {
			IsIntheFieldAndActive = false;
			StardustSymphonyField_Pos = Vector2.Zero;
			DataStorer.DeActivateContext("Synergy_StardustSymphony");
		}
		if (Player.HeldItem.type != ModContent.ItemType<StardustSymphony>()) {
			return;
		}
		if (FinalEffect_Exit) {
			return;
		}
		if (--FinalEffect_Delay > 0) {
			return;
		}
		if (--FinalEffect_Timer > 0) {
			if (FinalEffect_Finally) {
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<StardustSymphony_StarProjectile_Visual2>()] <= 0) {
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<StardustSymphony_StarProjectile_Visual2>(), 0, 0, Player.whoAmI);
				}
				FinalEffect_Timer = 0;
				FinalEffect_Exit = true;
				return;
			}
			else {
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(2000, 1000), Vector2.Zero, ModContent.ProjectileType<StardustSymphony_StarProjectile_Visual>(), 0, 0, Player.whoAmI);
			}
		}
		else {
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<StardustSymphony_StarProjectile_Visual>()] > 0) {
				FinalEffect_Finally = false;
			}
			else {
				FinalEffect_Finally = true;
				FinalEffect_Delay = 30;
				FinalEffect_Timer = 10;
			}
		}
	}
	public void FinalEffect() {
		if (Player.HeldItem.type != ModContent.ItemType<StardustSymphony>()) {
			return;
		}
		FinalEffect_Timer = 90;
		FinalEffect_Delay = 10;
		FinalEffect_Finally = false;
		FinalEffect_Exit = false;
	}
}
public class StardustSymphony_Buff : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		var modplayer = player.GetModPlayer<StardustSymphony_ModPlayer>();
		modplayer.BuffActive = true;
		if (player.Center.IsCloseToPosition(modplayer.StardustSymphonyField_Pos, 700f)) {
			player.ModPlayerStats().UpdateCritDamage += .45f;
			player.GetCritChance(DamageClass.Generic) += 15;
		}
		if (player.buffTime[buffIndex] <= 0) {
			modplayer.FinalEffect();
			modplayer.BuffActive = false;
		}
	}
}
public class StardustSymphony_CoolDown : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
//Trying out a possible new optimization method
public class StardustSymphony_ModSystem : ModSystem {
	public static NPC closestNPCinRange = null;
	public override void PreUpdateProjectiles() {
		Vector2 pos = Main.LocalPlayer.Center;
		pos.LookForHostileNPC(out NPC npc, 1500);
		closestNPCinRange = npc;
	}
}
public class StardustSymphony_Projectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(20);
		Projectile.extraUpdates = 20;
	}
	Vector2 mousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		mousePos = Main.MouseWorld;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.velocity = (mousePos - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
		if (Projectile.Center.IsCloseToPosition(mousePos, 10) || !Projectile.Center.IsCloseToPosition(player.Center, 450)) {
			Projectile.Kill();
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(Main.rand.Next(1, 11) + (int)(Projectile.damage * .025f));

		Vector2 pos = player.Center + Main.rand.NextVector2Circular(50, 50);
		Vector2 vel = (pos - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(.4f, .6f);
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), hit.Damage, hit.Knockback, player.whoAmI);
	}
	public override void OnKill(int timeLeft) {
		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override bool PreDraw(ref Color lightColor) {
		return false;
	}
}
public abstract class StardustSymphony_StarProjectile_Base : ModProjectile {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void AI() {
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}

		if (++Projectile.ai[0] <= 600) {
			Projectile.velocity *= .998f;
			return;
		}
		if (StardustSymphony_ModSystem.closestNPCinRange != null) {
			Projectile.timeLeft = 120;
			Vector2 pos = StardustSymphony_ModSystem.closestNPCinRange.Center;
			Projectile.ai[1] *= 1.01f;
			if (Projectile.ai[1] > 2) {
				Projectile.ai[1] = 2;
			}
			Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(Main.rand.Next(5, 26) + (int)(Projectile.damage * .005f));

		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 originOfThisProj = new Vector2(23, 23);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.SMALLWHITEBALL).Value;
		Texture2D textureDot = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 originDot = textureDot.Size() * .5f;
		float len = Projectile.oldPos.Length;
		if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
			for (int k = 0; k < len; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + originOfThisProj;
				Main.EntitySpriteDraw(texture, drawPos, null, lightColor with { A = 0 }, Projectile.rotation, origin, (Projectile.scale * (1 - k / len)) * .75f, SpriteEffects.None, 0);

				Main.EntitySpriteDraw(textureDot, drawPos, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.PiOver2) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.Pi) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.Pi + MathHelper.PiOver2) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.TwoPi) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
			}
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

			Texture2D texture2 = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
			Main.spriteBatch.Draw(texture2, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, 0, texture2.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

			Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.Glow_Medium).Value;
			Main.spriteBatch.Draw(texture3, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan with { A = 150 }, 0, texture3.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		}

		Texture2D textureStar = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale * .5f, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan.ScaleRGB(.3f) with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class StardustSymphony_StarProjectile_Normal : StardustSymphony_StarProjectile_Base {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(30);
		Projectile.extraUpdates = 10;
	}
	public override void AI() {
		if (++Projectile.ai[2] >= 360) {
			Projectile.ai[2] = 0;
			SoundEngine.PlaySound(SoundID.Item8 with { PitchRange = (-1f, -.9f) });
		}
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Projectile.timeLeft <= 1000) {
			Projectile.scale *= .9985f;
		}
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f) * Projectile.scale;
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}
		Projectile.velocity *= .998f;
		if (++Projectile.ai[0] <= 600) {
			return;
		}
		if (StardustSymphony_ModSystem.closestNPCinRange != null) {
			Projectile.timeLeft = 120;
			Vector2 pos = StardustSymphony_ModSystem.closestNPCinRange.Center;
			Projectile.ai[1] *= 1.01f;
			if (Projectile.ai[1] > 2) {
				Projectile.ai[1] = 2;
			}
			Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
		}
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (.5f, 1f) });
		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;

		if (Main.player[Projectile.owner].GetModPlayer<StardustSymphony_ModPlayer>().IsIntheFieldAndActive) {
			Projectile projExplode = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(.6f, 1.1f), ModContent.ProjectileType<StardustSymphony_Projectile_Explosion>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner);
			projExplode.scale = Main.rand.NextFloat(1.4f, 2.8f);
		}
	}
}
public class StardustSymphony_StarProjectile_Visual : StardustSymphony_StarProjectile_Base {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(30);
		Projectile.extraUpdates = 10;
	}
	public override bool? CanDamage() => false;
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Projectile.Center.IsCloseToPosition(player.Center, 200)) {
			Projectile.scale -= .005f;
			if (Projectile.scale <= 0) {
				Projectile.Kill();
			}
		}
		Vector2 dis = player.Center - Projectile.Center;
		Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 64f;
		if (Projectile.velocity.IsLimitReached(2)) {
			Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 originOfThisProj = new Vector2(23, 23);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.SMALLWHITEBALL).Value;
		Vector2 origin = texture.Size() * .5f;
		float len = Projectile.oldPos.Length;
		if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
			for (int k = 0; k < len; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + originOfThisProj;
				Main.EntitySpriteDraw(texture, drawPos, null, lightColor with { A = 0 }, Projectile.rotation, origin, (Projectile.scale * (1 - k / len)) * .75f, SpriteEffects.None, 0);
			}
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

			Texture2D texture2 = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
			Main.spriteBatch.Draw(texture2, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, 0, texture2.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

			Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.Glow_Medium).Value;
			Main.spriteBatch.Draw(texture3, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan with { A = 150 }, 0, texture3.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		}

		Texture2D textureStar = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale * .5f, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan.ScaleRGB(.3f) with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class StardustSymphony_StarProjectile_Special : StardustSymphony_StarProjectile_Base {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(100);
		Projectile.extraUpdates = 10;
		Projectile.scale += .5f;
	}
	Vector2 mouseLastPos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		mouseLastPos = Main.MouseWorld;
	}
	public override void AI() {
		if (++Projectile.ai[2] >= 360) {
			Projectile.ai[2] = 0;
			SoundEngine.PlaySound(SoundID.Item8 with { PitchRange = (-1f, -.9f) });
		}
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}
		Projectile.velocity *= .998f;
		Vector2 dis = mouseLastPos - Projectile.Center;
		Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 128f;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
		if (Projectile.Center.IsCloseToPosition(mouseLastPos, 5)) {
			Projectile.Kill();
		}
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (.5f, 1f) });
		int amount = Main.rand.Next(1, 4);
		for (int i = 0; i < amount; i++) {
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(.9f, 1.3f), ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		Projectile projExplode = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f), ModContent.ProjectileType<StardustSymphony_Projectile_Explosion>(), Projectile.damage * 3, Projectile.knockBack, Projectile.owner);
		projExplode.scale = Main.rand.NextFloat(3.4f, 4.8f);

		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
}
public class StardustSymphony_Projectile_Explosion : ModProjectile {
	public override string Texture => ModTexture.Glow_Big;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}
	public override void AI() {
		Projectile.velocity *= .98f;
		Projectile.scale -= .1f;
		if (Projectile.scale <= 0) {
			Projectile.Kill();
		}
		Projectile.rotation += MathHelper.ToRadians(5) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override bool PreDraw(ref Color lightColor) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		Vector2 drawpos = Projectile.position - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		Main.spriteBatch.Draw(texture, drawpos, null, Color.Cyan with { A = 200 }, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture, drawpos, null, Color.White, Projectile.rotation, origin, Projectile.scale * .75f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		Texture2D texture2 = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin2 = texture2.Size() * .5f;
		float scale = Projectile.scale;
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.Cyan with { A = 200 }, Projectile.rotation, origin2, new Vector2(scale * 2, scale), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.Cyan with { A = 200 }, Projectile.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale), SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture2, drawpos, null, Color.White with { A = 200 }, Projectile.rotation, origin2, new Vector2(scale * 2, scale) * .75f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture2, drawpos, null, Color.White with { A = 200 }, Projectile.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale) * .75f, SpriteEffects.None, 0);

		return false;
	}
}
/// <summary>
/// Ai0 : Delay<br/>
/// Ai1 : Distance ( important to be set )
/// </summary>
public class StardustSymphony_SmallStarProjectile : ModProjectile {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(100);
		Projectile.extraUpdates = 10;
	}
	Vector2 Original_mousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		Original_mousePos = Main.MouseWorld;
		Projectile.knockBack = 0;
	}
	public override void AI() {
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}
		if (--Projectile.ai[0] > 0) {
			return;
		}
		Projectile.rotation += MathHelper.ToRadians(1);
		Projectile.Center = Original_mousePos + Vector2.One.RotatedBy(Projectile.ai[2]) * Projectile.ai[1];
		if (Projectile.ai[0] % 10 == 0) {
			Projectile.ai[1]--;
			Projectile.ai[0] = 0;
		}
		Projectile.ai[2] += .01f;
		if (Projectile.Center.IsCloseToPosition(Original_mousePos, 5)) {
			Projectile.Kill();
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(Main.rand.Next(1, 6) + (int)(Projectile.damage * .001f));

		Dust dust;

		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
		dust.scale = Main.rand.NextFloat(.4f, .8f);
		dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;

		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (.5f, 1f) });

		Projectile projExplode = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f), ModContent.ProjectileType<StardustSymphony_Projectile_Explosion>(), Projectile.damage * 3, Projectile.knockBack, Projectile.owner);
		projExplode.scale = Main.rand.NextFloat(1.4f, 2.8f);

		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(2, 2), ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		projExplode.scale = Main.rand.NextFloat(.5f, .65f);

		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_StarDustBig>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Vector2.Zero;
		dust.rotation = Main.rand.NextFloat(MathHelper.PiOver4);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 originOfThisProj = new Vector2(23, 23);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.SMALLWHITEBALL).Value;
		Vector2 origin = texture.Size() * .5f;
		float len = Projectile.oldPos.Length;
		if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
			for (int k = 0; k < len; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + originOfThisProj;
				float percentage = 1 - k / len;
				Main.EntitySpriteDraw(texture, drawPos, null, Color.Cyan with { A = 0 }, Projectile.rotation, origin, Projectile.scale * percentage * .5f, SpriteEffects.None, 0);
			}
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

			Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.Glow_Medium).Value;
			Main.spriteBatch.Draw(texture3, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan with { A = 150 }, 0, texture3.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		}

		Texture2D textureStar = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.White with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale * .5f, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan.ScaleRGB(.3f) with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class StardustSymphony_SmallStarProjectile_Ghost : ModProjectile {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(5);
		Projectile.hide = true;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.knockBack = 0;
		Projectile.ai[1] = 300;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		Projectile.ai[1]--;
		Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, ModContent.DustType<StardustSymphony_DustStar_AI2>());
		dust.scale = .1f;
		dust.velocity.X = Projectile.ai[1];
		dust.velocity.Y = Main.rand.NextFloat(0, 360);
		dust.color = Color.Cyan;
		dust.noGravity = true;
		dust.customData = this;

		if (--Projectile.ai[0] > 0) {
			return;
		}
		Player player = Main.player[Projectile.owner];
		Projectile.ai[0] = 10;
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, Projectile.ai[1]);
		foreach (var target in npclist) {
			if (target.boss) {
				target.velocity *= .7f;
				continue;
			}
			player.StrikeNPCDirect(target, target.CalculateHitInfo(Projectile.damage, 1));
			target.velocity += (Projectile.Center - target.Center).SafeNormalize(Vector2.Zero) * 5;
		}
	}
}
public class StardustSymphony_StarProjectile_Visual2 : ModProjectile {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(3);
		Projectile.extraUpdates = 5;
		Projectile.scale = 3;
	}
	public override bool? CanDamage() => false;
	public override void AI() {
		Player player = Main.player[Projectile.owner];

		Vector2 directionToMouse = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);

		Projectile.rotation = directionToMouse.ToRotation();

		Dust dust;
		//Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
		//dust.scale = 1.2f;
		//dust.velocity = -directionToMouse * 10;
		//dust.color = Color.White;
		//dust.fadeIn = 1;
		//dust.noGravity = true;

		Projectile.Center = player.Center + directionToMouse * 10;

		dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(10, 90).RotatedBy(Projectile.rotation), 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
		dust.scale = Main.rand.NextFloat(1, 2);
		dust.velocity = directionToMouse * Main.rand.NextFloat(4, 15);
		dust.color = Color.Cyan;
		dust.fadeIn = 1;
		dust.noGravity = true;

		int damage = player.GetWeaponDamage(player.HeldItem) + 60;

		if (--Projectile.ai[1] <= 0) {
			Projectile.ai[1] = 5;
			Vector2 vel = directionToMouse * Main.rand.NextFloat(1, 2);
			int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + Main.rand.NextVector2Circular(10, 90).RotatedBy(Projectile.rotation), vel.Vector2RotateByRandom(60), ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), damage, 0, player.whoAmI);
			Main.projectile[proj].GetGlobalProjectile<RoguelikeGlobalProjectile>().SetCrit++;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Texture2D texture2 = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin2 = texture2.Size() * .5f;
		float scale = Projectile.scale;

		Main.spriteBatch.Draw(texture2, drawpos, null, Color.Cyan with { A = 200 }, Projectile.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale), SpriteEffects.None, 0);


		Main.spriteBatch.Draw(texture2, drawpos, null, Color.White with { A = 200 }, Projectile.rotation + MathHelper.PiOver2, origin2, new Vector2(scale * 2, scale) * .75f, SpriteEffects.None, 0);
		return false;
	}
}
/// <summary>
/// Velocity X : Distance<br/>
/// Velocity Y : Rotation
/// </summary>
public class StardustSymphony_DustStar_AI2 : ModDust {
	public override string Texture => ModTexture.Glow_Small;
	public override bool Update(Dust dust) {
		if (dust.customData is StardustSymphony_SmallStarProjectile_Ghost modproj) {
			dust.position = modproj.Projectile.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(dust.velocity.Y)) * dust.velocity.X;
			dust.velocity.X *= .98f;
			dust.velocity.Y += 5;
			if (dust.position.IsCloseToPosition(modproj.Projectile.Center, 150)) {
				dust.scale -= .005f;
			}
			else {
				if (dust.scale <= .5f) {
					dust.scale += .05f;
				}
			}
		}
		dust.rotation += MathHelper.ToRadians(15);
		if (dust.scale <= .005f) {
			dust.active = false;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		if (!ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

			Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
			Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		}

		Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.FOURSTAR).Value;
		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture3.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, Color.White with { A = 0 }, dust.rotation, texture3.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);
		return false;
	}
}
