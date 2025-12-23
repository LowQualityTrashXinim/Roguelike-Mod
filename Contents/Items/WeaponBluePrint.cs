using Roguelike.Texture;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items
{
	class WeaponBluePrint : ModItem {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = Item.height = 32;
			Item.material = true;
		}
	}
}
