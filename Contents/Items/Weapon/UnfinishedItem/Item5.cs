using Terraria.ID;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
internal class Item5 : SynergyModItem {
	//Keybroad Hero's Weapon
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicalHarp)
			.AddIngredient(ItemID.Keybrand)
			.Register();
	}
}
