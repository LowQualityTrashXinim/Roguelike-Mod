using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.IOhandle;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
using Roguelike.Contents.NPCs;
using Roguelike.Contents.Transfixion.Augmentation;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Contents.Transfixion.WeaponEnchantment;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon;
public struct SynergyBonus {
	public int ItemID;
	public bool Active;
	public string Tooltip = "";

	public SynergyBonus(int id) {
		ItemID = id;
	}
	public SynergyBonus(int id, string tooltip) {
		ItemID = id;
		Tooltip = tooltip;
	}
}
/// <summary>
/// This is synergy bonus system, this system will automatically handle most of the bonus action for you<br/>
/// No need to manual set, nor anything, you only need to check whenever or not if a bonus is active or not
/// </summary>
public class SynergyBonus_System : ModSystem {
	public static Dictionary<int, List<SynergyBonus>> Dictionary_SynergyBonus = new();
	public override void Load() {
		Dictionary_SynergyBonus = new();
		On_Item.NewItem_Inner += On_Item_NewItem_Inner;
	}

	private int On_Item_NewItem_Inner(On_Item.orig_NewItem_Inner orig, IEntitySource source, int X, int Y, int Width, int Height, Item itemToClone, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup) {
		int whoAmI = orig(source, X, Y, Width, Height, itemToClone, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
		Item item = Main.item[whoAmI];
		if (ModContent.GetInstance<RogueLikeConfig>().TotalRNG) {
			if (Main.rand.NextBool(5)) {
				if (item.IsAWeapon()) {
					int amount = 1 + Main.rand.Next(3);
					for (int i = 0; i < amount; i++) {
						EnchantmentSystem.EnchantItem(ref item, i);
					}
					if (item.TryGetGlobalItem(out GlobalItemHandle handler)) {
						handler.SetItemLevel(Main.rand.Next(0, 16));
					}
				}
				if (item.accessory) {
					AugmentsWeapon.AddAugments(ref item, Main.rand.Next(1, AugmentsLoader.TotalCount));
					if (item.TryGetGlobalItem(out AugmentsWeapon acc)) {
						acc.Modify_Charge(Main.rand.Next(255));
					}
				}
			}
		}
		return whoAmI;
	}

	public override void Unload() {
		Dictionary_SynergyBonus = null;
	}
	public static void Add_SynergyBonus(int SynergyItemID, int ItemID, string tooltip = "") {
		if (Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
			if (Dictionary_SynergyBonus[SynergyItemID].Select(b => b.ItemID).ToArray().Contains(ItemID)) {
				return;
			}
			Dictionary_SynergyBonus[SynergyItemID].Add(new(ItemID, tooltip));
			return;
		}
		Dictionary_SynergyBonus.Add(SynergyItemID, new() { { new(ItemID, tooltip) } });
	}
	/// <summary>
	/// Check if the synergy bonus is active or not<br/>
	/// <b>Note :</b> If you are checking a item group, check the key item instead
	/// </summary>
	/// <param name="SynergyItemID"></param>
	/// <param name="ItemID"></param>
	/// <returns></returns>
	public static bool Check_SynergyBonus(int SynergyItemID, int ItemID) {
		if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
			return false;
		}
		for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
			SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
			if (bonus.ItemID == ItemID) {
				return bonus.Active;
			}
		}
		return false;
	}
	public static string Get_SynergyBonusTooltip(int SynergyItemID, int itemID) {
		if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
			return "Synergy item not found !";
		}
		for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
			SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
			if (bonus.ItemID == itemID) {
				return bonus.Tooltip;
			}
		}
		return "Synergy bonus item not found !";
	}
	public static void Write_SynergyTooltip(ref List<TooltipLine> lines, SynergyModItem moditem, int itemID) {
		int SynergyItemID = moditem.Type;
		if (Main.LocalPlayer.HeldItem.type != moditem.Type) {
			return;
		}
		if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
			return;
		}
		SynergyBonus bonus = new();
		for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
			if (Dictionary_SynergyBonus[SynergyItemID][i].ItemID == itemID) {
				bonus = Dictionary_SynergyBonus[SynergyItemID][i];
			}
		}
		if (bonus.Active)
			lines.Add(new(moditem.Mod, moditem.Set_TooltipName(itemID), bonus.Tooltip));
	}
	/// <summary>
	/// return item id of synergy item
	/// </summary>
	/// <param name="SynergyItemID"></param>
	/// <returns></returns>
	public static int[] Get_SynergyBonus(int SynergyItemID) {
		if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
			return null;
		}
		else {
			return Dictionary_SynergyBonus[SynergyItemID].Select(sy => sy.ItemID).ToArray();
		}
	}

	public bool GodAreEnraged = false;
	public int CooldownCheck = 999;
	private void SynergyEnergyCheckPlayer(Player player) {
		int synergyCounter = 0;
		synergyCounter += player.CountItem(ModContent.ItemType<SynergyEnergy>(), 2);
		synergyCounter += player.inventory.Where(itemInv => itemInv.ModItem is SynergyModItem).Count();
		int maxCount = NPC.GetActivePlayerCount() + 1;
		if (synergyCounter >= maxCount) {
			GodAreEnraged = true;
		}
	}
	private void GodDecision(Player player) {
		if (Main.netMode == NetmodeID.MultiplayerClient)
			return;
		if (NPC.AnyNPCs(ModContent.NPCType<Guardian>()) || player.GetModPlayer<PlayerStatsHandle>().CanDropSynergyEnergy)
			return;
		if (player.IsDebugPlayer())
			return;
		CooldownCheck = ModUtils.CountDown(CooldownCheck);
		//Main.NewText(CooldownCheck);
		if (CooldownCheck <= 0) {
			SynergyEnergyCheckPlayer(player);
		}
		if (GodAreEnraged) {
			Vector2 randomSpamLocation = Main.rand.NextVector2CircularEdge(1500, 1500) + player.Center;
			NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)randomSpamLocation.X, (int)randomSpamLocation.Y, ModContent.NPCType<Guardian>());
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Red, "You have anger the God!");
			CooldownCheck = 999;
			GodAreEnraged = false;
		}
	}
	public override void PostUpdateWorld() {
	}
}
public class SynergyGlobalItem : GlobalItem {
	public override void OnCreated(Item item, ItemCreationContext context) {
		if (item.ModItem == null) {
			return;
		}
		if (item.ModItem is SynergyModItem && context is RecipeItemCreationContext) {
			ModUtils.AmmoForWeapon(Main.LocalPlayer, item.type);
		}
	}
}
/// <summary>
///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
/// </summary>
public class PlayerSynergyItemHandle : ModPlayer {
	public bool SynergyBonusBlock = false;
	public int SynergyBonus = 0;

