using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.NoneSynergy.StaffOfLootbox.Projectiles;
public class LBL_Waterbolt : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		CanDealContactDamage = false;
		IDtextureValue = ItemID.WaterBolt;
		Projectile.timeLeft = 240;
	}
	public override void AI() {
		Projectile.velocity *= .96f;
		if (IsNPCActive(out var npc)) {
			Projectile.velocity += (npc.Center + Vector2.One * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.Center.LookForHostileNPC(out NPC Player, 10000)) {
				var distance = Player.Center - Projectile.Center;
				Projectile.rotation = distance.ToRotation();
				if (++Projectile.ai[1] < 30) {
					return;
				}
				if (++Projectile.ai[2] >= 12) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, distance.SafeNormalize(Vector2.Zero) * 6f, ProjectileID.WaterBolt, Projectile.damage, 1, Projectile.owner);
					Projectile.ai[2] = 0;
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class LBL_BookOfSkull : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		CanDealContactDamage = false;
		IDtextureValue = ItemID.BookofSkulls;
		Projectile.timeLeft = 240;
	}
	public override void AI() {
		Projectile.velocity *= .96f;
		if (IsNPCActive(out var npc)) {
			Projectile.velocity += (npc.Center - Vector2.UnitY * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.Center.LookForHostileNPC(out NPC Player, 10000)) {
				var distance = Player.Center - Projectile.Center;
				Projectile.rotation = distance.ToRotation();
				if (++Projectile.ai[1] < 30) {
					return;
				}
				if (++Projectile.ai[2] >= 42) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, distance.SafeNormalize(Vector2.Zero) * 2f, ProjectileID.BookOfSkullsSkull, Projectile.damage, 1, Projectile.owner);
					Projectile.ai[2] = 0;
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class LBL_DemonScythe : BaseProjectile {
	public override void SetHostileDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		CanDealContactDamage = false;
		IDtextureValue = ItemID.DemonScythe;
		Projectile.timeLeft = 240;
	}
	public override void AI() {
		Projectile.velocity *= .96f;
		if (IsNPCActive(out var npc)) {
			Projectile.velocity += (npc.Center - Vector2.One.Add(0, 2) * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.Center.LookForHostileNPC(out NPC Player, 10000)) {
				var distance = Player.Center - Projectile.Center;
				Projectile.rotation = distance.ToRotation();
				if (++Projectile.ai[1] < 30) {
					return;
				}
				if (++Projectile.ai[2] >= 12) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -distance.SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, Projectile.damage, 1, Projectile.owner);
					Projectile.ai[2] = 0;
				}
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
