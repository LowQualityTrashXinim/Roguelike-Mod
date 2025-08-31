using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode;
public class RoguelikeBiomeHandle_ModPlayer : ModPlayer {
	HashSet<short> CurrentBiome = new HashSet<short>();
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		CurrentBiome.Clear();
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		Point position = (new Vector2(Player.position.X / RogueLikeWorldGen.GridPart_X, Player.position.Y / RogueLikeWorldGen.GridPart_Y)).ToTileCoordinates();
		int WorldIndex = RogueLikeWorldGen.MapIndex(position.X, position.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		if (zone == null) {
			return;
		}
		for (int i = 0; i < zone.Length; i++) {
			short biomeID = RogueLikeWorldGen.CharToBid(zone, i);
			CurrentBiome.Add(biomeID);
		}
		foreach (var item in CurrentBiome) {
			if (RogueLikeWorldGen.BiomeID.TryGetValue(item, out string value)) {
				Main.NewText("Player are currently in:" + value);
			}
			if (item == Bid.Forest) {
			}
		}
	}
}
internal class RoguelikeBiomeHandle_GlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		base.EditSpawnPool(pool, spawnInfo);
	}
	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
		base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
	}
	public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY) {
		base.EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY);
	}
}
