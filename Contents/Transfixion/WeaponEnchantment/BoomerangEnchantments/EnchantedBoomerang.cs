using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Roguelike.Contents.Projectiles.Rework;
using Roguelike.Common.Utils;
using Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;

namespace Roguelike.Contents.Transfixion.WeaponEnchantment.BoomerangEnchantments;
public class EnchantedBoomerang : ModEnchantment {
	Item itemstat = new();
	public override void SetDefaults() {
		ItemIDType = ItemID.EnchantedBoomerang;
		itemstat = ContentSamples.ItemsByType[ItemIDType];
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage += .2f;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			if (player.ownedProjectileCounts[ProjectileID.EnchantedBoomerang] < 1) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel * itemstat.shootSpeed, ProjectileID.EnchantedBoomerang, itemstat.damage, itemstat.knockBack, player.whoAmI);
			}
		}
		globalItem.Item_Counter1[index] = ModUtils.CountDown(globalItem.Item_Counter1[index]);
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem.Item_Counter1[index] <= 0) {
			if (proj.type == ProjectileID.EnchantedBoomerang ||
				proj.type == ModContent.ProjectileType<CustomEnchantedBoomerang>() ||
				proj.type == ModContent.ProjectileType<Roguelike_EnchantedBoomerang_ModProjectile>()) {
				for (int i = 0; i < 10; i++) {
					Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, Vector2.One.Vector2DistributeEvenlyPlus(10, 360, i) * 10, ProjectileID.EnchantedBeam, proj.damage, proj.knockBack, player.whoAmI);
				}
			}
		}
	}
}
public class CustomEnchantedBoomerang : Rework_Boomerang {
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.EnchantedBoomerang);
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 900;
	}
	public override bool PreAI() {
		Projectile.ai[0]--;
		return base.PreAI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.ai[0] <= 0) {
			for (int i = 0; i < 10; i++) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(10, 360, i) * 10, ProjectileID.EnchantedBeam, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			Projectile.ai[0] = 60;
		}
	}
}
