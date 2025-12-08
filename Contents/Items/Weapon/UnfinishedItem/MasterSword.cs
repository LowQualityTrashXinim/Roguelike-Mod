using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
internal class MasterSword : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.WoodenSword)
			.AddIngredient(ItemID.EnchantedSword)
			.Register();
	}
}
