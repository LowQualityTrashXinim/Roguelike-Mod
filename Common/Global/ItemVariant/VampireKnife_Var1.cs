using Terraria;
using Terraria.ID;

namespace Roguelike.Common.Global.ItemVariant;
internal class VampireKnive_Var1 : ModVariant {
	public override void SetDefault(Item item) {
		item.damage = 9;
		item.useTime = item.useAnimation = 40;
		item.shootSpeed = 6;
	}
}
