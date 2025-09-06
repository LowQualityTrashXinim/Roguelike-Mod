using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Items.Weapon.ItemVariant;

namespace Roguelike.Contents.Transfixion.Artifacts {
	internal class SlimyChaliceArtifact : Artifact {
		public override Color DisplayNameColor => Color.Blue;
		public override IEnumerable<Item> AddStartingItems(Player player) {
			WorldVaultSystem.Set_Variant = ModVariant.GetVariantType<SlimeStaff_Var1>();
			Item item = new Item(ItemID.SlimeStaff);
			yield return item;
		}
	}
	public class SlimyChalicePlayer : ModPlayer {
		public bool Bouncy = false;
		public override void ResetEffects() {
			Bouncy = Player.HasArtifact<SlimyChaliceArtifact>();
		}
		public override void UpdateEquips() {
			if (Bouncy) {
				Player.endurance += .1f;
			}
		}
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
			if (Bouncy) {
				modifiers.Knockback += 1;
				modifiers.KnockbackImmunityEffectiveness *= .5f;
			}
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
			if (Bouncy) {
				modifiers.Knockback += 1;
				modifiers.KnockbackImmunityEffectiveness *= .5f;
			}
		}
	}
	public class BouncyProjectileGlobal : GlobalProjectile {
		public override bool InstancePerEntity => true;
		int counter = 0;
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
			var player = Main.player[projectile.owner];
			if (player.HasArtifact<SlimyChaliceArtifact>() && !projectile.minion && !projectile.hostile) {
				projectile.tileCollide = true;
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
				if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
				if (projectile.timeLeft > 180) projectile.timeLeft = 180;
				if (++counter > 1) return false;
				projectile.damage = (int)(projectile.damage * 1.2f);
				return false;
			}
			return true;
		}
	}
}
