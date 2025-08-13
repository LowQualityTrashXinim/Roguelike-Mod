using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.TrialSystem;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.Trial;
internal class SpawnTrial : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			TrialModSystem.SetTrial(0, player.Center);
		}
		return base.UseItem(player);
	}
}
class TrialProtal : ModObject {
	public override void SetDefaults() {
		timeLeft = 9999;
	}
	public override void AI() {
		base.AI();
	}
	public override void Draw(SpriteBatch spritebatch) {
		base.Draw(spritebatch);
	}
}
