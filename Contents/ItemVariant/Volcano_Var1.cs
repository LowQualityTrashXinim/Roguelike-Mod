using Roguelike.Common.Global;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.ItemVariant;
internal class Volcano_Var1 : ModVariant {
	public override void SetDefault(Item item) {
		item.damage = 40;
		item.useTime = item.useAnimation = 44;
		item.knockBack = 10;
	}
}
