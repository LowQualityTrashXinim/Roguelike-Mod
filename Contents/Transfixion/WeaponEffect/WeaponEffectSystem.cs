using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace Roguelike.Contents.Transfixion.WeaponEffect;
internal class WeaponEffectSystem : ModSystem {
	public static List<WeaponEffect> list_effect { get; private set; } = new();
	public static WeaponEffect GetOutroEffect(int type) => type >= list_effect.Count || type < 0 ? null : list_effect[type];
	public static short Register(WeaponEffect effect) {
		ModTypeLookup<WeaponEffect>.Register(effect);
		effect.SetStaticDefaults();
		list_effect.Add(effect);
		return (short)(list_effect.Count - 1);
	}
}
public abstract class WeaponEffect : ModType {
	public short Type = -1;
	public string Description => ModUtils.LocalizationText("WeaponEffect", $"{Name}.Description");
	public static int GetOutroEffectType<T>() where T : WeaponEffect => ModContent.GetInstance<T>().Type;
	protected sealed override void Register() {
		Type = WeaponEffectSystem.Register(this);
		SetStaticDefaults();
	}
}
