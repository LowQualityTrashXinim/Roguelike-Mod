using Roguelike.Contents.Items.Lootbox.Lootpool;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.DisableLootbox {
	internal class BloodLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override List<int> Set_ItemPool() {
			return new List<int> { ItemPool.GetPoolType<BloodPool>() };
		}
		public override void AbsoluteRightClick(Player player) {
			base.AbsoluteRightClick(player);
		}
	}
}
