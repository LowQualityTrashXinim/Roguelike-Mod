using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.DisableLootbox {
	class IronLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Blue;
		}
		public override void LootPoolSetStaticDefaults() {
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, ItemID.IronAnvil);
		}
	}
}
