using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Contents.Transfixion.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class LifeForceOrb : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<LifeForceOrb>();
		CanBeStack = false;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeForceSpawn(player, target);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		LifeForceSpawn(player, target);
	}
	private static void LifeForceSpawn(Player player, NPC target) {
		if (Main.rand.NextBool(10))
			Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(target.width + 100, target.height + 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
	}
}
