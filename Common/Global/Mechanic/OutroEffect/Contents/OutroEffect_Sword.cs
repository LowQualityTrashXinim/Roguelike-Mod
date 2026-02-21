using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Sword : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Sword].Contains(item.type)) {
			damage += .15f;
		}
	}
}
