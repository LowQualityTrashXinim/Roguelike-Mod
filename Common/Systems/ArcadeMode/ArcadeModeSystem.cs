using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArcadeMode;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ShopSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Toggle;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Roguelike.Common.Systems.ArcaneMode;
public partial class ArcadeMode_System : ModSystem {
	public override void Load() {
		On_NPC.EncourageDespawn += On_NPC_EncourageDespawn;
	}
	private void On_NPC_EncourageDespawn(On_NPC.orig_EncourageDespawn orig, NPC self, int despawnTime) {
		if (!Active || !ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			return;
		}
		orig(self, despawnTime);
	}

	public Rectangle Rect_Structure() {
		Rectangle rect = ModContent.GetInstance<ArcadeModeStructure>().ArcadeStructure;
		return new Rectangle(rect.X + 13, rect.Y + 13, rect.Width - 30, rect.Height - 30);
	}
	/// <summary>
	/// This always have less width than the actual Structure itself
	/// </summary>
	/// <returns></returns>
	public Rectangle Rect_SpawnZone() {
		Rectangle structure = Rect_Structure();
		return new Rectangle(structure.X + 15, structure.Y + 15, structure.Width - 30, structure.Height - 30);
	}
	public Vector2 Get_SpawnNPCLocationRandomize() {
		Rectangle structure = Rect_Structure();
		Rectangle zone = Rect_SpawnZone();
		int maxX = Math.Max(structure.X, zone.X);
		int minX = Math.Min(structure.X, zone.X);

		int maxY = Math.Max(structure.Y, zone.Y);
		int minY = Math.Min(structure.Y, zone.Y);

		Rectangle rect = new Rectangle(
			Main.rand.Next(minX, maxX),
			Main.rand.Next(minY, maxY),
			zone.Width + (structure.Width - zone.Width) / 2, zone.Height + (structure.Height - zone.Height) / 2);
		Vector2 result = Main.rand.NextVector2RectangleEdge(rect);

		return result.ToWorldCoordinates();
	}
	public int Point = 0;
	public bool Active { get; private set; } = false;
	public int NGplus = 0;
	public void Start_ArcadeMode(int NewGameplus = 0) {
		Active = true;
		NGplus = NewGameplus;
	}
	ArcadeMode_Wave wave = null;
	public int[] Wave_Information() {
		if (wave == null) {
			int waveAhead = CurrentWave + 1;
			ArcadeMode_Wave cacheInfo = ArcadeMode_Database.Get_Wave(waveAhead);
			return [.. cacheInfo.NPCpool, .. cacheInfo.RareNPCpool, cacheInfo.BossNPC];
		}
		return [.. wave.NPCpool, .. wave.RareNPCpool, wave.BossNPC];
	}
	public int CurrentWave { get; private set; } = 0;
	int Freqency = 0;
	int Timer = 0;
	public string Get_Time() {
		if (wave == null) {
			return "0s";
		}
		else {
			return $"{MathF.Round((wave.SpawnNPC_Duration - Timer) / 60f, 2)}s";
		}
	}
	public override void PostUpdateEverything() {
		//Rectangle debugRect = Rect_SpawnZone();
		//for (int i = 0; i < 100; i++) {
		//	Vector2 Center = Main.rand.NextVector2RectangleEdge(debugRect).ToWorldCoordinates();
		//	Dust dust = Dust.NewDustDirect(Center, 0, 0, DustID.GemTopaz);
		//	dust.scale += Main.rand.NextFloat(-.1f, .2f);
		//	dust.noGravity = true;
		//}
		//debugRect = Rect_Structure();
		//for (int i = 0; i < 100; i++) {
		//	Vector2 Center = Main.rand.NextVector2RectangleEdge(debugRect).ToWorldCoordinates();
		//	Dust dust = Dust.NewDustDirect(Center, 0, 0, DustID.GemRuby);
		//	dust.scale += Main.rand.NextFloat(-.1f, .2f);
		//	dust.noGravity = true;
		//}
		//for (int i = 0; i < 100; i++) {
		//	Vector2 Center = Get_SpawnNPCLocationRandomize();
		//	Dust dust = Dust.NewDustDirect(Center, 0, 0, DustID.GemEmerald);
		//	dust.scale += Main.rand.NextFloat(-.1f, .2f);
		//	dust.noGravity = true;
		//}
		if (!Main.LocalPlayer.active || Main.LocalPlayer.dead || !ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			return;
		}
		if (Active) {
			if (wave == null) {
				wave = ArcadeMode_Database.Get_Wave(++CurrentWave);
				Main.NewText($"Start wave : {CurrentWave}");
				if (wave.BossNPC != 0) {
					Player player = Main.LocalPlayer;
					Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<ArcadeMode_SpawnBoss_Projectile>(), 0, 0, player.whoAmI, 0, 0, 600);
				}
				return;
			}
			if (++Timer > wave.SpawnNPC_Duration) {
				Main.NewText($"Wave {CurrentWave} completed !");
				Timer = 0;
				Freqency = 0;
				wave = null;
				Active = false;
				Main.LocalPlayer.QuickSpawnItem(Entity.GetSource_None(), ModContent.ItemType<OpenArcadeShopItem>());
				return;
			}
			if (++Freqency >= wave.SpawnTime_Frequency) {
				int type;
				int count = 0;
				for (int i = 0; i < Main.npc.Length; i++) {
					if (Main.npc[i].active && !Main.npc[i].boss) {
						count++;
					}
				}
				if (count < wave.SpawnNPC_Limit) {
					for (int i = 0; i < wave.SpawnNPC_Amount; i++) {
						if (Main.rand.NextFloat() <= wave.RareValue) {
							type = wave.Get_RareNPCtoSpawn();
						}
						else {
							type = wave.Get_NPCtoSpawn();
						}
						var obj = ModObject.NewModObject(Get_SpawnNPCLocationRandomize(), Vector2.Zero, ModObject.GetModObjectType<ArcadeMode_SpawnNPC_Indicator>());
						if (obj is ArcadeMode_SpawnNPC_Indicator spawn) {
							spawn.NPCtoSpawn = type;
						}
					}
				}
				Freqency = 0;
			}
		}
	}
	public void SpawnBoss() {
		if (!UniversalSystem.AnyVanillaBossAlive && wave != null) {
			int type;
			if (wave.BossNPC == 0) {
				return;
			}
			type = wave.BossNPC;
			var pos = Rect_Structure().Center.ToWorldCoordinates();
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
		}
	}
	public override void PreSaveAndQuit() {
		Active = false;
		if (Main.LocalPlayer.dead) {
			CurrentWave = 0;
			ModContent.GetInstance<UniversalSystem>().Count_BossKill = 0;
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["Arcade_NGplus"] = NGplus;
		tag["Arcade_CurrentWave"] = CurrentWave;
	}
	public override void LoadWorldData(TagCompound tag) {
		NGplus = tag.Get<int>("Arcade_NGplus");
		CurrentWave = tag.Get<int>("Arcade_CurrentWave");
	}
}
public class ArcadeMode_GlobalNPC : GlobalNPC {
	public override bool PreKill(NPC npc) {
		if (!ModContent.GetInstance<RoguelikeWorldProperty>().ArcadeMode) {
			return base.PreKill(npc);
		}
		int basePointGrant = 0;
		switch (npc.type) {
			case NPCID.BlueSlime:
				basePointGrant = 10;
				break;
			case NPCID.GreenSlime:
				basePointGrant = 5;
				break;
			case NPCID.RedSlime:
				basePointGrant = 11;
				break;
			case NPCID.YellowSlime:
				basePointGrant = 12;
				break;
			case NPCID.PurpleSlime:
				basePointGrant = 13;
				break;
			case NPCID.SlimeSpiked:
				basePointGrant = 15;
				break;
			case NPCID.GoldenSlime:
				basePointGrant = 10000;
				break;
			case NPCID.KingSlime:
				basePointGrant = 400;
				break;
			case NPCID.DemonEye:
			case NPCID.DemonEye2:
				basePointGrant = 20;
				break;
			case NPCID.CataractEye:
			case NPCID.CataractEye2:
			case NPCID.SleepyEye:
			case NPCID.SleepyEye2:
			case NPCID.DialatedEye:
			case NPCID.DialatedEye2:
			case NPCID.GreenEye:
			case NPCID.GreenEye2:
			case NPCID.PurpleEye:
			case NPCID.PurpleEye2:
				basePointGrant = 21;
				break;
			case NPCID.ServantofCthulhu:
				basePointGrant = 1;
				break;
			case NPCID.WanderingEye:
				basePointGrant = 1000;
				break;
			case NPCID.EyeofCthulhu:
				basePointGrant = 400;
				break;
			case NPCID.GoblinArcher:
			case NPCID.GoblinThief:
			case NPCID.GoblinPeon:
			case NPCID.GoblinScout:
			case NPCID.GoblinSorcerer:
				basePointGrant = 25;
				break;
			case NPCID.GoblinSummoner:
				basePointGrant = 1000;
				break;
			case NPCID.EaterofSouls:
				basePointGrant = 20;
				break;
			case NPCID.DevourerHead:
				basePointGrant = 50;
				break;
			case NPCID.CorruptSlime:
			case NPCID.Slimeling:
			case NPCID.DarkMummy:
				basePointGrant = 30;
				break;
			case NPCID.Corruptor:
				basePointGrant = 100;
				break;
			case NPCID.Slimer:
				basePointGrant = 100;
				break;
			case NPCID.EaterofWorldsHead:
			case NPCID.EaterofWorldsBody:
			case NPCID.EaterofWorldsTail:
				basePointGrant = 30;
				break;
			case NPCID.Crimera:
			case NPCID.FaceMonster:
			case NPCID.LittleCrimera:
			case NPCID.BigCrimera:
			case NPCID.BloodCrawler:
			case NPCID.Crimslime:
				basePointGrant = 35;
				break;
			case NPCID.BloodMummy:
				basePointGrant = 60;
				break;
			case NPCID.Creeper:
				basePointGrant = 15;
				break;
			case NPCID.Herpling:
			case NPCID.FloatyGross:
				basePointGrant = 100;
				break;
			case NPCID.IchorSticker:
				basePointGrant = 150;
				break;
			case NPCID.CursedHammer:
			case NPCID.CrimsonAxe:
				basePointGrant = 300;
				break;
			case NPCID.Bee:
			case NPCID.BeeSmall:
				basePointGrant = 1;
				break;
			case NPCID.Hornet:
			case NPCID.HornetHoney:
			case NPCID.HornetSpikey:
			case NPCID.HornetLeafy:
			case NPCID.HornetFatty:
			case NPCID.HornetStingy:
				basePointGrant = 29;
				break;
			case NPCID.BigHornetHoney:
			case NPCID.BigHornetFatty:
			case NPCID.BigHornetLeafy:
			case NPCID.BigHornetSpikey:
			case NPCID.BigHornetStingy:
				basePointGrant = 39;
				break;
			case NPCID.SpikedJungleSlime:
				basePointGrant = 40;
				break;
			case NPCID.JungleBat:
				basePointGrant = 15;
				break;
			case NPCID.JungleSlime:
				basePointGrant = 10;
				break;
			case NPCID.LacBeetle:
				basePointGrant = 18;
				break;
			case NPCID.DoctorBones:
				basePointGrant = 3000;
				break;
			case NPCID.QueenBee:
				basePointGrant = 750;
				break;
			case NPCID.IceSlime:
				basePointGrant = 19;
				break;
			case NPCID.Wolf:
			case NPCID.IcyMerman:
				basePointGrant = 65;
				break;
			case NPCID.IceElemental:
				basePointGrant = 60;
				break;
			case NPCID.IceBat:
				basePointGrant = 40;
				break;
			case NPCID.SnowFlinx:
				basePointGrant = 12;
				break;
			case NPCID.UndeadViking:
				basePointGrant = 21;
				break;
			case NPCID.IceGolem:
			case NPCID.Deerclops:
				basePointGrant = 1000;
				break;
		}

		//Point amplified

		ModContent.GetInstance<ArcadeMode_System>().Point += basePointGrant;

		return false;
	}
}
public class ArcadeUI : UIState {
	public Roguelike_UIPanel panel_Main;
	public Roguelike_UIPanel panel_Header;
	//Header section
	public Roguelike_UIText text_Start;
	public Roguelike_UIText text_WaveCounter;


