using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Contents.Projectiles;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_PearlWoodSword : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.PearlwoodSword;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 45;
		entity.scale = 1.5f;
		entity.useTime = entity.useAnimation = 24;
		entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
	}
	int swingCount = 0;
	public override void HoldItem(Item item, Player player) {
		if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			int damage = player.GetWeaponDamage(item);
			if (++swingCount >= 5) {
				swingCount = 0;
				Vector2 distance = Main.MouseWorld - player.Center;
				var vel = distance.SafeNormalize(Vector2.Zero);
				for (int i = 0; i < 6; i++) {
					var pos = player.Center + vel.Vector2DistributeEvenlyPlus(6, 180, i) * 60;
					Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, vel.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<pearlSwordProj>(), damage, 1, player.whoAmI);
				}
			}
			Vector2 toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 100;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = item.type;
			Main.projectile[proj].ai[2] = 120;
		}
	}
}
