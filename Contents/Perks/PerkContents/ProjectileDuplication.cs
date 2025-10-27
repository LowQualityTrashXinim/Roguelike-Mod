using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Contents.Items.BuilderItem;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class ProjectileDuplication : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= (StackLimit - StackAmount(player) + 1) * .15f;
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (type != ModContent.ProjectileType<ArenaMakerProj>()
			|| type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| type == ModContent.ProjectileType<TowerDestructionProjectile>()
			|| !ContentSamples.ProjectilesByType[type].minion) {
			player.GetModPlayer<PlayerStatsHandle>().Request_ShootExtra(StackAmount(player), 5 + 5 * StackAmount(player));
		}
	}
}
