using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Roguelike.Common.Utils;

using Roguelike.Common.Global;

namespace Roguelike.Contents.Items.Lootbox {
	internal class IceLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Pink;
		}
		public override void LootPoolSetStaticDefaults() {
		}
		public override List<int> FlagNumAcc() {
			var list = new List<int>() { 0, 1, 2, 3, 4, 5 };
			if (NPC.downedQueenBee) {
				list.Add(6);
			}
			if (NPC.downedBoss3) {
				list.Add(7);
			}
			return list;
		}
		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			if (NPC.downedQueenBee) {
				int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace });
				player.QuickSpawnItem(entitySource, OneRareBeeItem);
			}
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			player.QuickSpawnItem(entitySource, GetAccessory());
			GetWeapon(entitySource, player, 5);
			for (int i = 0; i < 5; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), 3);
			}
		}
	}
}
