using Microsoft.Xna.Framework;
using Roguelike.Common.Wrapper;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
public class Structure_FleshRealmEntrance : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		var data = ModWrapper.Get_StructureData("Assets/FleshRealm_Entrance", mod);
		int X = 22 * RogueLikeWorldGen.GridPart_X + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_X - data.width);
		int Y = 20 * RogueLikeWorldGen.GridPart_Y + RogueLikeWorldGen.Rand.Next(RogueLikeWorldGen.GridPart_Y - data.height);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			Rectangle FleshRealmEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(FleshRealmEntrance);
			system.Set_MapIgnoredZoneIntoWorldGen(FleshRealmEntrance);
			system.SaveStructureLocation("FleshRealm_Entrance", FleshRealmEntrance);
		}
	}
}
