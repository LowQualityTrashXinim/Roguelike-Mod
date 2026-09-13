using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Roguelike.Common.Systems;
using Roguelike.Contents.Items;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.General;

namespace Roguelike.Common.RoguelikeMode {
	internal class RoguelikeCommonRecipe : ModSystem {
		public override void AddRecipes() {
			//QoL convert
			var recipe = Recipe.Create(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.ManaCrystal);
			recipe.Register();
		}
		public override void PostAddRecipes() {
			foreach (var recipe in Main.recipe) {
				SynergyRecipe(recipe);
			}
		}
		private void SynergyRecipe(Recipe recipe) {
			if (recipe.createItem.ModItem is SynergyModItem) {
				recipe.AddIngredient(ModContent.ItemType<SynergyEnergy>());
			}
		}
		private void BossRush_Recipe(Recipe recipe) {
			if (recipe.HasResult(ItemID.FlamingArrow) ||
				recipe.HasResult(ItemID.FrostburnArrow) ||
				recipe.HasResult(ItemID.CursedArrow)) {
				recipe.DisableRecipe();
			}
		}
	}
}
