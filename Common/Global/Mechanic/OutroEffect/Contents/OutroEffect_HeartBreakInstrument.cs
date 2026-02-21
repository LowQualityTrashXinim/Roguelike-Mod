using Microsoft.Xna.Framework;
using Mono.Cecil;
using Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.HeartBreakInstrument;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_HeartBreakInstrument : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = 2;
	}
	public override void Update(Player player) {
		Vector2 position = player.Center;

		int type = ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>();
		for (int i = 0; i < 75; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), position + Main.rand.NextVector2Circular(450, 250), Main.rand.NextVector2CircularEdge(1, 1), type, 35 + (int)(player.GetWeaponDamage(player.HeldItem) * .35f), 0, player.whoAmI, 3, 15, i);
			if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
				proj.ScaleX = 5f;
				proj.ScaleY = .5f;
				projectile.scale = 2;
			}
		}
	}
}
