using System.Collections.Generic;
using Terraria.ModLoader;

namespace Roguelike.Common.Systems.BossRushMode;
public static class BossRushModifierLoader {
	private static readonly List<BossRushModifier> modifier = new();
	public static readonly List<BossRushModifier> modifier_good = new();
	public static readonly List<BossRushModifier> modifier_bad = new();
	public static int TotalCount => modifier.Count;
	public static int Register(BossRushModifier perk) {
		ModTypeLookup<BossRushModifier>.Register(perk);
		if (perk.PositiveModifier) {
			modifier_good.Add(perk);
		}
		else {
			modifier_bad.Add(perk);
		}
		modifier.Add(perk);
		return modifier.Count - 1;
	}
	public static BossRushModifier GetModifier(int type) {
		return type >= 0 && type < modifier.Count ? modifier[type] : null;
	}
}
public abstract class BossRushModifier : ModType {
	public int Type { get; private set; }
	protected sealed override void Register() {
		Type = BossRushModifierLoader.Register(this);
		SetStaticDefault();
	}
	public bool PositiveModifier = false;
	public static int GetModifierType<T>() where T : BossRushModifier {
		return ModContent.GetInstance<T>().Type;
	}
	public virtual string Description => "Error. Contact xinim";
	public virtual void SetStaticDefault() { }
	public virtual void OnChoose() { }
}
