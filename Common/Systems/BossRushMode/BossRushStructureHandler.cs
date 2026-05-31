using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Mode.BossRushMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Toggle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Roguelike.Common.Systems.BossRushMode;
public class BossRushStructureHandler : ModSystem {
	public override void Load() {
		On_NPC.EncourageDespawn += On_NPC_EncourageDespawn;
	}

	private void On_NPC_EncourageDespawn(On_NPC.orig_EncourageDespawn orig, NPC self, int despawnTime) {
		if (!Active || !RoguelikeWorldProperty.BossRushWorld) {
			return;
		}
		orig(self, despawnTime);
	}

	public HashSet<Point> MobsSpawningPos() {
		return new HashSet<Point>() {
		new Point(24,47),
		new Point(24,22),
		new Point(24,72),
		new Point(24,97),
		new Point(195,97),
		new Point(195,72),
		new Point(195,47),
		new Point(195,22)
		};
	}
	public Rectangle Rect_BossRushStructure() => ModContent.GetInstance<BossRushWorldGen>().BossRushStructure;
	Stopwatch _timer = new Stopwatch();
	public TimeSpan Get_Timer => _timer.Elapsed;

	bool Initialize = true;
	int SpawnTime = 0;
	int SpawnTimeLimit = 600;
	int SpawnAmount = 0;
	public bool NGplus = false;
	public int CurrentBadModifier = -1;
	public void Start_BossRush(bool NGplus = false) {
		Active = true;
		_timer.Start();
		BossList = [.. Arr_BossID];
		this.NGplus = NGplus;
	}
	public int[] NPCspawnPool = [
		NPCID.DemonEye,
		NPCID.Hornet,
		NPCID.FireImp,
		NPCID.BlueSlime,
		NPCID.SpikedIceSlime,
		NPCID.SpikedJungleSlime,
		NPCID.Crimera,
		NPCID.Corruptor,
		NPCID.Harpy,
		NPCID.GiantShelly,
		NPCID.GreekSkeleton,
		NPCID.GraniteFlyer,
		NPCID.GraniteGolem,
		NPCID.GiantWalkingAntlion,
		NPCID.GiantFlyingAntlion,
		NPCID.EaterofSouls,
		NPCID.GiantBat,
		NPCID.IceBat,
		NPCID.Demon,
		NPCID.CultistArcherWhite,
		NPCID.IceElemental,
		NPCID.IcyMerman,
		NPCID.Tim,
		];
	public int[] MiniBossspawnPool = [
		NPCID.RuneWizard,
		NPCID.Paladin,
		NPCID.RockGolem,
		NPCID.Medusa,
		NPCID.BigMimicCrimson,
		NPCID.BigMimicCorruption,
		NPCID.BigMimicHallow,
		NPCID.BigMimicJungle,
		NPCID.BloodSquid,
		NPCID.RainbowSlime,
		NPCID.GoblinSummoner,
		];
	public bool Active { get; private set; } = false;
	public readonly int[] Arr_BossID = [
		NPCID.KingSlime,
		NPCID.EyeofCthulhu,
		NPCID.EaterofWorldsHead,
		NPCID.BrainofCthulhu,
		NPCID.QueenBee,
		NPCID.Deerclops,
		NPCID.SkeletronHead,
		NPCID.QueenSlimeBoss,
		NPCID.Retinazer,
		NPCID.SkeletronPrime,
		NPCID.TheDestroyer,
		NPCID.Plantera,
		NPCID.Golem,
		NPCID.DukeFishron,
		NPCID.HallowBoss,
		NPCID.CultistBoss,
	];
	List<int> BossList = new();
	public int BossSpawnCD = 0;
	public override void PostUpdateEverything() {
		if (!Main.LocalPlayer.active || Main.LocalPlayer.dead || !Active || !RoguelikeWorldProperty.BossRushWorld) {
			return;
		}
		if (BossSpawnCD > 0 && BossSpawnCD <= ModUtils.ToSecond(10))
			if (BossSpawnCD % 60 == 0) {
				Main.NewText($"{BossSpawnCD / 60}", Color.Red);
			}
		if (Initialize) {
			if (BossList == null || BossList.Count < 1) {
				BossList = [.. Arr_BossID];
			}
			Initialize = false;
		}
		//SpawnTimeLimit = Math.Clamp(600 - SpawnAmount / 100, 60, 900);
		//if (++SpawnTime >= SpawnTimeLimit) {
		//	SpawnTime = 0;
		//	SpawnAmount++;
		//	var zone = Rect_BossRushStructure();
		//	var pos = (zone.Location + Main.rand.NextFromHashSet(MobsSpawningPos())).ToWorldCoordinates();
		//	var npc = NPC.NewNPCDirect(new EntitySource_SpawnNPC(), (int)pos.X, (int)pos.Y, Main.rand.Next(NPCspawnPool));
		//	npc.GetGlobalNPC<RoguelikeGlobalNPC>().CanDenyYouFromLoot = true;
		//	npc.timeLeft = 9999999;
		//}
		if (!Setting_SpawnOnPlayerCommand) {
			SpawnBoss();
		}
	}
	public void SpawnBoss(bool ForcedSpawn = false) {
		if (!ModContent.GetInstance<BossRushWorldGen>().IsABossAlive) {
			if (Setting_BossRushExtra && !Setting_SpawnOnPlayerCommand) {
				var modifier = Main.rand.Next(BossRushModifierLoader.modifier_good);
				if (modifier != null) {
					modifier.OnChoose();
				}
				modifier = Main.rand.Next(BossRushModifierLoader.modifier_bad);
				if (modifier != null) {
					modifier.OnChoose();
				}
				Main.LocalPlayer.GetModPlayer<BossRushModifierPlayer>().ResetAllModifier();
			}
			if (--BossSpawnCD > 0 && !ForcedSpawn) {
				return;
			}
			if (BossList == null || BossList.Count < 1) {
				if (!NGplus) {
					NGplus = true;
					Active = false;
					Main.NewText("Congratulation, you beaten boss rush mode");
					Main.NewText("If you want to continue to see how far you can push yourself, activate the boss rush item again");
					_timer.Stop();
					return;
				}
				else {
					BossList = [.. Arr_BossID];
				}
			}
			int type;
			if (Setting_FightBossInProgression) {
				type = BossList[0];
			}
			else {
				type = Main.rand.Next(BossList);
			}
			BossList.Remove(type);
			BossSpawnCD = ModUtils.ToSecond(30);
			var pos = Rect_BossRushStructure().Center.ToWorldCoordinates();
			var self = Main.LocalPlayer;
			int num = ModUtils.SpawnBoss((int)pos.X, (int)pos.Y, type, self.whoAmI);
			if (type == NPCID.Spazmatism) {
				NPC.SpawnBoss((int)pos.X, (int)pos.Y, NPCID.Retinazer, self.whoAmI);
			}
			else if (type == NPCID.Retinazer) {
				NPC.SpawnBoss((int)pos.X, (int)pos.Y, NPCID.Spazmatism, self.whoAmI);
			}
			if (type == NPCID.DukeFishron) {
				self.ZoneBeach = true;
			}
			if (type == NPCID.BrainofCthulhu) {
				self.ZoneCrimson = true;
			}
			if (type == NPCID.EaterofWorldsBody || type == NPCID.EaterofWorldsHead || type == NPCID.EaterofWorldsTail) {
				self.ZoneCorrupt = true;
			}
			if (type == NPCID.SkeletronHead || type == NPCID.SkeletronPrime || type == NPCID.Retinazer || type == NPCID.Spazmatism || type == NPCID.HallowBoss) {
				Main.dayTime = false;
				Main.time = Main.nightLength;
			}
			if (type == NPCID.QueenBee || type == NPCID.Plantera) {
				self.ZoneJungle = true;
			}
			if (ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra) {
				if (ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier5>()) {
					int amount = Main.rand.Next(20, 26);
					for (int i = 0; i < amount; i++) {
						NPC enemy = NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), pos, Main.rand.Next(NPCspawnPool));
						enemy.GetGlobalNPC<RoguelikeGlobalNPC>().DamageReduction += .95f;
					}
				}
				if (ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier6>()) {
					NPC enemy = NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), pos, Main.rand.Next(MiniBossspawnPool));
					enemy.GetGlobalNPC<RoguelikeGlobalNPC>().DamageReduction += .95f;
				}
				if (ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier8>()) {
					if (num == -1 || num == 200) {
						return;
					}
					NPC enemy = NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), pos, type);
					enemy.GetGlobalNPC<RoguelikeGlobalNPC>().IsAGhostEnemy = true;
					enemy.GetGlobalNPC<RoguelikeGlobalNPC>().BelongToWho = num;
					enemy.GetGlobalNPC<RoguelikeGlobalNPC>().CanDenyYouFromLoot = true;
				}
			}
		}
	}
	public override void PreSaveAndQuit() {
		Active = false;
		_timer.Reset();
		_timer.Stop();
		BossList = [.. Arr_BossID];
		CurrentBadModifier = -1;
		if (Main.LocalPlayer.dead) {
			ModContent.GetInstance<UniversalSystem>().Count_BossKill = 0;
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["NGplus"] = NGplus;
		tag["BossList"] = BossList;
		tag["BadModifier"] = CurrentBadModifier;
	}
	public override void LoadWorldData(TagCompound tag) {
		NGplus = tag.Get<bool>("NGplus");
		BossList = tag.Get<List<int>>("BossList");
		CurrentBadModifier = tag.Get<int>("BadModifier");
	}
	public static bool Setting_FightBossInProgression => ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Setting_FightBossInProgression;
	public static bool Setting_SpawnOnPlayerCommand => ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Setting_SpawnOnPlayerCommand;
	public static bool Setting_BossRushExtra => ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra;

}
public class BossRushModeModPlayer : ModPlayer {
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra) {
			if (npc.boss &&
				ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier7>()) {
				modifiers.FinalDamage.Flat += (int)(Player.statLifeMax2 * .25f);
			}
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().IsFromBoss &&
				ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier == BossRushModifier.GetModifierType<BR_BadModifier7>()) {
				modifiers.FinalDamage.Flat += (int)(Player.statLifeMax2 * .25f);
			}
		}
	}
}
public class BossRushModeGlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		if (!RoguelikeWorldProperty.BossRushWorld) {
			return;
		}
		pool.Clear();
	}
	public override void OnKill(NPC npc) {
		if (!RoguelikeWorldProperty.BossRushWorld) {
			return;
		}
		if (ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra) {
			ModContent.GetInstance<BossRushStructureHandler>().CurrentBadModifier = -1;
		}
		if (npc.boss && !BossRushStructureHandler.Setting_SpawnOnPlayerCommand) {
			ModContent.GetInstance<BossRushStructureHandler>().BossSpawnCD = ModUtils.ToSecond(30);
			Main.NewText("A boss have been killed, 30s until a new boss is spawn", Color.Red);
		}
	}
}
internal class BossRushModeActivation : ModItem {
	public override void SetStaticDefaults() {
		Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
		ItemID.Sets.AnimatesAsSoul[Item.type] = true;
	}
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<CursedSkull>();
	public override void SetDefaults() {
		Item.height = 60;
		Item.width = 56;
		Item.value = 0;
		Item.rare = ItemRarityID.Purple;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.scale = .5f;
	}
	public override bool CanUseItem(Player player) {
		return !ModUtils.IsAnyVanillaBossAlive();
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool? UseItem(Player player) {
		if (player.whoAmI == Main.myPlayer) {
			if (ModContent.GetInstance<UniversalSystem>().user2ndInterface.CurrentState != null) {
				return true;
			}
			var handler = ModContent.GetInstance<BossRushStructureHandler>();
			if (ModContent.GetInstance<RogueLikeConfig>().BossRushMode_Extra) {
				if (handler.CurrentBadModifier == -1) {
					ModContent.GetInstance<UniversalSystem>().ActivateBRmodifierUI();
					return true;
				}
			}
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (BossRushStructureHandler.Setting_SpawnOnPlayerCommand && player.ItemAnimationJustStarted) {
				if (!handler.Active) {
					handler.Start_BossRush();
				}
				handler.SpawnBoss(true);
				handler.BossSpawnCD = ModUtils.ToSecond(30);
				return true;
			}
			bool NGplus = handler.NGplus;
			if (ModContent.GetInstance<UniversalSystem>().Count_BossKill > 1) {
				Main.NewText("Restart boss rush !");
				handler.Start_BossRush(NGplus);
			}
			else {
				Main.NewText("Boss rush mode start !");
				handler.Start_BossRush(NGplus);
			}
		}
		return true;
	}
}
public class BossRushModifierUIState : UIState {
	public List<BossRushModifierUIButton> btn_List;
	public UITextPanel<string> textpanel;
	public Roguelike_UIPanel mainpanel;
	public Roguelike_UIPanel panel_Modifier_Container;
	public Roguelike_UIPanel panel_positive_Modifier;
	public Roguelike_UIPanel panel_negative_Modifier;
	public Roguelike_UIText txt_confirmSelection;
	public int PositiveModifier = -1;
	public int NegativeModifier = -1;
	public Roguelike_UIText txt_positiveSelection;
	public Roguelike_UIText txt_negativeSelection;
	public override void OnInitialize() {
		mainpanel = new();
		mainpanel.HAlign = .5f;
		mainpanel.VAlign = .5f;
		mainpanel.UISetWidthHeight(650, 400);
		mainpanel.BorderColor = Color.Black with { A = 0 };
		mainpanel.BackgroundColor = Color.Black with { A = 0 };
		Append(mainpanel);

		panel_Modifier_Container = new Roguelike_UIPanel();
		panel_Modifier_Container.UISetWidthHeight(400, 300);
		panel_Modifier_Container.BackgroundColor = Color.Black with { A = 0 };
		panel_Modifier_Container.BorderColor = Color.Black with { A = 0 };
		panel_Modifier_Container.SetPadding(0);
		panel_Modifier_Container.HAlign = .5f;
		panel_Modifier_Container.VAlign = 1;
		mainpanel.Append(panel_Modifier_Container);

		panel_positive_Modifier = new Roguelike_UIPanel();
		panel_positive_Modifier.UISetWidthHeight(400, 100);
		//panel_positive_Modifier.BorderColor = panel_positive_Modifier.BackgroundColor = Color.Black with { A = 0 };
		panel_Modifier_Container.Append(panel_positive_Modifier);

		panel_negative_Modifier = new Roguelike_UIPanel();
		panel_negative_Modifier.UISetWidthHeight(400, 100);
		//panel_negative_Modifier.BorderColor = panel_negative_Modifier.BackgroundColor = Color.Black with { A = 0 };
		panel_negative_Modifier.VAlign = 1;
		panel_Modifier_Container.Append(panel_negative_Modifier);

		textpanel = new UITextPanel<string>("Choose your modifier");
		textpanel.HAlign = .5f;
		textpanel.UISetWidthHeight(150, 53);
		mainpanel.Append(textpanel);
		btn_List = new List<BossRushModifierUIButton>();

		txt_confirmSelection = new("confirm selection ?");
		txt_confirmSelection.HAlign = 1;
		txt_confirmSelection.VAlign = .5f;
		txt_confirmSelection.Hide = true;
		txt_confirmSelection.OnLeftClick += Txt_confirmSelection_OnLeftClick;
		panel_Modifier_Container.Append(txt_confirmSelection);


		txt_positiveSelection = new("");
		txt_positiveSelection.VAlign = 1;
		panel_positive_Modifier.Append(txt_positiveSelection);


		txt_negativeSelection = new("");
		txt_negativeSelection.HAlign = 1;
		txt_negativeSelection.VAlign = 1f;
		panel_negative_Modifier.Append(txt_negativeSelection);
	}

