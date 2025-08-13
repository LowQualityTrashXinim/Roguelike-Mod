using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

class DrainingVeil : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulDrain)
			.AddIngredient(ItemID.ClingerStaff)
			.Register();
	}
}
