using Terraria.ModLoader;
using Terraria;
using Roguelike.Texture;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.aDebugItem.SkillDebug;
internal class SkillCoolDownRemove : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.Set_DebugItem(true);
	}
}
