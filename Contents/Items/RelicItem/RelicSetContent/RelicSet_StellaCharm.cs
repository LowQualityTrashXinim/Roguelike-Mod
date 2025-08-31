using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class StellaCharm_ModPlayer : ModPlayer {
	class StellaCharm : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 2;
		}
	}
	public int ChanceToActivate = 0;
	public bool StarCharm => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<StellaCharm>());
	public override void UpdateEquips() {
		if (StarCharm)
			Player.ModPlayerStats().AddStatsToPlayer(PlayerStats.SummonDMG, 1.12f);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (StarCharm && (proj.DamageType == DamageClass.Summon || proj.minion)) {
			float Chance = ChanceToActivate * 0.01f;
			if (!Main.dayTime) {
				Chance *= 1.5f;
			}
			if (Main.rand.NextFloat() < Chance) {
				ChanceToActivate = 0;
				Vector2 newPos = target.Center.Add(Main.rand.Next(-100, 100), -1000);
				Vector2 velocityTo = (target.Center - newPos).SafeNormalize(Vector2.UnitX) * 30;
				int damage = (int)(Player.GetDamage(DamageClass.Summon).ApplyTo(40) * (Player.maxMinions + Player.maxTurrets) / 2);
				int projectile = Projectile.NewProjectile(Player.GetSource_OnHit(target), newPos, velocityTo, ProjectileID.Starfury, damage, 1f, Player.whoAmI);
				Main.projectile[projectile].DamageType = DamageClass.Summon;
			}
			else {
				ChanceToActivate++;
			}
		}
	}
}
