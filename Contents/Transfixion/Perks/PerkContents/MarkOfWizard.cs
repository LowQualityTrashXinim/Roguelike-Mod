using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
internal class MarkOfWizard : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 5;
	}
	readonly int[] spells = [ProjectileID.BallofFire, ProjectileID.SkyFracture, ProjectileID.MagicMissile, ProjectileID.DemonScythe, ProjectileID.Blizzard, ProjectileID.StarCannonStar];
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		if (!player.Center.LookForAnyHostileNPC(1575f) || modplayer.synchronize_Counter % 60 != 0) {
			return;
		}
		int stack = StackAmount(player);
		int damage = 31 + (int)(player.GetWeaponDamage(player.HeldItem) * .42f);
		Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
		for (int i = 0; i < stack; i++) {
			int type = Main.rand.Next(spells);
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_FromThis(),
				player.Center,
				vel.Vector2RotateByRandom(10) * Main.rand.NextFloat(9, 12),
				type,
				damage,
				2,
				player.whoAmI);
			proj.Set_ProjectileTravelDistance(1575f);
			proj.DamageType = DamageClass.Magic;
			proj.tileCollide = false;
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		modifiers.SourceDamage -= .1f;
	}
}
