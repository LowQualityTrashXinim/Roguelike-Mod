using Terraria;
using Terraria.ID;
using System.Collections.Generic;
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
		public override void AbsoluteRightClick(Player player) {
			
		}
	}
}
