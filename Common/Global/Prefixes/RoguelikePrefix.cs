using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Roguelike.Common.Global.Prefixes;
internal class RoguelikePrefix : GlobalItem {
	public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) {
		if (pre == -2 || pre == -1) {
			return false;
		}
		return base.PrefixChance(item, pre, rand);
	}
}
