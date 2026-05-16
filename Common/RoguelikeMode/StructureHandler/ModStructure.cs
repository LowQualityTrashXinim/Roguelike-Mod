using Roguelike.Common.Systems.Achievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
public abstract class ModStructure {
	public string Name => GetType().Name;
	public virtual void SetDefault() {

	}
	public virtual void CreateStructure(Mod mod, RogueLikeWorldGen system) {

	}
}
public class ModStructure_System : ModSystem {
	public static readonly List<ModStructure> structures = [];
	public static ModStructure SafeGetAchievement(int type) => structures.Count > type && type >= 0 ? structures[type] : null;
	public static ModStructure GetAchievement(string achievementName) => structures.Where(achieve => achieve.Name == achievementName).FirstOrDefault();
}
