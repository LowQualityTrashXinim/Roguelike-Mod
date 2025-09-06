using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Roguelike.Contents.Tiles;
public abstract class WGtile : ModTile{
	public override string Texture => ModTexture.MissingTexture_Default;
	public override string HighlightTexture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.tileSolid[Type] = false;
		Main.tileMergeDirt[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileLighted[Type] = true;
		DustType = DustID.Stone;
		AddMapEntry(new Color(220, 21, 200));
	}
	public override bool RightClick(int i, int j) {
		Player player = Main.LocalPlayer;
		WorldGen.KillTile(i, j, noItem: true);
		On_RightClick(player, i, j);
		return base.RightClick(i, j);
	}
	public virtual void On_RightClick(Player player, int i, int j) { }
}
public class Portal_Dungeon : WGtile {

}
public class Portal_JungleTemple : WGtile {

}
