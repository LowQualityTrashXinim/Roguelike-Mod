using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts {
	internal class TokenOfGreedArtifact : Artifact {
		public override Color DisplayNameColor => Color.Navy;
	}
	public class GreedPlayer : ModPlayer {
		MethodInfo info;
		MethodInfo info2;
		public override void Initialize() {
			info = typeof(Player).GetMethod("ItemCheck_Shoot", BindingFlags.NonPublic | BindingFlags.Instance);
			info2 = typeof(Player).GetMethod("ProcessHitAgainstNPC", BindingFlags.NonPublic | BindingFlags.Instance);
		}
		bool Greed = false;
		public int[] ItemAnimation = new int[10];
		protected PlayerStatsHandle chestmodplayer => Player.GetModPlayer<PlayerStatsHandle>();
		List<int> WeaponCanShoot = new List<int>();
		List<int> WeaponCanMelee = new List<int>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
			if (Greed) {
				WeaponCanShoot.Clear();
				WeaponCanMelee.Clear();
				for (int i = 0; i < 10; i++) {
					ItemAnimation[i] = ModUtils.CountDown(ItemAnimation[i]);
					Item item = Player.inventory[i];
					if (i == Player.selectedItem) {
						continue;
					}
					if (item == null) {
						continue;
					}
					if (item.IsAir) {
						continue;
					}
					if (!item.IsAWeapon()) {
						continue;
					}
					if (item.shoot > 0) {
						WeaponCanShoot.Add(i);
					}
					if (!item.noMelee) {
						WeaponCanMelee.Add(i);
					}
					ItemLoader.HoldItem(item, Player);
				}
			}
		}
		public override void UpdateEquips() {
			if (!Greed) {
				return;
			}
			chestmodplayer.DropModifier += 1;
			chestmodplayer.ChanceLootDrop += .35f;
			chestmodplayer.TransmutationModifier -= .6f;
			if (!Player.ItemAnimationActive) {
				return;
			}
			if (info2 != null) {
				Rectangle itemhitbox = new((int)Player.itemLocation.X, (int)Player.itemLocation.Y, Player.itemWidth, Player.itemHeight);
				for (int i = 0; i < 200; i++) {
					NPC nPC = Main.npc[i];
					if (nPC.active && nPC.immune[Player.whoAmI] == 0) {
						foreach (int a in WeaponCanMelee) {
							Item sItem = Player.inventory[a];
							info2.Invoke(Player, [sItem, itemhitbox, Player.GetWeaponDamage(sItem), Player.GetWeaponKnockback(sItem), i]);
						}
					}
				}
			}
			foreach (int i in WeaponCanShoot) {
				Item item = Player.inventory[i];
				if (ItemAnimation[i] <= 0) {
					if (info != null) {
						info.Invoke(Player, [Player.whoAmI, item, Player.GetWeaponDamage(item)]);
					}
					ItemAnimation[i] = CombinedHooks.TotalUseTime(item.useTime, Player, item);
				}
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Greed) {
				damage *= .1f;
			}
		}
	}
	public class GreedPrivilage : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
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
