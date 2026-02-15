using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using System;
using Terraria;
using Terraria.DataStructures;
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
				chestplayer.ModifyGetAmount(1, true)
				);
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			int weaponAmount = (int)Math.Clamp(MathF.Ceiling(chestplayer.weaponAmount * .5f), 1, 999999);
			ModUtils.GetWeaponSpoil(source, weaponAmount);
			ModUtils.GetArmorPiece(source, player, true);
			int amount = chestplayer.ModifyGetAmount(1);
			for (int i = 0; i < amount; i++) {
				ModUtils.GetAccessories(source, player);
				ModUtils.GetSkillLootbox(source, player);
				ModUtils.GetRelic(source, player);
			}
		}
	}
	public class SuperRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			Item item = player.QuickSpawnItemDirect(new EntitySource_Misc("Spoil"), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				relic.AutoAddRelicTemplate(player, 4);
			}
		}
	}
	public class ArmorAccessorySpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.ArmorStatue);
		}
		public override string FinalDescription() {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			return Description.FormatWith(
				chestplayer.ModifyGetAmount(1, true),
				chestplayer.ModifyGetAmount(2, true)
				);
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				ModUtils.GetAccessories(new EntitySource_Misc("Spoil"), player);
			}
			int amount2 = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
			for (int i = 0; i < amount2; i++) {
				ModUtils.GetArmorPiece(new EntitySource_Misc("Spoil"), player, true);
			}
		}
	}
}
