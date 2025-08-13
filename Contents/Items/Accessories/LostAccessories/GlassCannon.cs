using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class GlassCannon : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
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
			cooldown = Player.itemAnimationMax * 2;
			Vector2 reVelocity = velocity.SafeNormalize(Vector2.Zero) * 17;
			Projectile.NewProjectile(source, position, reVelocity.Vector2RotateByRandom(5), ModContent.ProjectileType<GlassProjectile>(), (int)(damage * 1.15f), knockback, Player.whoAmI);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
