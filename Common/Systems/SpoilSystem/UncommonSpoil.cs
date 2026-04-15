using Humanizer;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.SpoilSystem;
public class UncommonSpoil {
	public class RareWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			for (int i = 1; i <= 4; i++) {
				ModUtils.GetWeapon(out int returnWeapon, out int amount, i);
				player.QuickSpawnItem(source, returnWeapon, amount);
				ModUtils.AmmoForWeapon(player, returnWeapon);
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
			chestplayer.GetAmount(false);
			return Description.FormatWith(
				Math.Ceiling(chestplayer.weaponAmount * .5f),
				chestplayer.potionTypeAmount
				);
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");

			var modplayer = player.ModPlayerStats();
			modplayer.GetAmount();
			int weaponAmount = (int)Math.Clamp(MathF.Ceiling(modplayer.weaponAmount * .5f), 1, 999999);
			ModUtils.GetWeaponSpoil(source, weaponAmount);
			ModUtils.GetPotion(source, player);
			int amount = modplayer.potionTypeAmount;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(source, Main.rand.Next(TerrariaArrayID.AllFood), modplayer.potionNumAmount);
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
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2, true));
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int amount = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				ModUtils.GetAccessories(source, player);
			}
		}
	}
	public class Tier2RelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true));
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(source, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 2);
				}
			}
		}
	}

	public class RandomSpoilUncommon : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override void OnChoose(Player player) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (spoil.RareValue != SpoilDropRarity.Uncommon
					&& spoil.RareValue != SpoilDropRarity.Common) {
					SpoilList.Remove(spoil);
				}
			}
			for (int i = 0; i < 2; i++) {
				ModSpoil spoil = Main.rand.Next(SpoilList);
				spoil.OnChoose(player);
				Main.NewText("You have earned : " + spoil.DisplayName, SpoilDropRarity.ColorBaseOnRareValue(spoil.RareValue));
			}
		}
	}
	public class SurprisePackage : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true));
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1);
			if (Main.rand.NextBool(3)) {
				amount += 2;
			}
			ModUtils.GetWeaponSpoil(source, amount);
		}
	}
	public class AdventureBundle : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Uncommon;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.UncommonDrop();
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true));
		}
		public override void OnChoose(Player player) {
			IEntitySource source = new EntitySource_Misc("Spoil");
			int amount = player.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1, true);
			ModUtils.GetWeaponSpoil(source, amount);
			ModUtils.GetAccessories(source, player, amount);
		}
	}
}
