using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Common.Wrapper;
using StructureHelper.Models;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Roguelike.Common.RoguelikeMode;
public static class GeneralWorldGenTask {
	/// <summary>
	/// This will generate the container which is classified as ??? structure<br/>
	/// The method will auto re-center itself so no need to do so with X,Y axis
	/// </summary>
	/// <param name="X"></param>
	/// <param name="Y"></param>

	public static void Generate_Container(UnifiedRandom Rand, Mod mod, int X, int Y, out Rectangle rec, List<Item> itemLoot = null) {
		var data = ModWrapper.Get_StructureData("Assets/TheContainer", mod);
		var re = RogueLikeWorldGen.Rect_CentralizeRect(X, Y, data.width, data.height);
		ModWrapper.GenerateFromData(data, re.TopLeft().ToPoint16());
		int chest = WorldGen.PlaceChest(re.X + data.width / 2 - 1, re.Y + data.height / 2);
		rec = re;
		if (chest == -1) {
			return;
		}
		RogueLikeWorldGen.AddLoot(Main.chest[chest]);
		Main.chest[chest].AddItemToShop(new Item(Rand.Next(TerrariaArrayID.RandomAssortment)));
		if (itemLoot != null) {
			foreach (var item in itemLoot) {
				Main.chest[chest].AddItemToShop(item);
			}
		}
	}
}
