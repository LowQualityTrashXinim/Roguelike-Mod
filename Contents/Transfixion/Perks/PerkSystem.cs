using System;
using Terraria;
using System.IO;
using Terraria.ID;
using System.Linq;
using Terraria.UI;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Roguelike.Common.Systems;
using Roguelike.Contents.Transfixion.Artifacts;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Texture;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks.BlessingPerk;
using Roguelike.Contents.Transfixion.Perks.RoguelikePerk;

namespace Roguelike.Contents.Transfixion.Perks {
	public class PerkItem : GlobalItem {
		public override bool? UseItem(Item item, Player player) {
			var perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionExpert && item.buffType > 0) {
				if (player.ItemAnimationJustStarted) {
					perkplayer.PotionExpert_perk_CanConsume = Main.rand.NextFloat() <= .55f;
				}
				return perkplayer.PotionExpert_perk_CanConsume;
			}
			return base.UseItem(item, player);
		}

		// how is drinking a potion with left click works differently from quick heal?... talking about a fresh spaghetti serving right there.
		public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
			var perkplayer = player.GetModPlayer<PerkPlayer>();
			var healingPotionstat = StatModifier.Default;
			if (perkplayer.perk_PotionCleanse) {
				healingPotionstat -= .5f;
			}
			if (perkplayer.perk_ImprovedPotion) {
				healingPotionstat += .7f;
			}
			healingPotionstat = healingPotionstat.CombineWith(player.ModPlayerStats().HealEffectiveness);
			healValue = (int)healingPotionstat.ApplyTo(healValue);
		}

		public override bool ConsumeItem(Item item, Player player) {
			var perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionCleanse && item.healLife > 0) {
				foreach (int i in player.buffType) {
					if (Main.debuff[i] && i != BuffID.PotionSickness) {
						player.ClearBuff(i);

					}
				}
			}
			return base.ConsumeItem(item, player);
		}
	}
	public class PerkModSystem : ModSystem {
		public static List<int> StarterPerkType { get; private set; } = new();
		public static List<int> WeaponUpgradeType { get; private set; } = new();
		public override void Load() {
			base.Load();
			On_Player.QuickMana += On_Player_QuickMana;
			if (StarterPerkType == null) {
				StarterPerkType = new();
			}
			if (WeaponUpgradeType == null) {
				WeaponUpgradeType = new();
			}
		}
		public override void Unload() {
			StarterPerkType = null;
			WeaponUpgradeType = null;
		}
		private void On_Player_QuickMana(On_Player.orig_QuickMana orig, Player self) {
			var perkplayer = self.GetModPlayer<PerkPlayer>();
			if (self.HasBuff(ModContent.BuffType<ManaBlock>()) && perkplayer.perk_ImprovedPotion) {
				return;
			}
			orig(self);
		}
		public override void PostSetupContent() {
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (ModPerkLoader.GetPerk(i).list_category.Contains(PerkCategory.Starter)) {
					StarterPerkType.Add(i);
				}
				if (ModPerkLoader.GetPerk(i).list_category.Contains(PerkCategory.WeaponUpgrade)) {
					WeaponUpgradeType.Add(i);
				}
			}
		}
	}
	class MagicOverhaulBuff : GlobalBuff {
		public override void Update(int type, Player player, ref int buffIndex) {
			if (type == BuffID.ManaSickness && player.GetModPlayer<PerkPlayer>().perk_ImprovedPotion) {
				if (player.statMana < player.statManaMax2) {
					player.statMana += 2;
				}
				if (player.buffTime[buffIndex] <= 0) {
					player.AddBuff(ModContent.BuffType<ManaBlock>(), ModUtils.ToSecond(10));
				}
			}
		}
	}
	class ManaBlock : ModBuff {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
		}
	}

	public class PerkPlayer : ModPlayer {
		public bool CanGetPerk = false;
		public int PerkrerollAmount = 1;
		private byte perk_Reroll = 1;
		public void Modify_RerollCount(byte amount, bool? negative = false) {
			short simulate = perk_Reroll;
			if (negative == null) {
				simulate = amount;
			}
			else if ((bool)negative) {
				simulate -= amount;
			}
			else {
				simulate += amount;
			}
			if (simulate < byte.MinValue) {
				perk_Reroll = byte.MinValue;
			}
			else if (simulate > byte.MaxValue) {
				perk_Reroll = byte.MaxValue;
			}
			else {
				perk_Reroll = (byte)simulate;
			}

		}
		public byte Reroll => perk_Reroll;
		/// <summary>
		/// Keys : Perk type<br/>
		/// Values : Stack value
		/// </summary>
		public Dictionary<int, int> perks = new Dictionary<int, int>();

		public bool perk_PotionExpert = false;
		public bool perk_PotionCleanse = false;
		public bool perk_AlchemistPotion = false;
		public bool perk_ImprovedPotion = false;
		public bool PotionExpert_perk_CanConsume = false;
		public bool perk_ScatterShot = false;
		public bool perk_EssenceExtraction = false;
		public override void Initialize() {
			perks = new Dictionary<int, int>();
			PerkrerollAmount = 1;
		}
		public int RerollAmount() {
			if (perks.ContainsKey(Perk.GetPerkType<BlessingOfPerk>())) {
				return PerkrerollAmount + perks[Perk.GetPerkType<BlessingOfPerk>()];
			}
			return PerkrerollAmount;
		}
		public bool HasPerk<T>() where T : Perk => perks.ContainsKey(Perk.GetPerkType<T>());
		public override void PostItemCheck() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnUseItem(Player, Player.HeldItem);
			}
		}
		public override bool CanUseItem(Item item) {
			if (item.buffType == BuffID.ManaSickness && Player.HasBuff(ModContent.BuffType<ManaBlock>())) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void ResetEffects() {
			perk_PotionExpert = false;
			perk_PotionCleanse = false;
			perk_AlchemistPotion = false;
			perk_ImprovedPotion = false;
			perk_ScatterShot = false;
			perk_EssenceExtraction = false;
			PerkrerollAmount = 1;
			PerkrerollAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + RerollAmount();
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ResetEffect(Player);
			}
		}
		public override void PostUpdateEquips() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).UpdateEquip(Player);
			}
		}
		public override void PostUpdate() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Update(Player);
			}
		}
		public override void PostUpdateRunSpeeds() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).PostUpdateRun(Player);
			}
		}
		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyShootStat(Player, item, ref position, ref velocity, ref type, ref damage, ref knockback);
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Shoot(Player, item, source, position, velocity, type, damage, knockback);
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public override void OnMissingMana(Item item, int neededMana) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnMissingMana(Player, item, neededMana);
			}
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyMaxStats(Player, ref health, ref mana);
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyCriticalStrikeChance(Player, item, ref crit);
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyItemScale(Player, item, ref scale);
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyDamage(Player, item, ref damage);
			}
		}
		public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyKnockBack(Player, item, ref knockback);
			}
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
			}
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
			}
		}
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByNPC(Player, npc, ref modifiers);
			}
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByProjectile(Player, proj, ref modifiers);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithItem(Player, item, target, hit, damageDone);
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByNPC(Player, npc, hurtInfo);
			}
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByProjectile(Player, proj, hurtInfo);
			}
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyManaCost(Player, item, ref reduce, ref mult);
			}
		}
		public override float UseSpeedMultiplier(Item item) {
			float useSpeed = 1;
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyUseSpeed(Player, item, ref useSpeed);
			}
			return useSpeed;
		}

		public override bool FreeDodge(Player.HurtInfo info) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).FreeDodge(Player, info)) {
					return true;
				}
			}
			return base.FreeDodge(info);
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).PreKill(Player)) {
					return false;
				}
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
		}
		public override bool OnPickup(Item item) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnPickUp(Player, item);
			}
			return base.OnPickup(item);
		}
		public override void SaveData(TagCompound tag) {
			tag["PlayerPerks"] = perks.Keys.ToList();
			tag["PlayerPerkStack"] = perks.Values.ToList();
			tag["perk_Reroll"] = perk_Reroll;
		}
		public override void LoadData(TagCompound tag) {
			var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
			var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
			perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

			int count = perks.Count;
			for (int i = count - 1; i >= 0; i--) {
				if (ModPerkLoader.GetPerk(perks.Keys.ElementAt(i)) == null) {
					perks.Remove(perks.Keys.ElementAt(i));
				}
			}
			if (tag.TryGet("perk_Reroll", out byte va)) {
				perk_Reroll = va;
			}
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			var packet = Mod.GetPacket();
			packet.Write((byte)Roguelike.MessageType.Perk);
			packet.Write((byte)Player.whoAmI);
			packet.Write(perks.Keys.Count);
			foreach (int item in perks.Keys) {
				packet.Write(item);
				packet.Write(perks[item]);
			}
			packet.Write(perk_Reroll);
			packet.Send(toWho, fromWho);
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			perks.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				perks.Add(reader.ReadInt32(), reader.ReadInt32());
			perk_Reroll = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			var clone = (PerkPlayer)targetCopy;
			clone.perks = perks;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			var clone = (PerkPlayer)clientPlayer;
			if (perks != clone.perks) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
	public enum PerkCategory : byte {
		None,
		Starter,
		WeaponUpgrade,
		ArtifactExclusive
	}
	public abstract class Perk : ModType {
		public string DisplayName => ModUtils.LocalizationText("ModPerk", $"{Name}.DisplayName");
		public string Description => ModUtils.LocalizationText("ModPerk", $"{Name}.Description");
		public string DescriptionIndex(int index) => ModUtils.LocalizationText("ModPerk", $"{Name}.Description{index}");
		public bool CanBeStack = false;
		/// <summary>
		/// This will get the value from Mod Perk player itself<br/>
		/// it is recommend to use this rather than get it yourself cause what it doing is pretty much the same<br/>
		/// This is prefer over the previous method as this do not update and isntead just get it from the source<br/>
		/// In whatever case it is, it is highly recommend to not cached it cause the performance increases is very minimal<br/>
		/// </summary>
		public int StackAmount(Player player) {
			if (player.TryGetModPlayer(out PerkPlayer perkplayer)) {
				if (perkplayer.perks.ContainsKey(Type))
					return perkplayer.perks[Type];
			}
			return 0;
		}
		/// <summary>
		/// This is where you set limit to amount of stack should a perk have<br/>
		/// <see cref="StackAmount"/> will always start at 0 and increase by 1 ( regardless if <see cref="CanBeStack"/> true or false )<br/>
		/// The next time this perk get choosen, it will increase by 1<br/>
		/// The perk will no longer show up if the stack amount reach the limit, for more info see <see cref="PerkUIState.ActivateNormalPerkUI(PerkPlayer, Player)"/><br/>
		/// If you are modifying tooltip base on <see cref="StackAmount"/> then you should substract stack amount by 1
		/// </summary>
		public int StackLimit = 1;
		/// <summary>
		/// Please set this texture string as if you are setting <see cref="ModItem.Texture"/>
		/// </summary>
		public string textureString = null;
		public string Tooltip = null;
		/// <summary>
		/// This will prevent from perk being able to be choose
		/// </summary>
		public bool CanBeChoosen = true;
		public int Type { get; private set; }
		public List<PerkCategory> list_category = new() { };
		protected sealed override void Register() {
			Type = ModPerkLoader.Register(this);
		}
		public static int GetPerkType<T>() where T : Perk {
			return ModContent.GetInstance<T>().Type;
		}
		public string PerkNameToolTip => ModifyName() + "\n" + ModifyToolTip();
		/// <summary>
		/// If you are using <see cref="StackAmount(Player)"/> check for tooltip, always set it back at lest by 1
		/// </summary>
		/// <returns></returns>
		public virtual string ModifyToolTip() {
			if (Description != null)
				return Description;
			return Tooltip;
		}
		public virtual string ModifyName() {
			return PerkName();
		}
		public string PerkName() {
			if (DisplayName != null)
				return DisplayName;
			string Name = ModPerkLoader.GetPerk(Type).Name;
			for (int i = Name.Length - 1; i > 0; i--) {
				if (char.IsUpper(Name[i])) {
					Name = string.Concat(Name.AsSpan(0, i), " ", Name.AsSpan(i));
				}
			}
			return Name;
		}
		public sealed override void Unload() {
			base.Unload();
			textureString = null;
			Tooltip = null;
		}
		public Perk() {
			SetDefaults();
			if (CanBeStack)
				Tooltip += "\n( Can be stack ! )";
		}
		/// <summary>
		/// This act different to <see cref="CanBeChoosen"/><br/>
		/// if this is set to false then it won't be add into perk pool, otherwise if <see cref="CanBeChoosen"/> is false and this is true then it still won't be add<br/>
		/// This only run on client side so it is recommend to uses <see cref="Main.LocalPlayer"/> to check player condition
		/// </summary>
		/// <returns></returns>
		public virtual bool SelectChoosing() => true;
		public virtual void SetDefaults() { }
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void OnUseItem(Player player, Item item) { }
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void PostUpdateRun(Player player) { }
		public virtual void UpdateEquip(Player player) { }
		public virtual void ResetEffect(Player player) { }
		public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
		public virtual void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) { }
		public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
		public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
		public virtual void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) { }
		public virtual void ModifyItemScale(Player player, Item item, ref float scale) { }
		public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
		public virtual void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) { }
		public virtual void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) { }
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
		public virtual void OnChoose(Player player) { }
		public virtual bool FreeDodge(Player player, Player.HurtInfo hurtInfo) => false;
		public virtual bool PreKill(Player player) => false;
		public virtual void OnPickUp(Player player, Item item) { }
	}
	public static class ModPerkLoader {
		private static readonly List<Perk> _perks = new();
		public static int TotalCount => _perks.Count;
		public static int Register(Perk perk) {
			ModTypeLookup<Perk>.Register(perk);
			_perks.Add(perk);
			if (perk.list_category.Count < 1) {
				perk.list_category.Add(PerkCategory.None);
			}
			return _perks.Count - 1;
		}
		public static Perk GetPerk(int type) {
			return type >= 0 && type < _perks.Count ? _perks[type] : null;
		}
	}
	internal class PerkUIState : UIState {
		public string Info = "";
		public const short DefaultState = 0;
		public const short StarterPerkState = 1;
		public short StateofState = 0;
		public Roguelike_UIImageButton reroll = null;

		public Roguelike_UIPanel panel_perkSelection = null;
		public PerkUIImageButton[] Arr_Perkbtn = null;
		public Roguelike_UIPanel panel_perkContainer = null;
		public Roguelike_UIPanel panel_rerollPerkContainer = null;
		public Roguelike_UITextPanel textpanel_rerollInfo = null;
		public override void OnInitialize() {
			panel_perkSelection = new();
			panel_perkSelection.UISetWidthHeight(400, 210);
			panel_perkSelection.HAlign = .5f;
			panel_perkSelection.VAlign = .5f;
			Append(panel_perkSelection);

			panel_perkContainer = new();
			panel_perkContainer.UISetWidthHeight(400, 100);
			panel_perkContainer.HAlign = .5f;
			panel_perkContainer.VAlign = 0;
			panel_perkSelection.Append(panel_perkContainer);

			var asset = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);

			Arr_Perkbtn = new PerkUIImageButton[5];
			for (int i = 0; i < Arr_Perkbtn.Length; i++) {
				var perkBTN = Arr_Perkbtn[i];
				perkBTN = new PerkUIImageButton(asset);
				perkBTN.ChangePerkType(Perk.GetPerkType<SuppliesDrop>());
				perkBTN.HAlign = MathHelper.Lerp(.05f, .95f, i / 4f);
				perkBTN.VAlign = .5f;
				perkBTN.UISetWidthHeight(52, 52);
				Arr_Perkbtn[i] = perkBTN;
				panel_perkContainer.Append(Arr_Perkbtn[i]);
			}

			panel_rerollPerkContainer = new();
			panel_rerollPerkContainer.Height.Pixels = 80;
			panel_rerollPerkContainer.Width.Percent = 1f;
			panel_rerollPerkContainer.HAlign = 0f;
			panel_rerollPerkContainer.VAlign = 1f;
			panel_perkSelection.Append(panel_rerollPerkContainer);

			textpanel_rerollInfo = new("Reroll remain: 0");
			textpanel_rerollInfo.UISetWidthHeight(200, 60);
			textpanel_rerollInfo.HAlign = 1f;
			textpanel_rerollInfo.VAlign = .5f;
			textpanel_rerollInfo.OnUpdate += Textpanel_rerollInfo_OnUpdate;
			panel_rerollPerkContainer.Append(textpanel_rerollInfo);

			reroll = new(asset);
			reroll.OnLeftClick += Reroll_OnLeftClick;
			reroll.OnUpdate += Reroll_OnUpdate;
			reroll.UISetWidthHeight(52, 52);
			reroll.HAlign = 0;
			reroll.VAlign = 1f;
			reroll.SetPostTex(ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<PerkUIState>("perkReroll")));
			panel_rerollPerkContainer.Append(reroll);
		}

		private void Textpanel_rerollInfo_OnUpdate(UIElement affectedElement) {
			textpanel_rerollInfo.SetText($"Reroll remain: {Main.LocalPlayer.GetModPlayer<PerkPlayer>().Reroll}");
		}

		private void PerkSelectionNormal(PerkPlayer modplayer, Player player) {
			var listOfPerk = new List<int>();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (modplayer.perks.ContainsKey(i)) {
					if (!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0
						|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
					continue;
				}
				if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
					continue;
				}
				listOfPerk.Add(i);
			}
			AssignPerkIDtoArrBtn(listOfPerk);
		}
		private void PerkSelectionStarter(PerkPlayer modplayer, Player player) {
			List<int> starterPerk = [.. PerkModSystem.StarterPerkType];
			foreach (var item in PerkModSystem.StarterPerkType) {
				if (modplayer.perks.ContainsKey(item)) {
					if (!ModPerkLoader.GetPerk(item).CanBeStack && modplayer.perks[item] > 0
						|| modplayer.perks[item] >= ModPerkLoader.GetPerk(item).StackLimit) {
						starterPerk.Remove(item);
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(item).SelectChoosing()) {
					starterPerk.Remove(item);
					continue;
				}
				if (!ModPerkLoader.GetPerk(item).CanBeChoosen) {
					starterPerk.Remove(item);
				}
			}
			AssignPerkIDtoArrBtn(starterPerk);
		}
		private void AssignPerkIDtoArrBtn(List<int> list_perk) {
			int[] Arr_PerkNumber = new int[5];
			for (int i = 0; i < Arr_PerkNumber.Length; i++) {
				if (Arr_PerkNumber[i] == 0) {
					if (list_perk.Count <= 1) {
						Arr_PerkNumber[i] = Perk.GetPerkType<SuppliesDrop>();
						Arr_Perkbtn[i].ChangePerkType(Perk.GetPerkType<SuppliesDrop>());
						Arr_Perkbtn[i].Info = Info;
						continue;
					}
					int perkID = Main.rand.Next(list_perk);
					if (Arr_PerkNumber.Contains(perkID)) {
						i--;
						continue;
					}
					else {
						Arr_PerkNumber[i] = perkID;
						Arr_Perkbtn[i].ChangePerkType(perkID);
						Arr_Perkbtn[i].Info = Info;
					}
				}
			}
		}
		private void Reroll_OnUpdate(UIElement affectedElement) {
			if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().Reroll <= 0) {
				reroll.UnselectAble = true;
			}
			if (affectedElement.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (affectedElement.IsMouseHovering) {
				Main.instance.MouseText("Reroll Perk !");
			}
		}
		private void Reroll_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 1 });
			var listOfPerk = new List<int>();
			var player = Main.LocalPlayer;
			player.TryGetModPlayer(out PerkPlayer modplayer);
			if (StateofState == DefaultState) {
				for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
					if (modplayer.perks.ContainsKey(i)) {
						if (!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0
							|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
							continue;
						}
					}
					if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
						continue;
					}
					if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
						continue;
					}
					listOfPerk.Add(i);
				}
			}
			else if (StateofState == StarterPerkState) {
				listOfPerk = [.. PerkModSystem.StarterPerkType];
			}
			foreach (var item in Arr_Perkbtn) {
				if (listOfPerk.Contains(item.perkType)) {
					listOfPerk.Remove(item.perkType);
				}
				if (listOfPerk.Count < 1) {
					item.ChangePerkType(Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() }));
				}
				else {
					int perkChoosen = Main.rand.Next(listOfPerk);
					item.ChangePerkType(perkChoosen);
					listOfPerk.Remove(perkChoosen);
				}
			}
			modplayer.Modify_RerollCount(1, true);
		}
		public override void OnActivate() {
			var player = Main.LocalPlayer;
			if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
				modplayer.Modify_RerollCount((byte)modplayer.RerollAmount(), null);
				if (StateofState == DefaultState) {
					reroll.UnselectAble = false;
					PerkSelectionNormal(modplayer, player);
				}
				if (StateofState == StarterPerkState) {
					reroll.UnselectAble = false;
					PerkSelectionStarter(modplayer, player);
				}
			}
		}
	}
	//Do all the check in UI state since that is where the perk actually get create and choose
	class PerkUIImageButton : Roguelike_UIImageButton {
		public int perkType;
		public string Info = "";
		private Asset<Texture2D> texture;
		private Asset<Texture2D> ahhlookingassdefaultbgsperktexture = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);
		public PerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
			this.texture = texture;
		}
		public void ChangePerkType(int type) {
			perkType = type;
			var perk = ModPerkLoader.GetPerk(perkType);
			if (perk != null && perk.textureString != null) {
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(perkType).textureString);
			}
			else {
				texture = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);
			}
			SetImage(texture);
			this.UISetWidthHeight(52, 52);
		}
		public override void LeftClick(UIMouseEvent evt) {
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = -1 });
			UniversalSystem.AddPerk(perkType);
			if (Info == "Glitch") {
				var perk = ModPerkLoader.GetPerk(perkType);
				int stack = 0;
				if (perk.CanBeStack) {
					stack = Main.LocalPlayer.GetModPlayer<PerkPlayer>().perks[perkType];
				}
				int length = Math.Clamp(perk.StackLimit - stack, 0, 999999);
				for (int i = 0; i < length; i++) {
					UniversalSystem.AddPerk(perkType);
				}
			}
		}
		public override void UpdateOuter(GameTime gametime) {
			if (ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			else {
				if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
					Main.instance.MouseText("");
				}
			}
			if (IsMouseHovering && Switch == 0) {
				Switch = ModUtils.Safe_SwitchValue(Switch, 100);
			}
			if (Switch != 0) {
				Switch = ModUtils.Safe_SwitchValue(Switch, 100, extraspeed: 1);
			}
		}
		int Switch = 0;
		public override void DrawImage(SpriteBatch spriteBatch) {
			base.DrawImage(spriteBatch);
			if (IsMouseHovering && ModPerkLoader.GetPerk(perkType) != null) {
				UICommon.TooltipMouseText(ModPerkLoader.GetPerk(perkType).DisplayName + "\n" + ModPerkLoader.GetPerk(perkType).ModifyToolTip());
			}
			if (Info == "Glitch") {
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Red * .5f);
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Blue * .5f);
			}
			spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position(), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
			if (Switch != 0) {
				float alpha = (100 - Switch) * 0.01f;
				float size = 1 + Switch * .01f * .75f;
				var origin = ahhlookingassdefaultbgsperktexture.Value.Size() * .5f;
				var adjustment = origin - origin * size;
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, GetInnerDimensions().Position() + adjustment, null, Color.White * alpha, 0, Vector2.Zero, size, SpriteEffects.None, 0f);
			}
			Vector2 size1 = texture.Size();
			Vector2 size2 = ahhlookingassdefaultbgsperktexture.Size();
			if (size1.X <= size2.X && size1.Y <= size2.Y) {
				spriteBatch.Draw(texture.Value, GetInnerDimensions().Position() + ahhlookingassdefaultbgsperktexture.Size() * .5f, null, Color.White, 0, texture.Size() * .5f, 1f, SpriteEffects.None, 0);
			}
			else {
				spriteBatch.Draw(texture.Value, GetInnerDimensions().Position() + ahhlookingassdefaultbgsperktexture.Size() * .5f, null, Color.White, 0, texture.Size() * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size1, size2, .8f), SpriteEffects.None, 0);
			}
		}
	}
}
