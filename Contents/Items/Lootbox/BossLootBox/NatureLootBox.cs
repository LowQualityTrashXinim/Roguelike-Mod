using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Lootbox.BossLootBox {
	class NatureLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Cyan;
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int wing = Main.rand.Next(new int[] { ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
			player.QuickSpawnItem(entitySource, wing);
			player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
			player.QuickSpawnItem(entitySource, ItemID.NaughtyPresent);
			player.QuickSpawnItem(entitySource, ItemID.PumpkinMoonMedallion);
			player.QuickSpawnItem(entitySource, ItemID.LihzahrdAltar);

		}
	}
}
