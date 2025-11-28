using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeBiome.GeneralGenPassess;
/// <summary>
/// Use this pass for setting up where the player gonna spawn, there are 2 configuration for this<br/>
/// You can either set spawn percentage where it goes from 0 to 1 or set the spawn location manually
/// </summary>
public class GeneralGenPass_PlayerSpawnLocaltion : GenPass {
	int locX, locY;
	public GeneralGenPass_PlayerSpawnLocaltion(float spawnX, float spawnY) : base("Setting up player spawn location", 0.0) {
		locX = (int)(spawnX * Main.maxTilesX);
		locY = (int)(spawnY * Main.maxTilesY);
	}
	public GeneralGenPass_PlayerSpawnLocaltion(int spawnX, int spawnY) : base("Setting up player spawn location", 0.0) {
		locX = spawnX; locY = spawnY;
	}
	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
		Main.spawnTileX = locX;
		Main.spawnTileY = locY;
	}
}
