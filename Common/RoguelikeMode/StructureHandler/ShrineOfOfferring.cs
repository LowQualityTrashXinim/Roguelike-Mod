using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.Contents;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class ShrineOfOfferring_ModSystem : ModSystem {
	public Rectangle Rect_SoF() => RogueLikeWorldGen.BiomeZone[Bid.ShrineOfOffering][0];
	public bool IsWithinRange = false;
	public bool PlayerGetTheItem = false;
	public override void PostUpdateEverything() {
		if (!ModContent.GetInstance<RoguelikeWorldProperty>().RoguelikeWorld || ModContent.GetInstance<RoguelikeWorldProperty>().TerrariaWorld) {
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
