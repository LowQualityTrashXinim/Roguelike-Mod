using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.RangeSynergyWeapon.Underdog;
class TheUnderdog : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(102, 28, 36, 7f, 24, 24, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
		Item.scale = .7f;
	}
	public override Vector2? HoldoutOffset() {
		return new(-13, 5.5f);
	}
	int counter = 0;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 40);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (player.statLife <= player.statLifeMax2 * .5f) {
			PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
			statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 10);
			statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.25f);
		}
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, Type, ModUtils.ToSecond(9));
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (++counter >= 6) {
			for (int i = 0; i < 70; i++) {
				Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Torch);
				dust.noGravity = true;
				dust.velocity = velocity.Vector2RotateByRandom(4).Vector2RandomSpread(2, Main.rand.NextFloat(.1f, 3.6f));
				dust.scale = Main.rand.NextFloat(.5f, 1.2f);
			}
			SoundEngine.PlaySound(SoundID.Item38 with { Pitch = -1 });
			for (int i = 0; i < 9; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(2).Vector2RandomSpread(2, Main.rand.NextFloat(.5f, 1.1f)), type, damage, knockback, player.whoAmI);
			}
			counter = 0;
		}
		else {
			bool check = OutroEffect_ModPlayer.Check_IntroEffect(player, Type);
			if (check) {
				for (int i = 0; i < 2; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(2).Vector2RandomSpread(2, Main.rand.NextFloat(.5f, 1.1f)), type, damage, knockback, player.whoAmI);
				}
			}
			SoundEngine.PlaySound(SoundID.Item11);
		}
		CanShootItem = true;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Musket)
			.AddIngredient(ItemID.TheUndertaker)
			.Register();
	}
}
public class Underdog_ModPlayer : ModPlayer {
	const int HealPoolCap = 300;
	int HealPool = HealPoolCap, TakenDamage, ConversionRate, ConversionTimer, Timer;
	public override void ResetEffects() {
		if (++Timer >= 120) {
			Timer = 120;
			if (++ConversionTimer >= 60) {
				TakenDamage--;
				ConversionTimer = 0;
				if (++ConversionRate > 200) {
					ConversionRate = 200;
				}
			}
			if (TakenDamage <= 0) {
				ConversionRate = 0;
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Player.IsHeldingModItem<TheUnderdog>()) {
			TakenDamage += hurtInfo.Damage;
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Player.IsHeldingModItem<TheUnderdog>()) {
			TakenDamage += hurtInfo.Damage;
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (item.type == ModContent.ItemType<TheUnderdog>()) {
			damage += ConversionRate;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource<TheUnderdog>()) {
			if (!Player.IsHealthAbovePercentage(.25f) || HealPool < HealPoolCap) {
				if (HealPool >= HealPoolCap) {
					HealPool = 0;
				}
				int heal = Main.rand.Next(1, 6);
				Player.Heal(heal);
				HealPool += heal;
			}
		}
	}
}
