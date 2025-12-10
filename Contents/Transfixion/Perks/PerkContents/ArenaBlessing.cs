using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class ArenaBlessing : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ArenaBlessing>();
		CanBeStack = true;
		StackLimit = 4;
	}
	public override string ModifyToolTip() {
		int stack = StackAmount(Main.LocalPlayer);
		if (stack > 0) {
			return DescriptionIndex(stack);
		}
		return Description;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<AdventureSpirit>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<AdventureSpirit>(), 0, 0, player.whoAmI);
		}
	}
}