	public Roguelike_UIPanel panel_Body;
	//Where to show enemy
	public List<img_EnemyDisplay> list_img_enemies = new();

	public Roguelike_UIPanel panel_Footer;
	//Footer section
	public ExitUI text_Back;
	private void Initialize_BasicPanel() {
		panel_Main = new();
		panel_Main.UISetWidthHeight(1200, 800);
		panel_Main.Disable_MouseItemUsesWhenHoverOverAUI();
		panel_Main.HAlign = panel_Main.VAlign = .5f;
		Append(panel_Main);

		panel_Header = new();
		panel_Header.Width.Precent = 1;
		panel_Header.Height.Pixels = 60;
		panel_Header.Disable_MouseItemUsesWhenHoverOverAUI();
		panel_Main.Append(panel_Header);

		panel_Footer = new();
		panel_Footer.Width.Precent = 1;
		panel_Footer.Height.Pixels = 60;
		panel_Footer.VAlign = 1f;
		panel_Footer.Disable_MouseItemUsesWhenHoverOverAUI();
		panel_Main.Append(panel_Footer);


		panel_Body = new();
		panel_Body.Width.Precent = 1;
		panel_Body.Height.Pixels = panel_Main.Height.Pixels - panel_Header.Height.Pixels - panel_Footer.Height.Pixels - 40;
		panel_Body.VAlign = .5f;
		panel_Body.Disable_MouseItemUsesWhenHoverOverAUI();
		panel_Main.Append(panel_Body);
	}
	private void Initialize_HeaderPanel() {
		text_Start = new("Start", 1.2f);
		text_Start.HAlign = 1f;
		text_Start.VAlign = .5f;
		text_Start.OnLeftClick += Text_Start_OnLeftClick;
		text_Start.OnUpdate += Text_Start_OnUpdate;
		panel_Header.Append(text_Start);

		text_WaveCounter = new("Wave : 0", 1.2f);
		text_WaveCounter.VAlign = .5f;
		text_WaveCounter.OnUpdate += Text_WaveCounter_OnUpdate;
		panel_Header.Append(text_WaveCounter);
	}
	private void Text_Start_OnUpdate(UIElement affectedElement) {
		if (affectedElement.IsMouseHovering) {
			text_Start.TextColor = Color.Yellow;
		}
		else {
			text_Start.TextColor = Color.White;
		}
	}
	private void Text_Start_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (!ModContent.GetInstance<ArcadeMode_System>().Active) {
			ModContent.GetInstance<ArcadeMode_System>().Start_ArcadeMode();
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		}
	}
	private void Text_WaveCounter_OnUpdate(UIElement affectedElement) {
		ArcadeMode_System system = ModContent.GetInstance<ArcadeMode_System>();
		text_WaveCounter.SetText($"Wave : {system.CurrentWave + 1}");
	}
	private void Initialize_FooterPanel() {
		text_Back = new();
		text_Back.HAlign = 1f;
		text_Back.VAlign = .5f;
		text_Back.SetText(text_Back.Text, 1.2f, false);
		panel_Footer.Append(text_Back);
	}
	int Row = 20, Column = 10;
	private void Initialize_BodyPanel() {
		Add_All_EnemyToShowUp();
	}
	public void Add_All_EnemyToShowUp() {
		ArcadeMode_System system = ModContent.GetInstance<ArcadeMode_System>();

		int[] information = system.Wave_Information().Where(id => id != 0).ToArray();
		int count = information.Length;
		for (int i = 0; i < count; i++) {
			img_EnemyDisplay img = new(ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT), information[i]);
			img.HAlign = (i % Row) / (Row - 1f);
			img.VAlign = (i / Row) / (Column - 1f);
			img.UISetWidthHeight(52, 52);
			panel_Body.Append(img);
			list_img_enemies.Add(img);
		}
	}

	public override void OnActivate() {
		if (list_img_enemies.Count < 1) {
			Add_All_EnemyToShowUp();
		}
	}
	public override void OnDeactivate() {
		foreach (var item in list_img_enemies) {
			item.Remove();
		}
		list_img_enemies.Clear();
	}
	public override void OnInitialize() {
		list_img_enemies = new();
		Initialize_BasicPanel();
		Initialize_HeaderPanel();
		Initialize_FooterPanel();
		Initialize_BodyPanel();
	}
}
public class img_EnemyDisplay : Roguelike_UIImage {
	Texture2D texture;
	int NPCid = 0;
	NPC npcNetID = null;
	public img_EnemyDisplay(Asset<Texture2D> texture, int ID) : base(texture) {
		this.texture = texture.Value;
		NPCid = ContentSamples.NpcsByNetId[ID].type;
		npcNetID = ContentSamples.NpcsByNetId[ID];
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (NPCid <= 0 || npcNetID == null) {
			return;
		}
		Main.instance.LoadNPC(NPCid);
		Texture2D texture = TextureAssets.Npc[NPCid].Value;
		Vector2 drawpos = this.GetInnerDimensions().Center();
		Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPCid] * .5f);

		Color color = npcNetID.color == Color.Transparent ? Color.White : npcNetID.color;

		spriteBatch.Draw(texture, drawpos, texture.Frame(1, Main.npcFrameCount[NPCid], 0, 0), color, 0, origin,
			ScaleCalculation(this.texture.Size(), new Vector2(texture.Width, texture.Height / (float)Main.npcFrameCount[NPCid])), SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.2f);
}
internal class ArcadeModeActivation : ModItem {
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
			ModContent.GetInstance<UniversalSystem>().ActivateArcadeUI();

