using Roguelike.Common.Utils;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
internal class DamageUp_ModPlayer : ModPlayer {
	public class RelicSet_DamageUp : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 1;
		}
	}
	public class RelicSet_DefensesUp : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 1;
		}
	}
	public bool set1 => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_DamageUp>());
	public bool set2 => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_DefensesUp>());
	public override void UpdateEquips() {
		if (set1) {
			Player.GetDamage(DamageClass.Generic) += .12f;
		}
		if (set2) {
			Player.ModPlayerStats().UpdateDefenseBase.Base += 6;
		}
	}
}
