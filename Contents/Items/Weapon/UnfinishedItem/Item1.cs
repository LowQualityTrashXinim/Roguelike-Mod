using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

//Atlantic's Thurst
class Item1 : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Trident)
			.AddIngredient(ItemID.Swordfish)
			.Register();
	}
}
