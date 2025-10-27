using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Perks.PerkContents;
public class ScatterShot : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ScatterShot>();
		CanBeStack = false;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_ScatterShot = true;
	}
}
