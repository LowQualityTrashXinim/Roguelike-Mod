using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.FrostSwordFish;
internal class FrostSwordFish : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(60, 64, 37, 6f, 18, 18, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;

		Item.rare = ItemRarityID.Green;
		Item.crit = 5;
		Item.value = Item.buyPrice(gold: 50);
		Item.useTurn = false;
		Item.scale += 0.25f;
		Item.UseSound = SoundID.Item1;
		Item.shoot = ModContent.ProjectileType<FrostDaggerFishP>();
		Item.shootSpeed = 1;

		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
			meleeItem.SwingType = BossRushUseStyle.Swipe;
			meleeItem.UseSwipeTwo = true;
		}
	}
	public bool SwitchedMode = false;
	public override bool AltFunctionUse(Player player) => true;
	public override bool CanUseItem(Player player) {
		if (player.altFunctionUse == 2) {
			SwitchedMode = !SwitchedMode;
			if (SwitchedMode) {
				ModUtils.CombatTextRevamp(player.Hitbox, Color.Aquamarine, "Projectile mode");
			}
			else {
				ModUtils.CombatTextRevamp(player.Hitbox, Color.IndianRed, "True melee mode");
			}
		}
		return base.CanUseItem(player);
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		base.ModifySynergyToolTips(ref tooltips, modplayer);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		base.HoldSynergyItem(player, modplayer);
		ShootCoolDown = ModUtils.CountDown(ShootCoolDown);
	}
	int count = 3;
	int ShootCoolDown = 0;
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		if (SwitchedMode && ShootCoolDown <= 0) {
			for (int i = 0; i < count; i++) {
				Projectile.NewProjectileDirect(source, position, velocity.Vector2DistributeEvenlyPlus(count, 70, i) * 5, type, (int)(damage * .25f), knockback, player.whoAmI, ai2: 1);
			}
			count = ModUtils.Safe_SwitchValue(count, 6, 3);
			ShootCoolDown = 30 + player.itemAnimationMax;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		if (SwitchedMode) {
			return;
		}
		Vector2 pos = ModUtils.SpawnRanPositionThatIsNotIntoTile(player.Center, 400, 400);
		Projectile.NewProjectile(Item.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<FrostDaggerFishP>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
		target.AddBuff(BuffID.Frostburn, 180);
	}
	public override void MeleeEffects(Player player, Rectangle hitbox) {
		Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);
		Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.IceRod, 0, 0, 0, Color.Aqua, .75f);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.IceBlade)
			.AddIngredient(ItemID.FrostDaggerfish, 100)
			.Register();
	}
}
public class FrostDaggerFishP : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.FrostDaggerfish);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
	}
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Generic;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.timeLeft = 500;
		Projectile.penetrate = -1;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Frostburn, 210);
	}
	int count = 19;
	public override void OnSpawn(IEntitySource source) {
		for (int i = 0; i < 50; i++) {
			Vector2 Circle = Main.rand.NextVector2CircularEdge(7f, 7f);
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.IceRod, Circle.X, Circle.Y, 0, Color.Aqua with { A = 0 }, 1.5f);
			Main.dust[dust].noGravity = true;
		}
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.ai[0] += 1f;
		if (Projectile.ai[0] <= 30) {
			Projectile.rotation += MathHelper.ToRadians(495 / 30);
			return;
		}

		for (int i = 0; i < 2; i++) {
			int dust = Dust.NewDust(Projectile.Center,
				(int)(Projectile.width * 0.5f),
				(int)(Projectile.height * 0.5f),
				DustID.IceTorch,
				Main.rand.Next(-5, 5) + Projectile.velocity.X * -0.25f,
				Main.rand.Next(-5, 5) + Projectile.velocity.Y * -0.25f,
				0, default, Main.rand.NextFloat(0.5f, 1.2f));
			Main.dust[dust].noGravity = true;
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
		if (Projectile.ai[2] == 0) {
			if (!Projectile.Center.LookForHostileNPC(out NPC npc, 900)) {
				Projectile.velocity.Y += 0.3f;
				return;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 7.5f;
		}
		Projectile.penetrate = 1;
		if (count >= 30) {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(),
				Projectile.Center,
				Projectile.velocity * 2.25f,
				ProjectileID.IceBolt,
				(int)(Projectile.damage * 0.75f),
				Projectile.knockBack * 0.65f,
				player.whoAmI);
			count = 0;
		}
		count++;
	}

	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 20; i++) {
			Vector2 Circle = Main.rand.NextVector2CircularEdge(5f, 5f);
			Dust.NewDust(Projectile.Center, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.IceRod, Circle.X, Circle.Y, 0, Color.Aqua, Main.rand.NextFloat(1, 1.25f));
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(ItemID.FrostDaggerfish);
		Texture2D texture = TextureAssets.Item[ItemID.FrostDaggerfish].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
