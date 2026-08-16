using Microsoft.Xna.Framework;
using Roguelike.Texture;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles;
internal class StructureBuilderProjectile : ModProjectile{
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.timeLeft = 360;
		Projectile.tileCollide = false;
		Projectile.hide = true;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.velocity = Vector2.Zero;
	}
	public override void AI() {
		Vector2 position = Projectile.Center;

	}
}
