using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Roguelike.Common.Global.ItemVariant;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.WeaponEffect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace Roguelike.Common.Global {
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
	/// <summary>
	/// This class hold mainly tooltip information and other general stats or field<br/>
	/// However this doesn't handle overhaul information
	/// </summary>
	public class GlobalItemHandle : GlobalItem {
		/// <summary>
		/// Use this to set variant before using Player.QuickSpawnItem or Item.NewItemDirect<br/>
		/// This is a hacky way of setting up custom stats for item
		/// </summary>
		public const byte None = 0;
		public override bool InstancePerEntity => true;
		public bool DebugItem = false;
		public bool ExtraInfo = false;
		public bool AdvancedBuffItem = false;
		public bool OverrideVanillaEffect = false;
		public int Counter = 0;
		public short VariantType = -1;
		public int ItemLevel = 0;
		public bool IsASword = false;
		public int OutroEffect_type = -1;
		public int InventoryWhoAmI = -1;
		public List<int> list_WeaponEffectType = new();
		public void SetItemLevel(int level) {
			int amountAdd = level / 5;
			if (amountAdd > 0) {
				for (int i = 0; i < amountAdd; i++) {
					list_WeaponEffectType.Add(Main.rand.Next(WeaponEffectSystem.list_effect.Count));
				}
			}
			ItemLevel = level;
		}
		public override GlobalItem NewInstance(Item target) {
			if (target.TryGetGlobalItem(out GlobalItemHandle handler)) {
				handler.ItemLevel = 0;
				handler.OutroEffect_type = -1;
				handler.InventoryWhoAmI = -1;
				handler.Counter = 0;
				list_WeaponEffectType = new();
			}
			return base.NewInstance(target);
		}
		public bool CheckVariant() => VariantType >= 0 && VariantType != ModVariant.GetVariantType<None_Var>();
		public override void OnCreated(Item item, ItemCreationContext context) {
			item.prefix = 0;
		}
		public override void SetDefaults(Item entity) {
			if (OutroEffect_type == -1) {
				OutroEffect_type = OutroEffect.GetOutroEffectType<OutroEffect_None>();
			}
			if (CheckVariant()) {
				var variant = WorldVaultSystem.GetVariant(VariantType);
				if (variant != null) {
					variant.SetDefault(entity);
				}
			}
			entity.prefix = 0;
		}
		public override bool CanUseItem(Item item, Player player) {
			return base.CanUseItem(item, player);
		}
		public override void HoldItem(Item item, Player player) {
			UpdateCriticalDamage = 0;
			if (CheckVariant()) {
				var variant = WorldVaultSystem.GetVariant(VariantType);
				if (variant != null) {
					variant.UpdateInv(item, player);
				}
			}
			for (int i = 0; i < list_WeaponEffectType.Count; i++) {
				int effectID = list_WeaponEffectType[i];
				WeaponEffect effect = WeaponEffectSystem.GetEffect(effectID);
				if (effect != null) {
					effect.UpdateItem(player, item, this);
				}
			}
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (CheckVariant()) {
				var variant = WorldVaultSystem.GetVariant(VariantType);
				if (variant != null) {
					variant.Shoot(item, player, source, position, velocity, type, damage, knockback);
				}
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public float CriticalDamage = 0;
		public float UpdateCriticalDamage;
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
			if (ItemLevel <= 0) {
				return;
			}
			damage += .02f * ItemLevel;
			damage.Base += (int)(ItemLevel * .5f);
			for (int i = 0; i < list_WeaponEffectType.Count; i++) {
				int effectID = list_WeaponEffectType[i];
				WeaponEffect effect = WeaponEffectSystem.GetEffect(effectID);
				if (effect != null) {
					effect.ModifyWeaponDamage(player, item, ref damage);
				}
			}
		}
		public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
			if (ItemLevel <= 0) {
				return;
			}
			crit += ItemLevel / 3;
			UpdateCriticalDamage += .05f * (ItemLevel / 4);
			for (int i = 0; i < list_WeaponEffectType.Count; i++) {
				int effectID = list_WeaponEffectType[i];
				WeaponEffect effect = WeaponEffectSystem.GetEffect(effectID);
				if (effect != null) {
					effect.ModifyWeaponCrit(player, item, ref crit);
				}
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (UniversalSystem.EnchantingState) {
				return;
			}
			//tooltips.Add(new(Mod, "Debug", $"Item width : {item.width} | height {item.height}"));
			if (item.IsAWeapon(true)) {
				for (int i = 0; i < tooltips.Count; i++) {
					TooltipLine line = tooltips[i];
					if (tooltips[i].Name == "ItemName") {
						if (ItemLevel > 0) {
							tooltips[i].Text = $"+{ItemLevel} {tooltips[i].Text}";
						}
					}
					if (line.Name == "CritChance") {
						tooltips.Insert(i + 1, new(Mod, "CritDamage", $"{Math.Round(CriticalDamage, 2) * 100}% bonus critical damage"));
						tooltips.Insert(i + 2, new(Mod, "ArmorPenetration", $"{item.ArmorPenetration} Armor penetration"));
					}
					else if (line.Name == "Damage") {
						line.Text = line.Text + $" | Base : {item.OriginalDamage}";
					}
					else if (line.Name == "Knockback") {
						line.Text = line.Text + $" | Base : {Math.Round(ContentSamples.ItemsByType[item.type].knockBack, 2)} | Modified : {Math.Round(Main.LocalPlayer.GetWeaponKnockback(item), 2)}";
					}
				}
			}
			if (ModContent.GetInstance<UniversalSystem>().user2ndInterface.CurrentState == ModContent.GetInstance<UniversalSystem>().transmutationUI) {
				tooltips.Add(new(Mod, "RarityValue", $"Rarity : [c/{ItemRarity.GetColor(item.OriginalRarity).Hex3()}:{item.OriginalRarity}]"));
			}
			Player player = Main.LocalPlayer;
			ProcessTriggerPlayer moddedplayer = player.GetModPlayer<ProcessTriggerPlayer>();
			for (int i = 0; i < list_WeaponEffectType.Count; i++) {
				int effectID = list_WeaponEffectType[i];
				WeaponEffect effect = WeaponEffectSystem.GetEffect(effectID);
				if (effect != null) {
					tooltips.Add(new TooltipLine(Mod, "WeaponEffect", $"[i:{ItemID.FallenStar}] {effect.Description}"));
				}
			}
			if (item.ModItem != null) {
				if (ExtraInfo) {
					if (!moddedplayer.Shift_Option()) {
						tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
					}
				}
			}
			else {
				if (item.IsAWeapon()) {
					if (!OutroEffectSystem.Has_WeaponTag(item.type)) {
						ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
					}
					else {
						if (!moddedplayer.Shift_Option()) {
							tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for weapon tag information]") { OverrideColor = Color.Gray });
						}
						else {
							string value = "This weapon is classified as following: \n";
							value += ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
							if (OutroEffect_type != -1) {
								OutroEffect ef = OutroEffectSystem.GetOutroEffect(OutroEffect_type);
								if (ef != null && ef.Type != OutroEffect.GetOutroEffectType<OutroEffect_None>()) {
									value += $"\nOutro effect: \n{ef.DisplayName}\n- {ef.ModifyTooltip()}";
								}
							}
							tooltips.Add(new TooltipLine(Mod, "Shift_Info", value) { OverrideColor = new Color(255, 255, 0, 0) });
						}
					}
				}
			}
			if (item.ModItem == null) {
				return;
			}
			if (item.ModItem.Mod != Mod) {
				return;
			}
			TooltipLine NameLine = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
			if (DebugItem && NameLine != null) {
				NameLine.Text += " [Debug]";
				NameLine.OverrideColor = Color.MediumPurple;
				return;
			}
			if (AdvancedBuffItem && NameLine != null) {
				NameLine.Text += " [Advanced]";
			}
		}
		public override void PostUpdate(Item item) {
			if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) && RoguelikeWorldProperty.BossRushWorld) {
				if (!Main.LocalPlayer.dead && item.type != ItemID.Heart && item.type != ItemID.Star && item.position.IsCloseToPosition(Main.LocalPlayer.Center, 1000)) {
					item.velocity = (Main.LocalPlayer.Center - item.Center).SafeNormalize(Vector2.Zero) * 5;
				}
			}
		}
		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
			//Prevent possible conflict, basically hardcoding to make it so that it only work for item belong to this mod
			string value = null;
			if (item.ModItem != null) {
				if (item.ModItem.Mod.Name != Mod.Name) {
					return true;
				}
				if (ExtraInfo) {
					value = ModUtils.LocalizationText("Items", $"{item.ModItem.Name}.ExtraInfo");
				}
			}
			if (value == null) {
				return base.PreDrawTooltip(item, lines, ref x, ref y); ;
			}
			ProcessTriggerPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ProcessTriggerPlayer>();
			if (moddedplayer.Shift_Option()) {
				float width;
				float height = -16;
				Vector2 pos;
				DynamicSpriteFont font = FontAssets.MouseText.Value;
				if (Main.MouseScreen.X < Main.screenWidth / 2) {
					string widest = lines.OrderBy(n => ChatManager.GetStringSize(font, n.Text, Vector2.One).X).Last().Text;
					width = ChatManager.GetStringSize(font, widest, Vector2.One).X;
					pos = new Vector2(x, y) + new Vector2(width + 30, 0);
				}
				else {
					width = ChatManager.GetStringSize(font, value, Vector2.One).X + 20;
					pos = new Vector2(x, y) - new Vector2(width + 30, 0);
				}
				width = ChatManager.GetStringSize(font, value, Vector2.One).X + 20;
				height += ChatManager.GetStringSize(font, value, Vector2.One).Y + 16;
				Terraria.Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)width + 20, (int)height + 20), new Color(25, 100, 55) * 0.85f);
				Terraria.Utils.DrawBorderString(Main.spriteBatch, value, pos, Color.White);
				pos.Y += ChatManager.GetStringSize(font, value, Vector2.One).Y + 16;
			}
			return base.PreDrawTooltip(item, lines, ref x, ref y);
		}
		public override bool? UseItem(Item item, Player player) {
			//if (AdvancedBuffItem && !UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
			//	player.AddBuff(ModContent.BuffType<Drawback>(), ModUtils.ToMinute(6));
			//}
			return base.UseItem(item, player);
		}
		public override void SaveData(Item item, TagCompound tag) {
			tag["VariantType"] = VariantType;
			tag["WeaponEffectList"] = list_WeaponEffectType;
			tag["ItemLevel"] = ItemLevel;
		}
		public override void LoadData(Item item, TagCompound tag) {
			VariantType = tag.Get<short>("VariantType");
			list_WeaponEffectType = tag.Get<List<int>>("WeaponEffectList");
			ItemLevel = tag.Get<int>("ItemLevel");
		}
	}
}
