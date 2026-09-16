using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.Global.Prefixes;
internal class RoguelikeModdedPrefixSystem : ModSystem {
	public static List<RoguelikePrefix> list_prefix = new();
	public static int TotalCount => list_prefix.Count;
	public static RoguelikePrefix GetPrefix(int type) {
		return type >= 0 && type < list_prefix.Count ? list_prefix[type] : null;
	}
	public static short Register(RoguelikePrefix prefix) {
		ModTypeLookup<RoguelikePrefix>.Register(prefix);
		list_prefix.Add(prefix);
		return (short)(list_prefix.Count - 1);
	}
}
public class RoguelikePrefixSystem_GlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public short Roguelike_Prefix = -1;
	public override void UpdateEquip(Item item, Player player) {
		var prefix = RoguelikeModdedPrefixSystem.GetPrefix(Roguelike_Prefix);
		if (prefix == null) {
			return;
		}
		prefix.UpdateEquip(item, player);
	}
	public override void SaveData(Item item, TagCompound tag) {
		tag["Roguelike_Prefix"] = Roguelike_Prefix;
	}
	public override void LoadData(Item item, TagCompound tag) {
		Roguelike_Prefix = tag.Get<short>("Roguelike_Prefix");
	}
}
public abstract class RoguelikePrefix : ModType {
	public short Type { get; private set; }
	public short GetPrefixType<T>() where T : RoguelikePrefix => ModContent.GetInstance<T>().Type;
	public string Description => ModUtils.LocalizationText("RoguelikePrefix", $"{Name}");
	protected sealed override void Register() {
		SetDefaults();
		Type = RoguelikeModdedPrefixSystem.Register(this);
	}
	public override sealed bool Equals(object obj) {
		return base.Equals(obj);
	}
	public override sealed int GetHashCode() {
		return base.GetHashCode();
	}
	public override sealed bool IsLoadingEnabled(Mod mod) {
		return base.IsLoadingEnabled(mod);
	}
	public override sealed void Load() {
		base.Load();
	}
	public override sealed void SetStaticDefaults() {
		base.SetStaticDefaults();
	}
	public override sealed void SetupContent() {
		base.SetupContent();
	}
	public override sealed string ToString() {
		return base.ToString();
	}
	public override sealed void Unload() {
		base.Unload();
	}
	public virtual void SetDefaults() { }
	public virtual void UpdateEquip(Item item, Player player) { }
}
