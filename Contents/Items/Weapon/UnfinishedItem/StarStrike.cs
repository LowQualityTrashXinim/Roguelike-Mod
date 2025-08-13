using Roguelike.Contents.Items.Weapon;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
public class StarStrike : SynergyModItem {
	public override string Texture => ModTexture.Get_MissingTexture("Synergy");
	/*
	 * "Genshin ahh weapon"
	 * Spear weapon
	 * Every 3rd swing shoot out star bolts that chase down enemy
	 * Hitting enemy with item trigger "name 1" for 5s ( 12s cool down )
	 * During "name 1" your attack speed is increases by 20%
	 * And each strike toward enemy increases star bolt damage toward that enemy by 5% ( capped at 100% )
	 */
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Starfury)
			.AddIngredient(ItemID.ThunderSpear)
			.Register();
	}
}

