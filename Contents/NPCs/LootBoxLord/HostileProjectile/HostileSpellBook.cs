using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
public class HostileWaterbolt : BaseHostileProjectile {
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
		if (IsNPCActive(out NPC npc)) {
			Projectile.velocity += (npc.Center + Vector2.One * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			npc.TargetClosest();
			var Player = Main.player[npc.target];
			Vector2 distance = Player.Center - Projectile.Center;
			Projectile.rotation = distance.ToRotation();
			if (++Projectile.ai[1] < 30) {
				return;
			}
			if (++Projectile.ai[2] >= 12) {
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, distance.SafeNormalize(Vector2.Zero) * 6f, ProjectileID.WaterBolt, Projectile.damage, 1 );
				Projectile.ai[2] = 0;
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostileBookOfSkull : BaseHostileProjectile {
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
		if (IsNPCActive(out NPC npc)) {
			Projectile.velocity += (npc.Center - Vector2.UnitY * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			npc.TargetClosest();
			var Player = Main.player[npc.target];
			Vector2 distance = Player.Center - Projectile.Center;
			Projectile.rotation = distance.ToRotation();
			if (++Projectile.ai[1] < 30) {
				return;
			}
			if (++Projectile.ai[2] >= 42) {
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, distance.SafeNormalize(Vector2.Zero) * 2f, ProjectileID.Skull, Projectile.damage, 1 );
				Projectile.ai[2] = 0;
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
public class HostileDemonScythe : BaseHostileProjectile {
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
		if (IsNPCActive(out NPC npc)) {
			Projectile.velocity += (npc.Center - Vector2.One.Add(0,2) * 30 - Projectile.Center).SafeNormalize(Vector2.Zero);
			npc.TargetClosest();
			var Player = Main.player[npc.target];
			Vector2 distance = Player.Center - Projectile.Center;
			Projectile.rotation = distance.ToRotation();
			if (++Projectile.ai[1] < 30) {
				return;
			}
			if (++Projectile.ai[2] >= 12) {
				ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -distance.SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, Projectile.damage, 1 );
				Projectile.ai[2] = 0;
			}
		}
		else {
			Projectile.Kill();
		}
	}
}
