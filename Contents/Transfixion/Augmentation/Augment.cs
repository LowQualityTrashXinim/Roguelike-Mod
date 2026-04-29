using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Roguelike.Contents.Transfixion.Augmentation;
internal class AugmentsLoader : ModSystem {
	private static readonly List<ModAugments> _Augmentss = new();
	public static int TotalCount => _Augmentss.Count;
	public static int Register(ModAugments enchant) {
		ModTypeLookup<ModAugments>.Register(enchant);
		_Augmentss.Add(enchant);
		return _Augmentss.Count;
	}
	public static List<ModAugments> ReturnListOfAugment() => _Augmentss;
	public static ModAugments GetAugments(int type) {
		return type > 0 && type <= _Augmentss.Count ? _Augmentss[type - 1] : null;
	}
}
public class AugmentsWeapon : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.accessory;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		var Augments = AugmentsLoader.GetAugments(Augment);
		if (Augments == null) {
			return;
		}
		var line = Augments.ModifyDescription(Main.LocalPlayer, this, item, Check_ChargeConvertToStackAmount());
		line.Text = $"{Augments.ModifyName(Main.LocalPlayer, this, item, Check_ChargeConvertToStackAmount())} : {line.Text}";
		ModUtils.AddTooltip(ref tooltips, line);
	}
	public override bool InstancePerEntity => true;
	public int Augment = -1;
	public int AugmentCharge = -1;
	public static void AddAugments<T>(ref Item item) where T : ModAugments {
		if (!item.accessory) {
			return;
		}
		int type = ModAugments.GetAugmentType<T>();
		var aug = AugmentsLoader.GetAugments(type);
		if (aug == null) {
			Main.NewText($"Augmentation not found ! Look up type: {type}");
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon acc)) {
			acc.Augment = type;
		}
	}
	public static void AddAugments(ref Item item, int type) {
		if (!item.accessory) {
			return;
		}
		var aug = AugmentsLoader.GetAugments(type);
		if (aug == null) {
			Main.NewText($"Augmentation not found ! Look up type: {type}");
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon acc)) {
			acc.Augment = type;
		}
	}
	public void Modify_Charge(int amount) {
		AugmentCharge += amount;
	}
	public int Check_ChargeConvertToStackAmount() {
		int Amount = AugmentCharge % 50;
		if (AugmentCharge >= 250) {
			return Amount;
		}
		else if (AugmentCharge == 0) {
			return 0;
		}
		return Amount + 1;
	}
	public override GlobalItem NewInstance(Item target) {
		if (target.TryGetGlobalItem(out AugmentsWeapon acc)) {
			acc.Augment = -1;
			acc.AugmentCharge = -1;
		}
		return base.NewInstance(target);
	}
	public override GlobalItem Clone(Item from, Item to) {
		var clone = (AugmentsWeapon)base.Clone(from, to);
		if (from.TryGetGlobalItem(out AugmentsWeapon acc)) {
			clone.Augment = acc.Augment;
			clone.AugmentCharge = acc.AugmentCharge;
		}
		return clone;
	}
	public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
		var augmentationplayer = player.GetModPlayer<AugmentsPlayer>();
		var Augments = AugmentsLoader.GetAugments(Augment);
		if (Augments == null) {
			return;
		}
		augmentationplayer.accItemUpdate.Add(item);
		augmentationplayer.valid++;
		Augments.UpdateAccessory(player, this, item);

	}
	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("Augment", Augment);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("Augment", out int TypeValue))
			Augment = TypeValue;
	}
}
public abstract class ModAugments : ModType {
	public int Type { get; internal set; }
	protected override void Register() {
		Type = AugmentsLoader.Register(this);
		SetStaticDefaults();
	}
	public static int GetAugmentType<T>() where T : ModAugments => ModContent.GetInstance<T>().Type;
	public Color tooltipColor = Color.White;
	public string DisplayName => ModUtils.LocalizationText("ModAugments", $"{Name}.DisplayName");
	public string Description => ModUtils.LocalizationText("ModAugments", $"{Name}.Description");
	protected string Description2(string Extra) => ModUtils.LocalizationText("ModAugments", $"{Name}.Description{Extra}");
	public virtual TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			string num = i.ToString();
			if (i == 0) {
				num = "";
			}
			string text = Description2(num);
			if (text == "") {
				break;
			}
			desc += "\n" + Description2(num);
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public string ColorWrapper(string Name) => $"[c/{tooltipColor.Hex3()}:{Name}]";
	public virtual void OnAdded(Player player, Item itme, AugmentsWeapon acc) { }
	public virtual string ModifyName(Player player, AugmentsWeapon acc, Item item, int stack) {
		string name = DisplayName;
		return ColorWrapper(name + ModUtils.Convert_NumberToRomanNumerals(stack));
	}
	public virtual void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, AugmentsWeapon acc, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPCWithProj(Player player, AugmentsWeapon acc, Projectile proj, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitByNPC(Player player, AugmentsWeapon acc, NPC npc, Player.HurtInfo info) { }
	public virtual void OnHitByProj(Player player, AugmentsWeapon acc, Projectile projectile, Player.HurtInfo info) { }
	public virtual void UpdateAccessory(Player player, AugmentsWeapon acc, Item item) { }
}
public class AugmentsPlayer : ModPlayer {
	public List<Item> accItemUpdate = new();
	/// <summary>
	/// The amount of augmentation currently equipped
	/// </summary>
	public int valid = 0;
	public override void ResetEffects() {
		valid = 0;
		accItemUpdate.Clear();
	}
	private static bool IsAugmentsable(Item item) => item.accessory;
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var itemAcc in accItemUpdate) {
			if (IsAugmentsable(itemAcc)) {
				var moditem = itemAcc.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.OnHitNPCWithItem(Player, moditem, item, target, hit);
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var item in accItemUpdate) {
			if (IsAugmentsable(item)) {
				var moditem = item.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.OnHitNPCWithProj(Player, moditem, proj, target, hit);
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var item in accItemUpdate) {
			if (IsAugmentsable(item)) {
				var moditem = item.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.ModifyHitNPCWithProj(Player, moditem, proj, target, ref modifiers);
			}
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				var moditem = acc.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.ModifyHitNPCWithItem(Player, moditem, item, target, ref modifiers);
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				var moditem = acc.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.OnHitByNPC(Player, moditem, npc, hurtInfo);
			}
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				var moditem = acc.GetGlobalItem<AugmentsWeapon>();
				var Augments = AugmentsLoader.GetAugments(moditem.Augment);
				if (Augments == null) {
					continue;
				}
				Augments.OnHitByProj(Player, moditem, proj, hurtInfo);
			}
		}
	}
}
