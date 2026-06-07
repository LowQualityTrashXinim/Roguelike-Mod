using Microsoft.Xna.Framework;
using Roguelike.Common.Wrapper;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class EoWStructure : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		var data = ModWrapper.Get_StructureData("Assets/BossChamber/CorruptChamber", mod);
		int X = 3 * RogueLikeWorldGen.GridPart_X + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_X / 2 );
		int Y = 18 * RogueLikeWorldGen.GridPart_Y + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_Y / 2);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);

		if (ModWrapper.IsInBound(data, point)) {
			Rectangle CrimsonEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(CrimsonEntrance);
			system.Set_MapIgnoredZoneIntoWorldGen(CrimsonEntrance);
			system.SaveStructureLocation("EoW_Structure", CrimsonEntrance);
		}
	}
}
