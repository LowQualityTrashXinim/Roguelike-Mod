using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Contents.Items.Lootbox.MiscLootbox;

namespace Roguelike.Contents.Items.Lootbox {
	class WoodenLootBox : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.White;
		}
		public override bool CanActivateSpoil => ModContent.GetInstance<RogueLikeConfig>().BossRushMode;
		public override void LootPoolSetStaticDefaults() {
		}
		public override void AbsoluteRightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			GetWeapon(entitySource, player, 2);
			GetArmorForPlayer(entitySource, player, true);
			GetAccessories(Type, player);
			GetPotion(Type, player);
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SpecialSkillLootBox>());
			int RandomModdedBuff = Main.rand.Next(TerrariaArrayID.SpecialPotion);
			player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
			player.QuickSpawnItem(entitySource, ItemID.GrapplingHook);
			player.QuickSpawnItem(entitySource, ItemID.LesserHealingPotion, 5);
		}
	}
}
