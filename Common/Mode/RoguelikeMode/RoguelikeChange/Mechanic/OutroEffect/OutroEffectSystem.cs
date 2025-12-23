using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.Mechanic.OutroEffect;
internal class OutroEffectSystem : ModSystem {
	public static short OutroEffectID = -1;
	public static List<WeaponEffect> list_effect { get; private set; } = new();
	public static short Register(WeaponEffect effect) {
		ModTypeLookup<WeaponEffect>.Register(effect);
		effect.SetStaticDefaults();
		list_effect.Add(effect);
		return (short)(list_effect.Count - 1);
	}
}
public abstract class WeaponEffect : ModType {
	public short Type = -1;
	protected override void Register() {
		Type = OutroEffectSystem.Register(this);
	}
	public virtual void Update(Player player, Item item) { }
}
public enum WeaponAttribute : byte {
	SoulBound,
	Principle,
	Rule,
}
public enum WeaponTag : byte {
	None,
	//Weapon type
	Sword,
	Shortsword,
	Greatsword,

	Spear,
	Frail,
	Yoyo,
	Boomerang,

	Bow,
	Repeater,
	Pistol,
	Rifle,
	Shotgun,
	Launcher,

	Staff,
	Wand,
	MagicGun,
	Book,

	SummonStaff,
	SummonWand,
	Whip,

	Other,

	Wooden,
	Ore,
	/// <summary>
	/// Not to be confused with actual magic weapon<br/>
	/// This is for weapon that shoot magical projectile like ice blade and Blood Rain Bow
	/// </summary>
	Magical,
	/// <summary>
	/// Not to be confused with <see cref="Magical"/><br/>
	/// This is for weapon that are like Candy Cane Sword and Tentacle Spike 
	/// </summary>
	Fantasy,
	/// <summary>
	/// For weapon that is a actual living creature like toxickarp
	/// </summary>
	Living,
	/// <summary>
	/// For weapon that shoot out elemental like fire, or water like ice bow, waterbolt, etc
	/// </summary>
	Elemental,
	/// <summary>
	/// For weapon that is consider must have like Night Edge, Terra Blade, True Night Edge, Last Prism, etc<br/>
	/// in more simple term, it is for weapon that consider to be lengendary
	/// </summary>
	Mythical,
	/// <summary>
	/// Lunar and moon lord weapon
	/// </summary>
	Celestial,
}
