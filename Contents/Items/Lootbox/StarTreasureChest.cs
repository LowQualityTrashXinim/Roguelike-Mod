using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Lootbox {
	class StarTreasureChest : ModItem {
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes() {
			var recipe = Recipe.Create(ItemID.FallenStar, 100);
			recipe.AddIngredient(ModContent.ItemType<StarTreasureChest>());
			recipe.Register();
		}
	}
}
