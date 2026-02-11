using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global.Mechanic.OutroEffect;

namespace Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Spear : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Spear].Contains(item.type)) {
			damage += .15f;
		}
	}
}
