using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.DisableLootbox {
	class SilverLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Green;
		}
		public override void LootPoolSetStaticDefaults() {

		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			player.QuickSpawnItem(entitySource, Main.rand.Next(new int[] { ItemID.AmethystHook, ItemID.TopazHook, ItemID.SapphireHook, ItemID.EmeraldHook }));
		}
	}
}
