using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class CrystalLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override void LootPoolSetStaticDefaults() {
		}
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
			player.QuickSpawnItem(entitySource, wing);
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < 2; i++) {
				player.QuickSpawnItem(entitySource, GetAccessory());
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			if (Main.rand.NextBool(5)) {
				player.QuickSpawnItem(entitySource, ItemID.QueenSlimeBossBag);
			}
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
			player.QuickSpawnItem(entitySource, wing);
			if (Main.rand.NextBool(5)) {
				player.QuickSpawnItem(entitySource, ItemID.QueenSlimeBossBag);
			}
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
			}
		}
	}
}
