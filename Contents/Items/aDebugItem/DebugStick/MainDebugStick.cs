using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.DebugStick;
internal class MainDebugStick : ModItem {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.MulticolorWrench);
	public override void SetDefaults() {
		Item.useTime = Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.width = Item.height = 30;
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateMainDebugUIMenu();
		}
		return base.UseItem(player);
	}
	List<SpriteTracker> tracker = new();
	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		if (tracker == null) {
			tracker = new();
		}
		if (tracker.Count <= 60) {
			SpriteTracker track = new(Main.rand.NextVector2CircularEdge(2, 2) * Main.rand.NextFloat(.5f, 1f), MathHelper.ToRadians(Main.rand.Next(-20, 20)), Main.rand.Next(60, 90));
			track.scale = .75f;
			tracker.Add(track);
		}
		var texture = TextureAssets.Item[Type].Value;
		for (int i = tracker.Count - 1; i >= 0; i--) {
			var tr = tracker[i];
			if (tr.position == Vector2.Zero) {
				tr.position = position;
			}
			tr.position += tr.velocity;
			tr.rotation += tr.rotationSp;
			var baseOnScale = drawColor;
			baseOnScale.A = (byte)(baseOnScale.A * tr.scale * .25f);
			spriteBatch.Draw(texture, tr.position, null, baseOnScale, tr.rotation, origin, tr.scale, SpriteEffects.None, 0);
			tr.scale -= .05f;
			if (--tr.TimeLeft <= 0 || tr.scale <= 0) {
				tracker.RemoveAt(i);
			}
			else {
				tracker[i] = tr;
			}
		}

		return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
