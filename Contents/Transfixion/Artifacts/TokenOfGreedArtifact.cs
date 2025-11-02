using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Transfixion.Artifacts {
	internal class TokenOfGreedArtifact : Artifact {
		public override Color DisplayNameColor => Color.Navy;
	}
	public class GreedPlayer : ModPlayer {
		bool Greed = false;
		protected PlayerStatsHandle chestmodplayer => Player.GetModPlayer<PlayerStatsHandle>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
		}
		public override void UpdateEquips() {
			if (Greed) {
				chestmodplayer.DropModifier += 1;
				chestmodplayer.ChanceLootDrop += .35f;
				chestmodplayer.TransmutationModifier -= .6f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Greed) {
				damage *= .85f;
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			if (Greed) {
				crit *= .5f;
			}
		}
	}
	public class GreedPrivilage : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>();
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.ChanceDropModifier += .35F;
			float stats = 1;
			foreach (var item in player.inventory) {
				if (item.IsAWeapon()) {
					stats += .001f * StackAmount(player);
				}
			}
			handle.AddStatsToPlayer(PlayerStats.PureDamage, Additive: stats);
		}
	}
	public class TheBigMoment : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>();
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			float chance = player.GetModPlayer<PlayerStatsHandle>().DropModifier.ApplyTo(1);
			if (Main.rand.NextFloat() <= chance * .01f * StackAmount(player)) {
				modifiers.SourceDamage *= 2;
			}
			else if (Main.rand.NextFloat() <= chance * .025f * StackAmount(player)) {
				modifiers.SourceDamage += 1;
			}
			else if (Main.rand.NextFloat() <= chance * .05f * StackAmount(player)) {
				player.Heal((int)chance);
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			float chance = player.GetModPlayer<PlayerStatsHandle>().DropModifier.ApplyTo(1);
			if (Main.rand.NextFloat() <= chance * .01f * StackAmount(player)) {
				modifiers.SourceDamage *= 2;
			}
			else if (Main.rand.NextFloat() <= chance * .025f * StackAmount(player)) {
				modifiers.SourceDamage += 1;
			}
			else if (Main.rand.NextFloat() <= chance * .05f * StackAmount(player)) {
				player.Heal((int)chance);
			}
		}
	}
}
