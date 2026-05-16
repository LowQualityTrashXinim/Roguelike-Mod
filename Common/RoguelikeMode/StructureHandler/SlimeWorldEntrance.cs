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
public class Structure_SlimeWorldEntrance : ModStructure {
	public override void CreateStructure(Mod mod, RogueLikeWorldGen system) {
		int X = 16 * RogueLikeWorldGen.GridPart_X;
		int Y = 11 * RogueLikeWorldGen.GridPart_Y;
		var data = ModWrapper.Get_StructureData("Assets/SlimeWorld_Entrance", mod);
		int Width = data.width / 2;
		int Height = data.height / 2;
		Point16 point = new(X - Width, Y - Height);
		if (ModWrapper.IsInBound(data, point)) {
			Rectangle rect = new(point.X, point.Y, data.width, data.height);
			ModWrapper.GenerateFromData(data, point);
			system.ZoneToBeIgnored.Add(rect);
			system.Set_MapIgnoredZoneIntoWorldGen(rect);
			system.SaveStructureLocation("SlimeWorld_Entrance", rect);
		}
	}
}
