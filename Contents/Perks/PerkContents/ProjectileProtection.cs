using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Roguelike.Contents.Perks.PerkContents;
public class ProjectileProtection : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ProjectileProtection>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		player.endurance += .05f * StackAmount(player);
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage += -.3f * StackAmount(player);
	}
}
