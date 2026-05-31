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
		SetStaticDefault();
		Type = BossRushModifierLoader.Register(this);
	}
	public bool PositiveModifier = false;
	public static int GetModifierType<T>() where T : BossRushModifier {
		return ModContent.GetInstance<T>().Type;
	}
	public virtual string Description => "Error. Contact xinim";
	public virtual void SetStaticDefault() { }
	public virtual void OnChoose() { }
}
public class BossRushModifierSystem : ModSystem {
	public bool NPC_IncreasesHP = false;
	public bool NPC_IncreasesDMG = false;
	public bool NPC_DealPercentageDMG = false;
	public bool NPC_ChaoticConcoction = false;
	public bool NPC_Ghost = false;
	public bool NPC_HyperRegenerative = false;
	public void ResetModifier() {
		NPC_IncreasesHP = false;
		NPC_IncreasesDMG = false;
		NPC_DealPercentageDMG = false;
		NPC_ChaoticConcoction = false;
		NPC_Ghost = false;
		NPC_HyperRegenerative = false;
	}
}
