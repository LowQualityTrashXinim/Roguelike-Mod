using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Rifle : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override string ModifyTooltip() {
		return string.Format(Tooltip, Duration);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(item.type)) {
			damage += .15f;
		}
	}
}
