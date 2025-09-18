﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Systems;

using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items {
	internal class SynergyEnergy : ModItem {
		public override void SetDefaults() {
			Item.rare = ItemRarityID.Red;
			Item.width = 54;
			Item.height = 20;
			Item.material = true;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<SynergyModPlayer>().acc_SynergyEnergy = true;
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.JumpBoost, Multiplicative: 1.01f);
			handle.Iframe += 1.1f;
			handle.RandomizeChanceEnchantment += .01f;
			handle.BuffTime *= 1.01f;
			handle.DebuffTime *= 1.01f;
			handle.DebuffBuffTime *= .99f;
		}
	}
	public class SynergyModPlayer : ModPlayer {
		public int ItemTypeCurrent = 0;
		public Item itemOld = null;
		public int ItemTypeOld = 0;
		public bool acc_SynergyEnergy = false;
		public bool IsTheItemInQuestionASynergyItem = false;
		public override void ResetEffects() {
			acc_SynergyEnergy = false;
			Item item = Player.HeldItem;
			IsTheItemInQuestionASynergyItem = item.ModItem is SynergyModItem;
			if(item.type == ItemID.None) {
				return;
			}
			if (ItemTypeCurrent != item.type) {
				ItemTypeCurrent = item.type;
				itemOld = item;
			}
			if (Player.itemAnimation == 1) {
				ItemTypeOld = ItemTypeCurrent;
			}
		}
		public override void UpdateEquips() {
			if (Player.itemAnimation == 1) {
				if (IsTheItemInQuestionASynergyItem && ItemTypeCurrent != ItemTypeOld) {
					SynergyModItem moditem = (SynergyModItem)itemOld.ModItem;
					moditem.OutroAttack(Player);
				}
			}
		}
		public bool CompareOldvsNewItemType => ItemTypeCurrent != ItemTypeOld || IsTheItemInQuestionASynergyItem;
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!CompareOldvsNewItemType) {
				if (item.ModItem is SynergyModItem) {
					damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
				}
				return;
			}
			if (acc_SynergyEnergy) {
				damage.Base += 5;
			}
			damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
		}
	}
}
