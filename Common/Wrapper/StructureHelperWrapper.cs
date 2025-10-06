using Microsoft.Xna.Framework;
using StructureHelper.API;
using StructureHelper.Models;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.Wrapper
{
    public static partial class ModWrapper
    {
        public static StructureData Get_StructureData(string path, Mod mod)
        {
            return Generator.GetStructureData(path, mod);
        }
        public static void GenerateFromData(StructureData data, Point16 pos)
        {
            Generator.GenerateFromData(data, pos);
        }
        public static bool IsInBound(StructureData data, Point16 pos)
        {
            return Generator.IsInBounds(data, pos);
        }
    }
}
