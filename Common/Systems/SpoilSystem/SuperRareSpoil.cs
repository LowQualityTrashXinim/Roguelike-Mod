using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Roguelike.Contents.Perks;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Contents.Items.Chest;
using Roguelike.Contents.Items.aDebugItem.UIdebug;
using Roguelike.Common.Global;

namespace Roguelike.Common.Systems.SpoilSystem;
internal class SuperRareSpoil {
	public class SuppliesPackage : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			return Description.FormatWith(
				Math.Ceiling(chestplayer.weaponAmount * .5f),
				chestplayer.ModifyGetAmount(1)
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
			LootBoxBase.GetAccessories(itemsource, player);
			LootBoxBase.GetArmorPiece(itemsource, player, true);
			LootBoxBase.GetSkillLootbox(itemsource, player);
			LootBoxBase.GetRelic(itemsource, player);
		}
	}
	public class PerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int type = ModContent.ItemType<WorldEssence>();
			if (Main.rand.NextFloat() <= .01f) {
				type = ModContent.ItemType<PerkDebugItem>();
			}
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), type);
		}
	}
	public class SuperRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			Item item = player.QuickSpawnItemDirect(player.GetSource_OpenItem(itemsource), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				relic.AutoAddRelicTemplate(player, 4);
			}
		}
	}
	public class TrinketSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			player.QuickSpawnItem(entitySource, Main.rand.Next(ModItemLib.TrinketAccessories));
		}
	}
	public class DivineWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			player.GetModPlayer<EnchantmentModplayer>().SafeRequest_EnchantItem(1, 3);
			LootBoxBase.GetWeapon(ContentSamples.ItemsByType[itemsource], player, Main.rand.Next(1, 5), 0);
		}
	}
}