			//SoundEngine.PlaySound(SoundID.Roar, player.position);
			//if (player.ItemAnimationJustStarted && player.ownedProjectileCounts[ModContent.ProjectileType<BossRush_SpawnBoss_Projectile>()] < 1) {
			//	if (!handler.Active) {
			//		handler.Start_BossRush();
			//	}
			//	Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<BossRush_SpawnBoss_Projectile>(), 0, 0, player.whoAmI, 0, 0, 600);
			//	handler.BossSpawnCD = ModUtils.ToSecond(30);
			//	return true;
			//}
			//bool NGplus = handler.NGplus;
			//if (ModContent.GetInstance<UniversalSystem>().Count_BossKill > 1) {
			//	Main.NewText("Restart boss rush !");
			//	handler.Start_BossRush(NGplus);
			//}
			//else {
			//	Main.NewText("Boss rush mode start !");
			//	handler.Start_BossRush(NGplus);
			//}
		}
		return true;
	}
}
public class ArcadeMode_SpawnNPC_Indicator : ModObject {
	public int NPCtoSpawn = 0;
	public override void SetDefaults() {
		timeLeft = 180;
		NPCtoSpawn = 0;
	}
	public override void Draw(SpriteBatch spritebatch) {
		Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Center - Main.screenPosition;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.Red, MathHelper.PiOver2, origin, 2, SpriteEffects.None);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.White, MathHelper.PiOver2, origin, 2 * .5f, SpriteEffects.None);

		Main.EntitySpriteDraw(texture, drawpos, null, Color.Orange, 0, origin, 2 * .75f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.White, 0, origin, 2 * .25f, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
	}
	public override void OnKill() {
		for (int i = 0; i < 30; i++) {
			Dust dust = Dust.NewDustDirect(Center, 0, 0, DustID.GemTopaz);
			dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
			dust.scale += Main.rand.NextFloat(-.1f, .2f);
			dust.noGravity = true;
		}
		ModUtils.DustStar(Center, DustID.GemRuby, Color.Red);
		if (NPCtoSpawn == 0) {
			return;
		}
		NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)Center.X, (int)Center.Y, NPCtoSpawn);
	}
}
public class ArcadeMode_SpawnBoss_Projectile : ModProjectile {
	public override string Texture => ModTexture.WHITEDOT;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.damage = 0;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = 9999;
		Projectile.extraUpdates = 10;
		Projectile.light = 1;
	}
	public override void AI() {
		var pos = ModContent.GetInstance<ArcadeMode_System>().Rect_Structure().Center.ToWorldCoordinates();
		Projectile.velocity += (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * .1f;
		Projectile.velocity *= .995f;
		if (Projectile.Center.IsCloseToPosition(pos, 10)) {
			if (--Projectile.ai[2] <= 0) {
				Projectile.Kill();
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);

		lightColor = Color.Red;

		Projectile.DrawTrail(lightColor);

		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Texture2D texture2 = ModContent.Request<Texture2D>(ModTexture.Glow_Medium).Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);

		Main.EntitySpriteDraw(texture, drawpos, null, Color.White with { A = 25 }, 0, texture.Size() * .5f, Projectile.scale + 3, SpriteEffects.None);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.White with { A = 100 }, 0, texture2.Size() * .5f, Projectile.scale + 1, SpriteEffects.None);

		Main.EntitySpriteDraw(texture, drawpos, null, lightColor with { A = 25 }, 0, texture.Size() * .5f, Projectile.scale + 2, SpriteEffects.None);
		Main.EntitySpriteDraw(texture2, drawpos, null, lightColor with { A = 200 }, 0, texture2.Size() * .5f, Projectile.scale, SpriteEffects.None);

		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.Black, 0, texture2.Size() * .5f, Projectile.scale, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
	public override void OnKill(int timeLeft) {
		ModContent.GetInstance<ArcadeMode_System>().SpawnBoss();
		ModUtils.DustStar(Projectile.Center, DustID.GemRuby, Color.Red, 50, 24, 0, 15);
		for (int i = 0; i < 360; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GemRuby);
			dust.noGravity = true;
			dust.scale += Main.rand.NextFloat(1.4f);
			dust.velocity = Main.rand.NextVector2CircularEdge(30, 30) * Main.rand.NextFloat(.75f, 1.1f);
		}
	}
}
