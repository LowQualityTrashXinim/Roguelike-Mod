using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class ShrineOfOfferring_ModSystem : ModSystem {
	public Rectangle Rect_SoF() => RogueLikeWorldGen.BiomeZone[Bid.ShrineOfOffering][0];
	public bool IsWithinRange = false;
	public bool PlayerGetTheItem = false;
	public override void PostUpdateEverything() {
		if (!ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld) {
			return;
		}
		var player = Main.LocalPlayer;
		var loc = Rect_SoF();
		if (player.Center.IsCloseToPosition(loc.Center().ToWorldCoordinates(), 1500)) {
			if (!IsWithinRange) {
				var worldPos = loc.Center().ToWorldCoordinates();
				ModObject.NewModObject(worldPos, Vector2.Zero, ModObject.GetModObjectType<ModObject_WorldEssence>());
			}
			IsWithinRange = true;
		}
		else {
			IsWithinRange = false;
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["SoF_PlayerGetWorldEssence"] = PlayerGetTheItem;
	}
	public override void LoadWorldData(TagCompound tag) {
		PlayerGetTheItem = tag.Get<bool>("SoF_PlayerGetWorldEssence");
	}
}
public class ModObject_WorldEssence : ItemObject {
	public override int itemToDrop => ModContent.ItemType<WorldEssence>();
	public override void OnDropItem(Item item) {
		ModContent.GetInstance<ShrineOfOfferring_ModSystem>().PlayerGetTheItem = true;
	}
}
public abstract class ItemObject : ModObject {
	public override void SetDefaults() {
		timeLeft = 9999;
	}
	public virtual void OnDropItem(Item item) {

	}
	public virtual int itemToDrop => ItemID.DirtBlock;
	public virtual int Amount => 1;
	public int Counter = 0;
	public int frame = 1;
	public int frameCounter = 0;
	public override void AI() {
		if (Main.LocalPlayer.Center.IsCloseToPosition(Center, 1500)) {
			timeLeft = 9999;
		}
		else {
			Kill();
		}
		if (Main.LocalPlayer.Center.IsCloseToPosition(Center, 50)) {
			Kill();
			var item = Main.LocalPlayer.QuickSpawnItemDirect(new EntitySource_Misc("Object"), itemToDrop, Amount);
			OnDropItem(item);
		}
	}
	public override void Draw(SpriteBatch spritebatch) {
		BasicDrawItem(spritebatch);
		BasicSealAuraEffect(spritebatch, 0, Vector2.Zero, Color.White);
	}
	public void BasicDrawItem(SpriteBatch spritebatch, int framecounter = 1) {
		Main.instance.LoadItem(itemToDrop);
		var texture = TextureAssets.Item[itemToDrop].Value;
		var drawpos = Center - Main.screenPosition;
		var color = Color.White;
		var framing = texture.Frame(1, frame, 0, framecounter);
		spritebatch.Draw(texture, drawpos, null, color, 0, framing.Size() * .5f, 1f, SpriteEffects.None, 1);
	}
	public void BasicSealAuraEffect(SpriteBatch spritebatch, int AuraCounter, Vector2 offset, Color color, float scale = 1f) {
		var glow = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		spritebatch.End();
		spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		float percentalge = Math.Clamp(1 - AuraCounter / 300f, .5f, 1f);
		spritebatch.Draw(glow, position - Main.screenPosition + offset, null, color * percentalge, 0, glow.Size() * .5f, scale + AuraCounter * .01f, SpriteEffects.None, 0);
		spritebatch.End();
		spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
	}
}
