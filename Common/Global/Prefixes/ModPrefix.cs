using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace Roguelike.Common.Global.Prefixes;

public abstract class BaseAccPrefix : ModPrefix {
	public override PrefixCategory Category => PrefixCategory.Accessory;
	public sealed override bool CanRoll(Item item) {
		return item.accessory;
	}
	public sealed override bool AllStatChangesHaveEffectOn(Item item) {
		return base.AllStatChangesHaveEffectOn(item);
	}
	public sealed override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {
		base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
	}

}
public class Evasive : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.DodgeChance += .04f;
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+4% dodge chance") {
			IsModifier = true,
		};
	}
}
public class Vital : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: 20);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+20 maximum health") {
			IsModifier = true,
		};
	}
}

public class Cunning : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.14f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+14% critical damage") {
			IsModifier = true,
		};
	}
}

public class Stealthy : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, 1.28f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+28% First strike damage") {
			IsModifier = true,
		};
	}
}
public class Spiky : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, 1.06f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+6% thorn damage") {
			IsModifier = true,
		};
	}
}
public class Vampiric : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.LifeSteal += .04f;
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+4% life steal") {
			IsModifier = true,
		};
	}
}

public class Energetic : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.EnergyCap, Base: 50);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+50 maximum energy") {
			IsModifier = true,
		};
	}
}

public class Alchemic : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1.1f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+10% debuff damage") {
			IsModifier = true,
		};
	}
}

public class Jumpy : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.04f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+4% jump boost") {
			IsModifier = true,
		};
	}
}
public class Holy : BaseAccPrefix {
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Iframe, Base: 10);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+10 invincibility frame") {
			IsModifier = true,
		};
	}
}
