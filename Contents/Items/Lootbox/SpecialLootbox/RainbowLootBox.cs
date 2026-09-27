using Roguelike.Common.Systems.SpoilSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Accessories;
using Roguelike.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using Roguelike.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using Roguelike.Contents.Items.Lootbox.BossLootBox;
using Roguelike.Contents.Items.Lootbox.DisableLootbox;
using Roguelike.Contents.Items.Lootbox.Lootpool;
using Roguelike.Contents.Items.NoneSynergy.ParadoxPistol;
using Roguelike.Contents.Transfixion.Perks;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Lootbox.SpecialLootbox {
	internal class RainbowLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {
		}
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Purple;
		}
		public override bool CanActivateSpoil => false;
		public override bool ChestUseOwnLogic => true;
		public override List<int> Set_ItemPool() {
			return new() { ItemPool.GetPoolType<RainbowLootboxPool>() };
		}
		public override int WeaponLevelRangeRandomizer(Player player) {
			return Main.rand.Next(0, 101);
		}
		private void Attempt_ToGiveAdditionalLootbox(IEntitySource entitySource, Player player) {
			int chestRanAmount = Main.rand.Next(0, 9);
			for (int i = 0; i < chestRanAmount; i++) {
				switch (Main.rand.Next(12)) {
					case 0:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<WoodenLootBox>());
						break;
					case 1:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<IronLootBox>());
						break;
					case 2:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverLootBox>());
						break;
					case 3:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<CrimsonLootBox>());
						break;
					case 4:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<CorruptionLootBox>());
						break;
					case 5:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldLootBox>());
						break;
					case 6:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<HoneyLootBox>());
						break;
					case 7:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<ShadowLootBox>());
						break;
					case 8:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<CrystalLootBox>());
						break;
					case 9:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<MechLootBox>());
						break;
					case 10:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<NatureLootBox>());
						break;
					case 11:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<LihzahrdLootBox>());
						break;
				}
				if (Main.rand.NextBool(10)) {
					chestRanAmount++;
				}
			}
		}
		private void Attempt_DroppingWeapons(IEntitySource entitySource, Player player) {
			int randomAmount = Main.rand.Next(0, 25);
			if (randomAmount <= 0) {
				return;
			}
			GetWeapon(entitySource, player, randomAmount);
		}
		private void Attempt_DroppingArmors(Player player) {
			int randomAmount = Main.rand.Next(0, 11);
			if (randomAmount <= 0) {
				return;
			}
			GetArmor(player, randomAmount);
		}
		private void Attempt_DroppingAccessories(Player player) {
			int randomAmount = Main.rand.Next(0, 11);
			if (randomAmount <= 0) {
				return;
			}
			GetAccessories(player, randomAmount);
		}
		private void Attempt_DroppingPotion(Player player) {
			int randomAmount = Main.rand.Next(0, 25);
			if (randomAmount <= 0) {
				return;
			}
			GetPotions(player, randomAmount, 1);
		}
		private void Attempt_AdditionalDrop(IEntitySource entitySource, Player player) {
			int randomRareStuff = Main.rand.Next(1000);
			switch (randomRareStuff) {
				case 0:
					player.QuickSpawnItem(entitySource, ItemID.Zenith);
					break;
				case 1:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<UltimatePistol>());
					break;
				case 2:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<EmblemofProgress>());
					break;
				case 3:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<SynergyEnergy>());
					break;
				case 4:
					player.QuickSpawnItem(entitySource, Main.rand.Next(ModItemLib.SynergyItem));
					break;
				case 5:
					player.QuickSpawnItem(entitySource, ItemID.CoinGun);
					player.QuickSpawnItem(entitySource, ItemID.PlatinumCoin, 9999);
					break;
				case 6:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<EvilEye>());
					break;
				case 7:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<KingSlimeDelight>());
					break;
				case 8:
					Attempt_AdditionalDrop(entitySource, player);
					Attempt_AdditionalDrop(entitySource, player);
					break;
				case 9:
					player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.SpecialPotion));
					break;
				case 10:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<WorldEssence>());
					break;
				case 11:
					player.QuickSpawnItem(entitySource, ModContent.ItemType<GlitchWorldEssence>());
					break;
				case 12:
					ModSpoilSystem.GetSpoils("SSR_LunarGift").OnChoose(player);
					break;
				case 13:
					ModSpoilSystem.GetSpoils("SSR_LunarGift2").OnChoose(player);
					break;
				case 14:
					ModSpoilSystem.GetSpoils("SSR_RelicSpoil").OnChoose(player);
					break;
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);

			Attempt_ToGiveAdditionalLootbox(entitySource, player);

			Attempt_DroppingWeapons(entitySource, player);

			Attempt_DroppingArmors(player);

			Attempt_DroppingAccessories(player);

			Attempt_DroppingPotion(player);

			Attempt_AdditionalDrop(entitySource, player);

			if (Main.rand.NextBool(50)) {
				AbsoluteRightClick(player);
			}
		}
	}
}
