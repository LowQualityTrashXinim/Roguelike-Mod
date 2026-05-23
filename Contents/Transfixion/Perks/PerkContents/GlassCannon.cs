using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class GlassCannon : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 999;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage *= 1 + StackAmount(player) * .25f;
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().CappedHealthAmount = 50;
		player.GetModPlayer<GlassCannonPlayer>().GlassCannon = true;
	}
}
public class GlassCannonPlayer : ModPlayer {
	public bool GlassCannon = false;
	public int cooldown = 0;
	public override void ResetEffects() {
		GlassCannon = false;
		cooldown = ModUtils.CountDown(cooldown);
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (GlassCannon && cooldown == 0) {
			cooldown = Player.itemAnimationMax * 5;
			var reVelocity = velocity.SafeNormalize(Vector2.Zero) * 17;
			Projectile.NewProjectile(source, position, reVelocity.Vector2RotateByRandom(5), ModContent.ProjectileType<GlassProjectile>(), damage * 5, knockback, Player.whoAmI);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
