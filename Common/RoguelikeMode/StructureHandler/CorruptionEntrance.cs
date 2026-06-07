using Microsoft.Xna.Framework;
using Roguelike.Common.Wrapper;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class Structure_CorruptionEntrance : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		var data = ModWrapper.Get_StructureData("Assets/Entrance/Corruption_Entrance", mod);
		int X = 6 * RogueLikeWorldGen.GridPart_X + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_X / 2);
		int Y = 19 * RogueLikeWorldGen.GridPart_Y + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_X / 2);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			Rectangle CorruptionEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(CorruptionEntrance);
			system.Set_MapIgnoredZoneIntoWorldGen(CorruptionEntrance);
			system.SaveStructureLocation("Corruption_Entrance", CorruptionEntrance);
		}
	}
}
