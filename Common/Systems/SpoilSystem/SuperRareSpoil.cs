using Humanizer;
using Mono.Cecil;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using System;
using System.Collections.Generic;
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
			chestplayer.GetAmount(false);
			return Description.FormatWith(
				chestplayer.weaponAmount,
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
			ModUtils.GetWeaponSpoil(source, chestplayer.weaponAmount);
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
			chestplayer.GetAmount(false);
			return Description.FormatWith(
				chestplayer.ModifyGetAmount(1, true),
				chestplayer.ModifyGetAmount(2, true),
				chestplayer.weaponAmount
				);
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			int amount = chestplayer.ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				ModUtils.GetAccessories(new EntitySource_Misc("Spoil"), player);
			}
			int amount2 = chestplayer.ModifyGetAmount(1);
			for (int i = 0; i < amount2; i++) {
				ModUtils.GetArmorPiece(new EntitySource_Misc("Spoil"), player, true);
			}
			chestplayer.GetAmount();
			ModUtils.GetWeaponSpoil(new EntitySource_Misc("Spoil"), chestplayer.weaponAmount);
		}
	}
	public class PerkSpoil2 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), ModContent.ItemType<LuckEssence>());
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), ModContent.ItemType<WorldEssence>());
		}
	}
	public class RandomSpoilUncommon3 : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (spoil.RareValue == SpoilDropRarity.SSR) {
					SpoilList.Remove(spoil);
				}
			}
			for (int i = 0; i < 3; i++) {
				ModSpoil spoil = Main.rand.Next(SpoilList);
				spoil.OnChoose(Main.LocalPlayer);
				Main.NewText("You have earned : " + spoil.DisplayName, SpoilDropRarity.ColorBaseOnRareValue(spoil.RareValue));
			}
		}
	}
}
