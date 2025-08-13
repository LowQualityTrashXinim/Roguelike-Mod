using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
