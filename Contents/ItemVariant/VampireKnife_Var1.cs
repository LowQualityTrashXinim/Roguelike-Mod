using Roguelike.Common.Global;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.ItemVariant;
internal class VampireKnive_Var1 : ModVariant {
	public override void SetDefault(Item item) {
		item.damage = 9;
		item.useTime = item.useAnimation = 40;
		item.shootSpeed = 6;
	}
}
