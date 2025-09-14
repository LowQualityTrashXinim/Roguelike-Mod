using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

public class Unforgiving : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 24, 10f, 30, 30, ItemUseStyleID.Shoot, 1, 12, true, AmmoID.Arrow);
	}
	public override bool CanShoot(Player player) {
		if (GunMode) {
			Item.useAmmo = AmmoID.Bullet;
		}
		else {
			Item.useAmmo = AmmoID.Arrow;
		}
		Item ammo = player.ChooseAmmo(Item);
		if (ammo == null) {
			GunMode = !GunMode;
			if (GunMode) {
				Item.useAmmo = AmmoID.Bullet;
			}
			else {
				Item.useAmmo = AmmoID.Arrow;
			}
			ammo = player.ChooseAmmo(Item);
			if (ammo == null) {
				return false;
			}
		}
		Pre_ShootType = ammo.shoot;
		return base.CanShoot(player);
	}
	public int ModeCounter = 0;
	public int GlobalCounter = 0;
	int Pre_ShootType = 0;
	/// <summary>
	/// If gun mode is false then currently is bow mode<br/>
	/// Otherwise it is gun mode obviously
	/// </summary>
	public bool GunMode = false;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (GunMode) {
			type = Pre_ShootType;
		}
		else {
			if (Pre_ShootType == ProjectileID.WoodenArrowFriendly) {
				type = ProjectileID.ShadowFlameArrow;
			}
			else {
				type = Pre_ShootType;
			}
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		ModeCounter = ModUtils.Safe_SwitchValue(ModeCounter, 5);
		GlobalCounter = ModUtils.Safe_SwitchValue(ModeCounter, 25);
		Unforgiving_ModPlayer unforgive = player.GetModPlayer<Unforgiving_ModPlayer>();
		int counter = unforgive.Counter;
		unforgive.Counter = -player.itemAnimationMax;
		int CD = unforgive.CD;
		if (CD >= 60) {
			unforgive.CD = 0;
		}
		else {
			return;
		}
		if (GunMode) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
		}
		else {
			Projectile.NewProjectile(source, position, Vector2.Zero, ProjectileID.DesertDjinnCurse, damage, knockback, player.whoAmI);
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.OnyxBlaster)
			.AddIngredient(ItemID.ShadowFlameBow)
			.Register();
	}
}
public class Unforgiving_ModPlayer : ModPlayer {
	public int Counter = 0;
	public int CD = 0;
	public override void ResetEffects() {
		if (++Counter >= 270) {
			Counter = 270;
		}
		if (CD >= 60) {
			CD = 60;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource<Unforgiving>()) {
			if (Main.rand.NextBool(4)) {
				int time = ModUtils.ToSecond(Main.rand.Next(1, 3));
				target.AddBuff(BuffID.ShadowFlame, time);
			}
		}
	}
}
public class Unforgiving_BlastWave : ModProjectile {

}
