using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
/// <summary>
/// This should be ties with a modplayer for easy ease of uses and also easy to keep track
/// </summary>
public abstract class RelicSet : ModType {
	public static short GetRelicSetType<T>() where T : RelicSet {
		return ModContent.GetInstance<T>().Type;
	}
	public string DisplayName => Language.GetTextValue($"Mods.Roguelike.RelicSet.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.Roguelike.RelicSet.{Name}.Description");
	public short Type { get; private set; }
	public byte Requirement = 0;
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = RelicSetSystem.Register(this);
	}
}
public static class RelicSetSystem {
	public static bool Check_RelicSetRequirment(Player player, int type) {
		if (player.TryGetModPlayer(out RelicSetPlayerHandle handle)) {
			if (type < 0 || type >= handle.RelicSet.Length) {
				return false;
			}
			return handle.RelicSet[type] >= set[type].Requirement;
		}
		return false;
	}
	private static readonly List<RelicSet> set = new();
	public static int TotalCount => set.Count;
	public static short Register(RelicSet template) {
		ModTypeLookup<RelicSet>.Register(template);
		set.Add(template);
		return (short)(set.Count - 1);
	}
	public static RelicSet GetSet(int type) {
		return type >= 0 && type < set.Count ? set[type] : null;
	}
}
public class RelicSetPlayerHandle : ModPlayer {
	/// <summary>
	/// Relic set type will point to the value in the array<br/>
	/// </summary>
	public int[] RelicSet = new int[RelicSetSystem.TotalCount];
	public override void Initialize() {
		Array.Resize(ref RelicSet, RelicSetSystem.TotalCount);
	}
	public override void ResetEffects() {
		Array.Fill(RelicSet, 0);
	}
}
