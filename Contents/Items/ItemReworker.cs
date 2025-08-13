using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items;
public class ItemReworker : ModItem {
	

	public virtual int VanillaItemType => ItemID.None;
	public override string Texture => "Terraria/Images/Item_"+VanillaItemType;
	public override void SetDefaults() {


		Item.CloneDefaults(VanillaItemType);
		

	}
}