	private void Txt_confirmSelection_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		var modifier = BossRushModifierLoader.GetModifier(PositiveModifier);
		if (modifier != null) {
			modifier.OnChoose();
		}
		modifier = BossRushModifierLoader.GetModifier(NegativeModifier);
		if (modifier != null) {
			modifier.OnChoose();
		}
		Main.LocalPlayer.GetModPlayer<BossRushModifierPlayer>().ResetAllModifier();
	}

	public override void OnActivate() {
		int optionsAmount = 5;
		PositiveModifier = -1;
		NegativeModifier = -1;
		txt_positiveSelection.SetText("");
		txt_negativeSelection.SetText("");
		btn_List.Clear();
		List<BossRushModifier> positive_modifier = new(BossRushModifierLoader.modifier_good);
		for (int i = 0; i < optionsAmount; i++) {
			var modifier = Main.rand.Next(positive_modifier);
			positive_modifier.Remove(modifier);
			float Hvalue = MathHelper.Lerp(0, 1, i / (float)(optionsAmount - 1));
			BossRushModifierUIButton btn = new BossRushModifierUIButton(
				TextureAssets.InventoryBack10, modifier.Type);
			btn.HAlign = Hvalue;
			btn.SetVisibility(.6f, 1);
			btn_List.Add(btn);
			panel_positive_Modifier.Append(btn);
		}
		List<BossRushModifier> negative_modifier = new(BossRushModifierLoader.modifier_bad);
		for (int i = 0; i < optionsAmount; i++) {
			var modifier = Main.rand.Next(negative_modifier);
			negative_modifier.Remove(modifier);
			float Hvalue = MathHelper.Lerp(0, 1, i / (float)(optionsAmount - 1));
			BossRushModifierUIButton btn = new BossRushModifierUIButton(
				TextureAssets.InventoryBack19, modifier.Type);
			btn.HAlign = Hvalue;
			btn.SetVisibility(.6f, 1);
			btn_List.Add(btn);
			panel_negative_Modifier.Append(btn);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (PositiveModifier != -1 && NegativeModifier != -1) {
			txt_confirmSelection.Hide = false;
		}
		else {
			txt_confirmSelection.Hide = true;
		}
	}
}
public class BossRushModifierUIButton : UIImageButton {
	int type = 0;
	public BossRushModifierUIButton(Asset<Texture2D> texture, int type) : base(texture) {
		this.type = type;
	}
	public override void LeftClick(UIMouseEvent evt) {
		var modifier = BossRushModifierLoader.GetModifier(type);
		if (modifier != null) {
			if (modifier.PositiveModifier) {
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.PositiveModifier = modifier.Type;
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.txt_positiveSelection.SetText(modifier.Description);
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.txt_positiveSelection.TextColor = Color.Green;
			}
			else {
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.NegativeModifier = modifier.Type;
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.txt_negativeSelection.SetText(modifier.Description);
				ModContent.GetInstance<UniversalSystem>().UI_BRmodifier.txt_negativeSelection.TextColor = Color.Red;
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			var modifier = BossRushModifierLoader.GetModifier(type);
			if (modifier != null) {
				Main.instance.MouseText(modifier.Description);
			}
		}
	}
}
