using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Contents.Projectiles;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class LightSpeedRound : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<LightSpeedRound_Player>().LightSpeedRound = true;
		player.ModPlayerStats().Range_CritDamage += .15f;
		player.GetCritChance(DamageClass.Ranged) += 5;
	}
}
public class LightSpeedRound_Player : ModPlayer {
	public bool LightSpeedRound = false;
	public override void ResetEffects() {
		LightSpeedRound = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (LightSpeedRound) {
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<HitScanBullet>();
			}
			velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length();
		}
	}
}
