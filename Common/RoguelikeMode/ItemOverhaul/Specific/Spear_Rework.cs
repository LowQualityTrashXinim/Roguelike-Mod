using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class Roguelike_Spear_Item : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Spear;
	}
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<Roguelike_Spear>();
		entity.useTime = entity.useAnimation = 60;
		entity.damage = 80;
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.altFunctionUse == 2) {
			if (player.HasBuff<SpearCoolDown>()) {
				return;
			}
			type = ModContent.ProjectileType<SpearThrownProjectile>();
			player.AddBuff<SpearCoolDown>(180);
			velocity = velocity.SafeNormalize(Vector2.Zero) * 8;
		}
	}
}
public class SpearCoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