	public int Annihiliation_Counter = 0;

	public override void ResetEffects() {
		SynergyBonus = 0;
		SynergyBonusBlock = false;

		if (Player.HeldItem.type != ModContent.ItemType<Annihiliation>()) {
			Annihiliation_Counter = 0;
		}
		if (!Player.HeldItem.IsAWeapon()) {
			return;
		}
		if (!ModItemLib.SynergyItem.Select(i => i.type).Contains(Player.HeldItem.type)) {
			return;
		}
		int synergyItem = Player.HeldItem.type;
		if (!SynergyBonus_System.Dictionary_SynergyBonus.ContainsKey(synergyItem)) {
			return;
		}
		int SynergyBonusLength = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem].Count;
		for (int l = 0; l < SynergyBonusLength; l++) {
			int itemIDBonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l].ItemID;
			bool HasItem = Player.HasItem(itemIDBonus);
			if (HasItem) {
				SynergyBonus++;
			}
			SynergyBonus bonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l];
			bonus.Active = HasItem;
			SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l] = bonus;
		}
	}
}
public abstract class SynergyModItem : ModItem {
	public string Set_TooltipName(int ItemID) => $"{Name}_{ContentSamples.ItemsByType[ItemID].Name}";
	public sealed override void SetStaticDefaults() {
		ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
		CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
		Synergy_SetStaticDefaults();
	}
	public virtual void Synergy_SetStaticDefaults() { }
	public ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
	public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
		ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
		if (CustomColor != null) {
			tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
		}
	}
	public override sealed void ModifyWeaponCrit(Player player, ref float crit) {
		Synergy_ModifyWeaponCrit(player, ref crit);
		if (!player.GetModPlayer<PerkPlayer>().perk_UntappedPotential) {
			return;
		}
		PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
		crit += 4 * modplayer.SynergyBonus;
	}
	public virtual void Synergy_ModifyWeaponCrit(Player player, ref float crit) { }
	public override sealed void ModifyWeaponDamage(Player player, ref StatModifier damage) {
		Synergy_ModifyWeaponDamage(player, ref damage);
		if (!player.GetModPlayer<PerkPlayer>().perk_UntappedPotential) {
			return;
		}
		float damageIncreasement = 0;
		float damageMultiplier = 0;
		PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
		if (modplayer.SynergyBonus > 0) {
			damageMultiplier += 0.025f * modplayer.SynergyBonus;
		}
		else {
			damageMultiplier += 0.01f;
		}
		for (int i = 0; player.inventory.Length > 0; i++) {
			if (i > 50) {
				break;
			}
			Item item = player.inventory[i];
			if (!item.IsAWeapon() || item == Item || item.ModItem is SynergyModItem) {
				continue;
			}
			damageIncreasement += player.inventory[i].damage * damageMultiplier;
		}
		damage += damageIncreasement;
	}
	public virtual void Synergy_ModifyWeaponDamage(Player player, ref StatModifier damage) { }
	public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
	public override sealed void HoldItem(Player player) {
		string internalItemName = Item.ModItem.Name;
		if (!SynergyBonus_System.Dictionary_SynergyBonus.ContainsKey(Type)) {
			return;
		}
		List<SynergyBonus> listBonus = SynergyBonus_System.Dictionary_SynergyBonus[Type];
		if (!RoguelikeData.SynergyProgressTracker.ContainsKey(internalItemName)) {
			RoguelikeData.SynergyProgressTracker.Add(internalItemName, new());
			foreach (SynergyBonus bonus in listBonus) {
				SynergyBonus defaultBonus = new(bonus.ItemID);
				RoguelikeData.SynergyProgressTracker[internalItemName].Add(defaultBonus);
			}
		}
		else {
			if (RoguelikeData.SynergyProgressTracker[internalItemName].Count != listBonus.Count) {
				RoguelikeData.SynergyProgressTracker[internalItemName].Clear();
				foreach (SynergyBonus bonus in listBonus) {
					SynergyBonus defaultBonus = new(bonus.ItemID);
					RoguelikeData.SynergyProgressTracker[internalItemName].Add(defaultBonus);
				}
			}
			for (int i = 0; i < listBonus.Count; i++) {
				if (listBonus[i].Active) {
					SynergyBonus bonus = listBonus[i];
					bonus.Tooltip = "";
					RoguelikeData.SynergyProgressTracker[internalItemName][i] = bonus;
				}
			}
		}
	}
	public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
	}
	public override sealed void UpdateInventory(Player player) {
		base.UpdateInventory(player);
		//Very funny that hold item happen after ModifyWeaponDamage
		//This probably will tank our mod performance, but well, it is what it is
		PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
		if (player.HeldItem == Item) {
			HoldSynergyItem(player, modplayer);
		}
		SynergyUpdateInventory(player, modplayer);
	}
	public virtual void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {

	}
	public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

	}
	/// <summary>
	/// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
	/// </summary>
	/// <param name="player"></param>
	/// <param name="modplayer"></param>
	public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
	public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
		return CanShootItem;
	}
	public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
	public override sealed void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		base.OnHitNPC(player, target, hit, damageDone);
		OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
	}
	public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) { }

	private int countX = 0;
	private float positionRotateX = 0;
	private int rotate = 0;
	private void PositionHandle() {
		if (positionRotateX < 3.5f && countX == 1) {
			positionRotateX += .2f;
		}
		else {
			countX = -1;
		}
		if (positionRotateX > 0 && countX == -1) {
			positionRotateX -= .2f;
		}
		else {
			countX = 1;
		}
	}
	Color auraColor;
	private void ColorHandle() {
		switch (Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus) {
			case 1:
				auraColor = new Color(255, 50, 0, 30);
				break;
			case 2:
				auraColor = new Color(255, 255, 0, 30);
				break;
			case 3:
				auraColor = new Color(0, 255, 255, 30);
				break;
			default:
				auraColor = new Color(255, 255, 255, 30);
				break;
		}
	}
	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		PositionHandle();
		ColorHandle();
		rotate = ModUtils.Safe_SwitchValue(rotate, (int)(MathHelper.Pi * 1000));
		if (ItemID.Sets.AnimatesAsSoul[Type] || Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus < 1 || Main.LocalPlayer.HeldItem.type != Type) {
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		int[] synegyB = SynergyBonus_System.Get_SynergyBonus(Type);
		if (synegyB != null) {
			int len = synegyB.Length;
			for (int i = 0; i < len; i++) {
				if (!SynergyBonus_System.Dictionary_SynergyBonus[Type][i].Active) {
					continue;
				}
				int type = synegyB[i];
				Main.instance.LoadItem(type);
				Texture2D synergyBonus = TextureAssets.Item[type].Value;
				spriteBatch.Draw(synergyBonus, position + Vector2.One.RotatedBy(MathHelper.ToRadians(rotate) + MathHelper.Pi * MathHelper.Lerp(0, 1, i / (float)(len))) * 10, synergyBonus.Frame(), Color.White, 0, synergyBonus.Size() * .5f, .55f, SpriteEffects.None, 0);
			}
		}
		Main.instance.LoadItem(Type);
		Texture2D texture = TextureAssets.Item[Type].Value;
		for (int i = 0; i < 3; i++) {
			spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
		}
		return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
public abstract class SynergyBuff : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override sealed void SetStaticDefaults() {
		base.SetStaticDefaults();
		SynergySetStaticDefaults();
	}
	public virtual void SynergySetStaticDefaults() {

	}
	public override sealed void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
		UpdatePlayer(player, ref buffIndex);
	}
	public virtual void UpdatePlayer(Player player, ref int buffIndex) {

	}
	public override sealed void Update(NPC npc, ref int buffIndex) {
		base.Update(npc, ref buffIndex);
		UpdateNPC(npc, ref buffIndex);
	}
	public virtual void UpdateNPC(NPC npc, ref int buffIndex) {

	}
}
