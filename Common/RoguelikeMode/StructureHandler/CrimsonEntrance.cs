using Roguelike.Common.Wrapper;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;

public class Structure_CrimsonEntrance : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		var data = ModWrapper.Get_StructureData("Assets/Crimson_Entrance", mod);
		int X = 21 * RogueLikeWorldGen.GridPart_X + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_X - data.width);
		int Y = 12 * RogueLikeWorldGen.GridPart_Y + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_Y - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			Rectangle CrimsonEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(CrimsonEntrance);
			system.Set_MapIgnoredZoneIntoWorldGen(CrimsonEntrance);
			system.SaveStructureLocation("Crimson_Entrance", CrimsonEntrance);
		}
	}
}
