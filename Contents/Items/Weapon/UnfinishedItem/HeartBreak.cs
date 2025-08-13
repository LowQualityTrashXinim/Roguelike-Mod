using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;

class HeartBreak : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicalHarp)
			.AddIngredient(ItemID.Anchor)
			.Register();
	}
}
