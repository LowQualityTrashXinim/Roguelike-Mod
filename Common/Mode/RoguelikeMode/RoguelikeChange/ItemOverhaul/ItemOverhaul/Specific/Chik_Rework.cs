using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_Chik : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Chik;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 42;
	}
}
public class Roguelike_Chik_GlobalProjectile : GlobalProjectile {
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
		return entity.type == ProjectileID.Chik;
	}
	public override void PostAI(Projectile projectile) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.2f, 1f), ProjectileID.CrystalShard, projectile.damage / 3 + 1, 1, projectile.owner);
		}
	}
}
public class Roguelike_CHik_ModPlayer : ModPlayer {
	public int HitCounter = 0;
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.Chik) {
			return;
		}
		if (++HitCounter >= 12) {
			float rotationRand = MathHelper.ToRadians(Main.rand.Next(90));
			for (int i = 0; i < 12; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(proj.GetSource_FromAI(), proj.Center, Vector2.One.RotatedBy(rotationRand).Vector2DistributeEvenlyPlus(12, 360, i) * 6, ProjectileID.CrystalShard, hit.Damage / 3 + 1, 1, proj.owner);
				projectile.penetrate = -1;
				projectile.maxPenetrate = -1;
				projectile.usesIDStaticNPCImmunity = true;
				projectile.idStaticNPCHitCooldown = 20;
			}
			if (HitCounter >= 15) {
				HitCounter = 0;
			}
		}
	}
}
