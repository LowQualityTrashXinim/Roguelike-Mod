using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class Roguelike_Trident : GlobalItem{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Trident;
	}
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<Roguelike_Trident_Projectile>();
		entity.useTime = entity.useAnimation = 40;
		entity.damage = 60;
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.altFunctionUse == 2) {
			if (player.HasBuff<TridentCoolDown>()) {
				return;
			}
			type = ModContent.ProjectileType<Roguelike_Trident_ThrownProjectile>();
			player.AddBuff<TridentCoolDown>(180);
			velocity = velocity.SafeNormalize(Vector2.Zero) * 8;
		}
	}
}
public class TridentCoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class Roguelike_Trident_Projectile: SpearReworkProjectile {
	protected override float HoldoutRangeMax => 125;
	protected override float HoldoutRangeMin => -10;
	protected override int SpearType => ProjectileID.Trident;
}
public class Roguelike_Trident_ThrownProjectile : SpearThrownProjectile {
	protected override int SpearType => ProjectileID.Trident;
	public override void PostAI() {
		if (++Projectile.ai[0] >= 10) {
			Projectile.ai[0] = 0;
			Projectile.NewProjectileDirect(
				Projectile.GetSource_FromAI(),
				Projectile.Center + Main.rand.NextVector2Circular(15, 15),
				Vector2.UnitY * Main.rand.NextFloat(),
				ProjectileID.FlaironBubble,
				Projectile.damage / 4,
				1,
				Projectile.owner
				);
		}
	}
}
