using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon.PureSynergyWeapon.Resolve;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class SpectreArcher_ModPlayer : ModPlayer {
	class RelicSet_SpectreArcher : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 4;
		}
	}
	public bool SpectreQuiver => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_SpectreArcher>());
	public int timer = 0;
	public override void ResetEffects() {
		if (timer <= 60) {
			timer++;
		}
	}
	public override void UpdateEquips() {
		if (SpectreQuiver)
			Player.ModPlayerStats().AddStatsToPlayer(PlayerStats.RangeDMG, Base: 10);
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if ((item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) && SpectreQuiver) {
			velocity = (velocity * 1.1f).LimitedVelocity(20f);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (timer >= 60 && (item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) && SpectreQuiver) {
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(4, 40, i), ModContent.ProjectileType<ResolveGhostArrow>(), (int)(damage * .34f), knockback, Player.whoAmI);
			}
			timer = 0;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
