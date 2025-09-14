using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items;
public abstract class ItemReworker : ModItem {
	public virtual int VanillaItemType => ItemID.None;
	public override string Texture => ModUtils.GetVanillaTexture<Item>(VanillaItemType);
	public override void SetDefaults() {
		Item.CloneDefaults(VanillaItemType);
	}
}
