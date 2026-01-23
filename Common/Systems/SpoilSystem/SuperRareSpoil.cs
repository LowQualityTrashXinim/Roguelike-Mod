using Humanizer;
using Mono.Cecil;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.aDebugItem.UIdebug;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
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
				chestplayer.ModifyGetAmount(1)
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
			ModUtils.GetAccessories(source, player);
			ModUtils.GetArmorPiece(source, player, true);
			ModUtils.GetSkillLootbox(source, player);
			ModUtils.GetRelic(source, player);
		}
	}
	public class PerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(new EntitySource_Misc("Spoil"), ModContent.ItemType<WorldEssence>());
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
	public class DivineWeaponSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SuperRare;
		}
		public override bool IsSelectable(Player player) {
			return SpoilDropRarity.SuperRareDrop();
		}
		public override void OnChoose(Player player) {
			player.GetModPlayer<EnchantmentModplayer>().SafeRequest_EnchantItem(1, 3);
			PlayerStatsHandle chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			chestplayer.GetAmount();
			ModUtils.GetWeaponSpoil(new EntitySource_Misc("Spoil"), chestplayer.weaponAmount);
		}
	}
}
