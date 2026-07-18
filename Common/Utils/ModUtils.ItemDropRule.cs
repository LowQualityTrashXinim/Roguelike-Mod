using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Roguelike.Common.Utils;
public static partial class ModUtils {
	public static void Disable_BossBagDropRule(this NPCLoot npcLoot, int id) {
		npcLoot.RemoveWhere(rule => rule is DropLocalPerClientAndResetsNPCMoneyTo0 drop && drop.itemId == id);
	}
	public static void Disable_ExpertMasterModeDropRule(this NPCLoot npcLoot) {
		npcLoot.RemoveWhere(rule => rule is DropBasedOnExpertMode drop);
	}
}
