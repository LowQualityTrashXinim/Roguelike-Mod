using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Contents.Projectiles;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Consumable.Ammo;
internal class LightSpeedRound : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Item_DefaultToAmmo(8, 8, 9, 16, 1.5f, 1, ModContent.ProjectileType<HitScanBullet>(), AmmoID.Bullet);
		Item.DamageType = DamageClass.Ranged;
		Item.rare = ItemRarityID.Green;
		Item.value = 10;
	}
}
