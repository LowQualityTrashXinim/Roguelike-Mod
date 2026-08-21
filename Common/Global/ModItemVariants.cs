using Microsoft.Xna.Framework;
using Roguelike.Contents.ItemVariant;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
public class WorldVaultSystem : ModSystem {
	private static List<ModVariant> variantlist = new();
	public static short None = -1;
	public static short Register(ModVariant variant) {
		ModTypeLookup<ModVariant>.Register(variant);
		variantlist.Add(variant);
		if (variant is None_Var) {
			None = (short)(variantlist.Count - 1);
		}
		return (short)(variantlist.Count - 1);
	}
	public static ModVariant GetVariant(int type) => type >= variantlist.Count || type < 0 ? null : variantlist[type];
}
public abstract class ModVariant : ModType {
	public short Variant = -1;
	public static short GetVariantType<T>() where T : ModVariant => ModContent.GetInstance<T>().Variant;
	protected sealed override void Register() {
		SetStaticDefaults();
		Variant = WorldVaultSystem.Register(this);
	}
	public virtual void SetDefault(Item item) { }
	public virtual void Shoot(Item item, Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
	public virtual void UpdateInv(Item item, Player player) { }
}
