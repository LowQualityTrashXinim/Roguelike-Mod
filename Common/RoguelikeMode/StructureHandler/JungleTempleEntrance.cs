using Microsoft.Xna.Framework;
using Roguelike.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class Structure_JungleTempleEntrance : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		int X = 11 * RogueLikeWorldGen.GridPart_X + RogueLikeWorldGen.Rand.Next(0, RogueLikeWorldGen.GridPart_X / 2);
		int Y = 5 * RogueLikeWorldGen.GridPart_Y + RogueLikeWorldGen.Rand.Next(0, RogueLikeWorldGen.GridPart_Y / 2);
		var data = ModWrapper.Get_StructureData("Assets/Entrance/JungleTemple_Entrance", mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			Rectangle JungleTempleEntrance = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(JungleTempleEntrance);
			system.Set_MapIgnoredZoneIntoWorldGen(JungleTempleEntrance);
			system.SaveStructureLocation("JungleTemple_Entrance", JungleTempleEntrance);
		}
	}
}
