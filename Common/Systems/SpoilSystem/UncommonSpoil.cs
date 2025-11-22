using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Terraria.DataStructures;
using Roguelike.Contents.Items.RelicItem;

namespace Roguelike.Common.Systems.SpoilSystem;
public class UncommonSpoil {
	public class RareWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			for (int i = 1; i <= 4; i++) {
				ModUtils.GetWeapon(out int returnWeapon, out int amount, i);
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), returnWeapon, amount);
				ModUtils.AmmoForWeapon(itemsource, player, returnWeapon);
			}
		}
	}
	public class WeaponPotionSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.Campfire);
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			return Description.FormatWith(
				Math.Ceiling(chestplayer.weaponAmount * .5f),
				chestplayer.potionTypeAmount
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			ModUtils.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
			ModUtils.GetPotion(itemsource, player);
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			int amount = chestplayer.potionTypeAmount;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.Next(TerrariaArrayID.AllFood), chestplayer.potionNumAmount);
			}
		}
	}
	public class UpgradeAccSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.SpectreBoots);
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2));
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				ModUtils.GetAccessories(itemsource, player);
			}
		}
	}
	public class Tier2RelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1));
		}
		public override void OnChoose(Player player, int itemsource) {
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 2);
				}
			}
		}
	}
}
