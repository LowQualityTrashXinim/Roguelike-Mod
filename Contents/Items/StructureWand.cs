using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items
{
	class StructureWand : ModItem {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Swordfish);
		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool? UseItem(Player player) {
			if(player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ActivateStructureSaverUI();
			}

			return true;
		}
	}
}
