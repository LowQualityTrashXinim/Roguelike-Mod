using Roguelike.Contents.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Accessories.ReworkedAccessories;
public class StingerNecklace : ItemReworker {

	public override int VanillaItemType => ItemID.StingerNecklace;

	public override void UpdateEquip(Player player) {

		player.GetArmorPenetration(DamageClass.Generic) += 10;


	}

}
public class StingerNecklacePlayer : ModPlayer {

	public bool StingerNecklace = false;
	public override void ResetEffects() {
		StingerNecklace = false;
	}

	public override void OnHitAnything(float x, float y, Entity victim) {
		
	}
}


