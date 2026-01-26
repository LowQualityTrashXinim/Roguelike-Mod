using Humanizer;
using Mono.Cecil;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.RelicItem;
using System;
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
			chestplayer.GetAmount();
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
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			int amount = chestplayer.potionTypeAmount;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(source, Main.rand.Next(TerrariaArrayID.AllFood), chestplayer.potionNumAmount);
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
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>().ModifyGetAmount(1));
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
}
