using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.aDebugItem.UIdebug;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			ModUtils.GetWeapon(ContentSamples.ItemsByType[itemsource], player, additiveModify: .5f);
			ModUtils.GetAccessories(itemsource, player);
			ModUtils.GetArmorPiece(itemsource, player, true);
			ModUtils.GetSkillLootbox(itemsource, player);
			ModUtils.GetRelic(itemsource, player);
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
	public class DivineWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			player.GetModPlayer<EnchantmentModplayer>().SafeRequest_EnchantItem(1, 3);
			ModUtils.GetWeapon(ContentSamples.ItemsByType[itemsource], player, Main.rand.Next(1, 5), 0);
		}
	}
}
