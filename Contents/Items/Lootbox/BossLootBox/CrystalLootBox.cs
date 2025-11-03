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
