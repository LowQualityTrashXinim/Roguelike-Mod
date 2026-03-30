using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Roguelike.Common.Systems;

namespace Roguelike.Common.Global.Prefixes;
internal class RoguelikePrefix : GlobalItem {
	public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) {
		if (pre == -2 || pre == -1) {

		}
		return base.PrefixChance(item, pre, rand);
	}
	public override bool AllowPrefix(Item item, int pre) {
		if (!UniversalSystem.LuckDepartment(UniversalSystem.CHECK_PREFIX)) {
			if (pre == PrefixID.Hard || pre == PrefixID.Guarding || pre == PrefixID.Armored ||
				pre == PrefixID.Precise ||
				pre == PrefixID.Jagged || pre == PrefixID.Spiked || pre == PrefixID.Angry ||
				pre == PrefixID.Hasty2 || pre == PrefixID.Fleeting || pre == PrefixID.Brisk ||
				pre == PrefixID.Wild || pre == PrefixID.Rash || pre == PrefixID.Intrepid) {
				return false;
			}
		}
		return base.AllowPrefix(item, pre);
	}
	public override int ChoosePrefix(Item item, UnifiedRandom rand) {
		return base.ChoosePrefix(item, rand);
	}
}
