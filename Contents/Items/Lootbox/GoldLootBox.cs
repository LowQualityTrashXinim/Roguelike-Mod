using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox {
	class GoldLootBox : LootBoxBase {
		public override void LootPoolSetStaticDefaults() {

		}
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Pink;
		}
		public override List<int> FlagNumAcc() {
			var list = new List<int>() { 0, 1, 2, 3, 4, 5, 7 };
			if (NPC.downedQueenBee) {
				list.Add(6);
			}
			return list;
		}

		public override void OnRightClick(Player player, PlayerStatsHandle modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			GetArmorForPlayer(entitySource, player, Main.rand.NextBool(5));
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			player.QuickSpawnItem(entitySource, GetAccessory());
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ItemID.CalmingPotion, 10);
		}
	}
}
