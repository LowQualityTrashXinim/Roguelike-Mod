using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Contents.Transfixion.Arguments;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Transfixion.Artifacts {
	internal class TokenOfPrideArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.PaleGreen;
	}
	public class PridePlayer : ModPlayer {
		bool Pride = false;
		public override void ResetEffects() {
			Pride = Player.HasArtifact<TokenOfPrideArtifact>();
		}
		public override void UpdateEquips() {
			if (Pride) {
				PlayerStatsHandle handle = Player.GetModPlayer<PlayerStatsHandle>();
				float multiplier = 0;
				handle.DropModifier *= multiplier;
			}
		}
		public override void PreUpdate() {
			if (!Pride) {
				return;
			}
			Item item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot == null || globalitem.EnchantmenStlot.Length < 1 && EnchantmentGlobalItem.CanBeEnchanted(item)
					|| globalitem.EnchantmenStlot[3] != ItemID.None || globalitem.EnchantmenStlot[3] == -1) {
					return;
				}
				EnchantmentSystem.EnchantItem(ref item, 3);
			}
		}
	}
	/*
	 TokenOfPride_Upgrade1: {
	DisplayName: Noble Pride [Token Of Pride]
	Description: + Loot drop value are now halve instead of 0x
}

TokenOfPride_Upgrade2: {
	DisplayName: Blacksmith Pride [Token Of Pride]
	Description:
		'''
		+ Increases chance of getting augmentation from 65% to 85%
		+ Weapon have 20% chance to get additional enchanted
		'''
}
*/
	public class BlindPride : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() || AchievementSystem.IsAchieved("TokenOfPride");
		}
		public override void UpdateEquip(Player player) {
			player.AddBuff(BuffID.Blackout, 2);
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .55f + .2f * StackAmount(player);
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 25 + 5 * StackAmount(player);
		}
	}
	public class PridefulPossession : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() || AchievementSystem.IsAchieved("TokenOfPride");
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: player.GetModPlayer<AugmentsPlayer>().valid * StackAmount(player));
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				int power = globalitem.GetValidNumberOfEnchantment();
				damage += power * .1f * StackAmount(player);
			}
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				int power = globalitem.GetValidNumberOfEnchantment();
				crit += power * StackAmount(player);
			}
		}
	}
}
