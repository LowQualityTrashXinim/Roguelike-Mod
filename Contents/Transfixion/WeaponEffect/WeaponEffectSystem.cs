using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Roguelike.Contents.Items.Weapon;

namespace Roguelike.Contents.Transfixion.WeaponEffect;
internal class WeaponEffectSystem : ModSystem {
	public static List<WeaponEffect> list_effect { get; private set; } = new();
	public static WeaponEffect GetEffect(int type) => type >= list_effect.Count || type < 0 ? null : list_effect[type];
	public static short Register(WeaponEffect effect) {
		ModTypeLookup<WeaponEffect>.Register(effect);
		effect.SetStaticDefaults();
		list_effect.Add(effect);
		return (short)(list_effect.Count - 1);
	}
}
public abstract class WeaponEffect : ModType {
	public short Type = -1;
	public string Description => ModUtils.LocalizationText("WeaponEffect", $"{Name}");
	public static int GetOutroEffectType<T>() where T : WeaponEffect => ModContent.GetInstance<T>().Type;
	protected sealed override void Register() {
		Type = WeaponEffectSystem.Register(this);
		SetStaticDefaults();
	}
	public virtual void UpdateItem(Player player, Item item, GlobalItemHandle handler) { }
	public virtual void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage) { }
	public virtual void ModifyWeaponCrit(Player player, Item item, ref float crit) { }
}
