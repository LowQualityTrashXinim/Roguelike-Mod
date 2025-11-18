using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode;
public class RoguelikeBiomeHandle_ModPlayer : ModPlayer {
	public HashSet<short> CurrentBiome = new HashSet<short>();
	public float shaderOff = 1;
	public float shaderstorm = 1f;
	public SlotId strongBlizzardSound = SlotId.Invalid;
	public SlotId insideBlizzardSound = SlotId.Invalid;
	public float strongblizzVol = 1f;
	public override void OnEnterWorld() {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		if (gen.RoguelikeWorld && SubworldSystem.Current == null) {
			if (gen.BiomeMapping[0] == null)
				ModContent.GetInstance<RogueLikeWorldGen>().InitializeBiomeWorld();
		}
	}
	public override void ResetEffects() {
		CurrentBiome.Clear();
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		if (!Player.active || !gen.RoguelikeWorld) {
			return;
		}
		Point position = (new Vector2(Player.position.X / RogueLikeWorldGen.GridPart_X, Player.position.Y / RogueLikeWorldGen.GridPart_Y)).ToTileCoordinates();
		int WorldIndex = RogueLikeWorldGen.MapIndex(position.X, position.Y);
		if (WorldIndex >= gen.BiomeMapping.Length) {
			return;
		}
		string zone = gen.BiomeMapping[WorldIndex];
		bool IsInSmallForest = gen.ForestZone.Where(rect => rect.Contains(Player.Center.ToTileCoordinates())).Any();
		if (IsInSmallForest) {
			CurrentBiome.Add(Bid.Forest);
		}
		if (zone == null) {
			return;
		}
		for (int i = 0; i < zone.Length; i++) {
			short biomeID = RogueLikeWorldGen.CharToBid(zone, i);
			CurrentBiome.Add(biomeID);
		}
	}
}
public abstract class BiomeData : ModType {
	public short BiomeID = Bid.None;
	public ushort Type = 0;
	protected override void Register() {
		Type = RoguelikeBiomeHandle_ModSystem.Register(this);
	}
	public List<spawnInfo> SpawningInfo(NPCSpawnInfo spawnInfo) {
		return new();
	}
	public struct spawnInfo {
		/// <summary>
		/// Type of NPC to be spawned
		/// </summary>
		public int Type = 0;
		/// <summary>
		/// Their weight range from 0 to 1f with 0 being unlikely to spawn and 1f being common<br/>
		/// </summary>
		public float Weight = 1f;

		public spawnInfo() {
		}
	}
}
public class RoguelikeBiomeHandle_ModSystem : ModSystem {
	FieldInfo blizzardsetting = null;
	FieldInfo blizzardsoundset = null;
	public static Dictionary<short, BiomeData> dict_BiomeData { get; private set; } = new();
	public static ushort Register(BiomeData data) {
		ModTypeLookup<BiomeData>.Register(data);
		if (!dict_BiomeData.TryAdd(data.BiomeID, data)) {
			dict_BiomeData[data.BiomeID] = data;
		}
		return (ushort)dict_BiomeData.Keys.Count;
	}
	public override void Load() {
		if (dict_BiomeData == null) {
			dict_BiomeData = new();
		}
		if (blizzardsetting == null) {
			blizzardsetting = typeof(Player).GetField("disabledBlizzardGraphic", BindingFlags.NonPublic | BindingFlags.Static);
		}
		if (blizzardsoundset == null) {
			blizzardsoundset = typeof(Player).GetField("disabledBlizzardSound", BindingFlags.NonPublic | BindingFlags.Static);
		}
		On_Main.DrawBlack += On_Main_DrawBlack;
		On_Player.UpdateBiomes += On_Player_UpdateBiomes;
		On_WorldGen.UpdateWorld_Inner += On_WorldGen_UpdateWorld_Inner;
	}

	private void On_WorldGen_UpdateWorld_Inner(On_WorldGen.orig_UpdateWorld_Inner orig) {
		if (!ModContent.GetInstance<RogueLikeWorldGen>().RoguelikeWorld) {
			orig();
			return;
		}
		Wiring.UpdateMech();
		TileEntity.UpdateStart();
		foreach (TileEntity value in TileEntity.ByID.Values) {
			value.Update();
		}

		TileEntity.UpdateEnd();
		Liquid.skipCount++;
		if (Liquid.skipCount > 1) {
			Liquid.UpdateLiquid();
			Liquid.skipCount = 0;
		}
	}

	public override void Unload() {
		dict_BiomeData.Clear();
	}

	private void On_Main_DrawBlack(On_Main.orig_DrawBlack orig, Main self, bool force) {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		if (!gen.RoguelikeWorld) {
			orig(self, force);
			return;
		}
		if (Main.shimmerAlpha == 1f)
			return;

		int num = (Main.tileColor.R + Main.tileColor.G + Main.tileColor.B) / 3;
		float num2 = (float)(num * 0.1f) / 255f;
		if (Lighting.Mode == LightMode.Retro) {
			num2 = (Main.tileColor.R - 55) / 255f;
			if (num2 < 0f)
				num2 = 0f;
		}
		else if (Lighting.Mode == LightMode.Trippy) {
			num2 = (num - 55) / 255f;
			if (num2 < 0f)
				num2 = 0f;
		}
		Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();
		Vector2 vector = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
		Point point = new Point(-Main.offScreenRange / 16 + screenOverdrawOffset.X, -Main.offScreenRange / 16 + screenOverdrawOffset.Y);
		int num3 = (int)((Main.screenPosition.X - vector.X) / 16f - 1f) + point.X;
		int num4 = (int)((Main.screenPosition.X + Main.screenWidth + vector.X) / 16f) + 2 - point.X;
		int num5 = (int)((Main.screenPosition.Y - vector.Y) / 16f - 1f) + point.Y;
		int num6 = (int)((Main.screenPosition.Y + Main.screenHeight + vector.Y) / 16f) + 5 - point.Y;

		if (num3 < 0)
			num3 = point.X;

		if (num4 > Main.maxTilesX)
			num4 = Main.maxTilesX - point.X;

		if (num5 < 0)
			num5 = point.Y;

		if (num6 > Main.maxTilesY)
			num6 = Main.maxTilesY - point.Y;

		bool flag = Main.ShouldShowInvisibleWalls();
		for (int i = num5; i < num6; i++) {
			bool flag2 = i >= Main.UnderworldLayer;
			if (flag2)
				num2 = 0.2f;
			for (int j = num3; j < num4; j++) {
				int num7 = j;
				for (; j < num4; j++) {
					if (!WorldGen.InWorld(j, i))
						return;

					Tile tile = Main.tile[j, i];
					if (tile == null)
						tile = new Tile();

					float num8 = Lighting.Brightness(j, i);
					num8 = MathF.Floor(num8 * 255f) / 255f;
					byte b = tile.LiquidAmount;
					bool num9 = num8 <= num2 && ((b < 250) || WorldGen.SolidTile(tile) || (b >= 200 && num8 == 0f));
					bool flag3 = tile.IsActuated && Main.tileBlockLight[tile.TileType] && (!tile.IsTileInvisible || flag);
					bool flag4 = !WallID.Sets.Transparent[tile.WallType] && (!tile.IsWallInvisible || flag);
					if (!num9 || (!flag4 && !flag3) || (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(j, i) && tile.WallType == WallID.None && !tile.IsHalfBlock))
						break;
				}

				if (j - num7 > 0) {
					Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(num7 << 4, i << 4) - Main.screenPosition + vector, new Rectangle(0, 0, j - num7 << 4, 16), Color.Black);
				}
			}
		}
	}

	/// <summary>
	/// Do NOT touch this, I tried my best to reimplement this from the base vanilla but it is so fuckery and I just hope it doesn't break anything
	/// </summary>
	/// <param name="self"></param>
	/// <param name="modplayer"></param>
	private void VanillaReimplementation(Player self, RoguelikeBiomeHandle_ModPlayer modplayer) {
		bool flag3 = (self.ZoneTowerStardust = false);
		bool flag5 = (self.ZoneTowerNebula = flag3);
		bool zoneTowerSolar = (self.ZoneTowerVortex = flag5);
		self.ZoneTowerSolar = zoneTowerSolar;
		self.ZoneOldOneArmy = false;
		Vector2 vector = Vector2.Zero;
		Vector2 vector2 = Vector2.Zero;
		Vector2 vector3 = Vector2.Zero;
		Vector2 vector4 = Vector2.Zero;
		_ = Vector2.Zero;
		for (int i = 0; i < 200; i++) {
			if (!Main.npc[i].active)
				continue;

			if (Main.npc[i].type == NPCID.LunarTowerStardust) {
				if (self.Distance(Main.npc[i].Center) <= 4000f) {
					self.ZoneTowerStardust = true;
					vector4 = Main.npc[i].Center;
				}
			}
			else if (Main.npc[i].type == NPCID.LunarTowerNebula) {
				if (self.Distance(Main.npc[i].Center) <= 4000f) {
					self.ZoneTowerNebula = true;
					vector3 = Main.npc[i].Center;
				}
			}
			else if (Main.npc[i].type == NPCID.LunarTowerVortex) {
				if (self.Distance(Main.npc[i].Center) <= 4000f) {
					self.ZoneTowerVortex = true;
					vector2 = Main.npc[i].Center;
				}
			}
			else if (Main.npc[i].type == NPCID.LunarTowerSolar) {
				if (self.Distance(Main.npc[i].Center) <= 4000f) {
					self.ZoneTowerSolar = true;
					vector = Main.npc[i].Center;
				}
			}
			else if (Main.npc[i].type == NPCID.DD2LanePortal && self.Distance(Main.npc[i].Center) <= 4000f) {
				self.ZoneOldOneArmy = true;
				vector = Main.npc[i].Center;
			}
		}

		float num5 = 1f;
		float num6 = 1f;
		float num7 = Main.shimmerAlpha;
		if (self.CanSeeShimmerEffects()) {
			num5 *= 1f;
			num6 *= 0.7f;
			if (num7 < 1f) {
				num7 += 0.025f;
				if (num7 > 1f)
					num7 = 1f;
			}

			if (num7 >= 0.5f) {
				Main.shimmerDarken = MathHelper.Clamp(Main.shimmerDarken + 0.025f, 0f, 1f);
				Main.shimmerBrightenDelay = 4f;
			}
		}
		else if (num7 > 0f) {
			Main.shimmerDarken = MathHelper.Clamp(Main.shimmerDarken - 0.05f, 0f, 1f);
			if (Main.shimmerDarken == 0f && Main.shimmerBrightenDelay > 0f)
				Main.shimmerBrightenDelay -= 1f;

			if (Main.shimmerBrightenDelay == 0f)
				num7 -= 0.05f;

			if (num7 < 0f)
				num7 = 0f;
		}

		Main.shimmerAlpha = num7;
		if (Main.getGoodWorld) {
			bool flag7 = false;
			int num8 = NPC.FindFirstNPC(245);
			if (num8 >= 0 && Vector2.Distance(self.Center, Main.npc[num8].Center) < 2000f)
				flag7 = true;

			if (flag7) {
				num5 *= 0.6f;
				num6 *= 0.6f;
			}
			else if (self.ZoneLihzhardTemple) {
				num5 *= 0.88f;
				num6 *= 0.88f;
			}
			else if (self.ZoneDungeon) {
				num5 *= 0.94f;
				num6 *= 0.94f;
			}
		}

		if (num5 != Player.airLightDecay) {
			if (Player.airLightDecay >= num5) {
				Player.airLightDecay -= 0.005f;
				if (Player.airLightDecay < num5)
					Player.airLightDecay = num5;
			}
			else {
				Player.airLightDecay += 0.005f;
				if (Player.airLightDecay > num5)
					Player.airLightDecay = num5;
			}
		}

		if (num6 != Player.solidLightDecay) {
			if (Player.solidLightDecay >= num6) {
				Player.solidLightDecay -= 0.005f;
				if (Player.solidLightDecay < num6)
					Player.solidLightDecay = num6;
			}
			else {
				Player.solidLightDecay += 0.005f;
				if (Player.solidLightDecay > num6)
					Player.solidLightDecay = num6;
			}
		}
		Point point = self.Center.ToTileCoordinates();
		//bool flag8 = self.ZoneRain && self.ZoneSnow;
		//if (Main.remixWorld)
		//	flag8 = (double)(self.position.Y / 16f) > Main.worldSurface && Main.raining && self.ZoneSnow;

		bool flag9 = point.Y > Main.maxTilesY - 320;
		bool flag10 = self.ZoneOverworldHeight && (point.X < 380 || point.X > Main.maxTilesX - 380);

		//TODO: Are these flags a problem?
		LoaderManager.Get<BiomeLoader>().UpdateBiomes(self);

		self.ManageSpecialBiomeVisuals("Stardust", self.ZoneTowerStardust, vector4 - new Vector2(0f, 10f));
		self.ManageSpecialBiomeVisuals("Nebula", self.ZoneTowerNebula, vector3 - new Vector2(0f, 10f));
		self.ManageSpecialBiomeVisuals("Vortex", self.ZoneTowerVortex, vector2 - new Vector2(0f, 10f));
		self.ManageSpecialBiomeVisuals("Solar", self.ZoneTowerSolar, vector - new Vector2(0f, 10f));
		self.ManageSpecialBiomeVisuals("MoonLord", NPC.AnyNPCs(398));
		self.ManageSpecialBiomeVisuals("BloodMoon", Main.bloodMoon || Main.SceneMetrics.BloodMoonMonolith || self.bloodMoonMonolithShader);
		self.ManageSpecialBiomeVisuals("Blizzard", Main.UseStormEffects /*&& flag8*/);
		self.ManageSpecialBiomeVisuals("HeatDistortion", Main.UseHeatDistortion && (flag9 || ((double)point.Y < Main.worldSurface && self.ZoneDesert && !flag10 && !Main.raining && !Filters.Scene["Sandstorm"].IsActive())));
		if (Main.GraveyardVisualIntensity > 0f) {
			if (!Filters.Scene["Graveyard"].IsActive()) {
				Filters.Scene.Activate("Graveyard", default(Vector2));
			}
			else {
				Filters.Scene["Graveyard"].GetShader().UseTargetPosition(self.Center);
				float progress = MathHelper.Lerp(0f, 0.75f, Main.GraveyardVisualIntensity);
				Filters.Scene["Graveyard"].GetShader().UseProgress(progress);
				Filters.Scene["Graveyard"].GetShader().UseIntensity(1.2f);
			}
		}
		else if (Filters.Scene["Graveyard"].IsActive()) {
			Filters.Scene.Deactivate("Graveyard");
		}

		if (!Filters.Scene["Sepia"].IsActive() && self.dontStarveShader)
			Filters.Scene.Activate("Sepia", default(Vector2));
		else if (Filters.Scene["Sepia"].IsActive() && !self.dontStarveShader)
			Filters.Scene.Deactivate("Sepia");

		if (!Filters.Scene["WaterDistortion"].IsActive() && Main.WaveQuality > 0)
			Filters.Scene.Activate("WaterDistortion", default(Vector2));
		else if (Filters.Scene["WaterDistortion"].IsActive() && Main.WaveQuality == 0)
			Filters.Scene.Deactivate("WaterDistortion");

		if (Filters.Scene["WaterDistortion"].IsActive()) {
			float num9 = (float)Main.maxTilesX * 0.5f - Math.Abs((float)point.X - (float)Main.maxTilesX * 0.5f);
			float num10 = 1f;
			float num11 = Math.Abs(Main.windSpeedCurrent);
			num10 += num11 * 1.25f;
			float num12 = MathHelper.Clamp(Main.maxRaining, 0f, 1f);
			num10 += num12 * 1.25f;
			float num13 = 0f - (MathHelper.Clamp((num9 - 380f) / 100f, 0f, 1f) * 0.5f - 0.25f);
			num10 += num13;
			float num14 = 1f - MathHelper.Clamp(3f * ((float)((double)point.Y - Main.worldSurface) / (float)(Main.rockLayer - Main.worldSurface)), 0f, 1f);
			num10 *= num14;
			float num15 = 0.9f - MathHelper.Clamp((float)(Main.maxTilesY - point.Y - 200) / 300f, 0f, 1f) * 0.9f;
			num10 += num15;
			num10 += (1f - num14) * 0.75f;
			num10 = MathHelper.Clamp(num10, 0f, 2.5f);
			Filters.Scene["WaterDistortion"].GetShader().UseIntensity(num10);
		}

		if (flag9) {
			float val = (float)(point.Y - (Main.maxTilesY - 320)) / 120f;
			val = Math.Min(1f, val) * 2f;
			Filters.Scene["HeatDistortion"].GetShader().UseIntensity(val);
		}

		modplayer.shaderOff = Terraria.Utils.Clamp(modplayer.shaderOff + (float)self.behindBackWall.ToDirectionInt() * -0.005f, -0.1f, 1.1f);
		modplayer.shaderstorm = Terraria.Utils.Clamp(modplayer.shaderOff, 0f, 1f);
		if (Filters.Scene["Sandstorm"].IsActive()) {
			Filters.Scene["Sandstorm"].GetShader().UseIntensity(modplayer.shaderstorm * 0.4f * Math.Min(1f, Sandstorm.Severity));
			Filters.Scene["Sandstorm"].GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * modplayer.shaderstorm);
			((SimpleOverlay)Overlays.Scene["Sandstorm"]).GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * (1f - modplayer.shaderstorm));
		}
		else if (self.ZoneDesert && !flag10 && !Main.raining && !flag9) {
			Vector3 vector5 = Main.tileColor.ToVector3();
			float num16 = (vector5.X + vector5.Y + vector5.Z) / 3f;
			float num17 = modplayer.shaderstorm * 4f * Math.Max(0f, 0.5f - Main.cloudAlpha) * num16;
			Filters.Scene["HeatDistortion"].GetShader().UseIntensity(num17);
			if (num17 <= 0f)
				Filters.Scene["HeatDistortion"].IsHidden = true;
			else
				Filters.Scene["HeatDistortion"].IsHidden = false;
		}
		//bool blizset = (bool)blizzardsetting.GetValue(null);
		//if (!blizset) {
		//	try {
		//		if (flag8) {
		//			float cloudAlpha = Main.cloudAlpha;
		//			if (Main.remixWorld)
		//				Main.cloudAlpha = 0.4f;

		//			bool value = NPC.IsADeerclopsNearScreen();
		//			modplayer.shaderdeerclop = MathHelper.Clamp(modplayer.shaderdeerclop + (float)value.ToDirectionInt() * 0.0033333334f, 0f, 1f);
		//			float num18 = Math.Min(1f, Main.cloudAlpha * 2f) * modplayer.shaderstorm;
		//			float num19 = modplayer.shaderstorm * 0.4f * Math.Min(1f, Main.cloudAlpha * 2f) * 0.9f + 0.1f;
		//			num19 = MathHelper.Lerp(num19, num19 * 0.5f, modplayer.shaderdeerclop);
		//			num18 = MathHelper.Lerp(num18, num18 * 0.5f, modplayer.shaderdeerclop);
		//			Filters.Scene["Blizzard"].GetShader().UseIntensity(num19);
		//			Filters.Scene["Blizzard"].GetShader().UseOpacity(num18);
		//			((SimpleOverlay)Overlays.Scene["Blizzard"]).GetShader().UseOpacity(1f - num18);
		//			if (Main.remixWorld)
		//				Main.cloudAlpha = cloudAlpha;
		//		}
		//	}
		//	catch {
		//		blizzardsetting.SetValue(null, true);
		//	}
		//}
		//bool blizsoundset = (bool)blizzardsoundset.GetValue(null);
		//if (!blizsoundset) {
		//	try {
		//		if (flag8) {

		//			ActiveSound activeSound = SoundEngine.FindActiveSound(SoundID.BlizzardStrongLoop);
		//			ActiveSound activeSound2 = SoundEngine.FindActiveSound(SoundID.BlizzardInsideBuildingLoop);
		//			if (activeSound == null)
		//				modplayer.strongBlizzardSound = SoundEngine.PlaySound(SoundID.BlizzardStrongLoop);

		//			if (activeSound2 == null)
		//				modplayer.insideBlizzardSound = SoundEngine.PlaySound(SoundID.BlizzardInsideBuildingLoop);

		//			SoundEngine.TryGetActiveSound(modplayer.strongBlizzardSound, out _);
		//			SoundEngine.TryGetActiveSound(modplayer.insideBlizzardSound, out ActiveSound sound);

		//			activeSound2 = sound;
		//		}

		//		if (flag8)
		//			modplayer.strongblizzVol = Math.Min(modplayer.strongblizzVol + 0.01f, 1f);
		//		else
		//			modplayer.strongblizzVol = Math.Max(modplayer.strongblizzVol - 0.01f, 0f);

		//		float num20 = Math.Min(1f, Main.cloudAlpha * 2f) * modplayer.shaderstorm;
		//		SoundEngine.TryGetActiveSound(modplayer.strongBlizzardSound, out ActiveSound activeSound3);
		//		SoundEngine.TryGetActiveSound(modplayer.insideBlizzardSound, out ActiveSound activeSound4);
		//		if (modplayer.strongblizzVol > 0f) {
		//			if (activeSound3 == null) {
		//				modplayer.strongBlizzardSound = SoundEngine.PlaySound(SoundID.BlizzardStrongLoop);
		//				SoundEngine.TryGetActiveSound(modplayer.strongBlizzardSound, out ActiveSound soundResult);
		//				activeSound3 = soundResult;
		//			}

		//			activeSound3.Volume = num20 * modplayer.strongblizzVol;
		//			if (activeSound4 == null) {
		//				modplayer.insideBlizzardSound = SoundEngine.PlaySound(SoundID.BlizzardInsideBuildingLoop);
		//				SoundEngine.TryGetActiveSound(modplayer.insideBlizzardSound, out ActiveSound soundResult);
		//				activeSound4 = soundResult;
		//			}

		//			activeSound4.Volume = (1f - num20) * modplayer.strongblizzVol;
		//		}
		//		else {
		//			if (activeSound3 != null)
		//				activeSound3.Volume = 0f;
		//			else
		//				modplayer.strongBlizzardSound = SlotId.Invalid;

		//			if (activeSound4 != null)
		//				activeSound4.Volume = 0f;
		//			else
		//				modplayer.insideBlizzardSound = SlotId.Invalid;
		//		}
		//	}
		//	catch {
		//		blizzardsoundset.SetValue(null, true);
		//	}
		//}

		// Added by TML.
		//self.ZonePurity = self.InZonePurity();

		if (!self.dead) {
			Point point2 = self.Center.ToTileCoordinates();
			// Extra patch context.
			if (WorldGen.InWorld(point2.X, point2.Y, 1)) {
				int num21 = 0;
				if (Main.tile[point2.X, point2.Y] != null)
					num21 = Main.tile[point2.X, point2.Y].WallType;

				switch (num21) {
					case 86:
						AchievementsHelper.HandleSpecialEvent(self, 12);
						break;
					case 62:
						AchievementsHelper.HandleSpecialEvent(self, 13);
						break;
				}
			}

			if (self._funkytownAchievementCheckCooldown > 0)
				self._funkytownAchievementCheckCooldown--;

			if (Main.specialSeedWorld)
				AchievementsHelper.HandleSpecialEvent(self, 26);

			if (self.position.Y / 16f > (float)Main.UnderworldLayer)
				AchievementsHelper.HandleSpecialEvent(self, 14);
			else if (self._funkytownAchievementCheckCooldown == 0 && (double)(self.position.Y / 16f) < Main.worldSurface && self.ZoneGlowshroom)
				AchievementsHelper.HandleSpecialEvent(self, 15);
			else if (self._funkytownAchievementCheckCooldown == 0 && self.ZoneGraveyard)
				AchievementsHelper.HandleSpecialEvent(self, 18);
			// Extra patch context.
		}
		else {
			self._funkytownAchievementCheckCooldown = 100;
		}

		LoaderManager.Get<SceneEffectLoader>().UpdateSceneEffect(self);
	}
	private void On_Player_UpdateBiomes(On_Player.orig_UpdateBiomes orig, Player self) {
		RogueLikeWorldGen gen = ModContent.GetInstance<RogueLikeWorldGen>();
		if (!gen.RoguelikeWorld) {
			orig(self);
			return;
		}
		self.zone1 = new BitsByte();
		self.zone2 = new BitsByte();
		self.zone3 = new BitsByte();
		self.zone4 = new BitsByte();
		self.zone5 = new BitsByte();

		Tile tileSafely = Framing.GetTileSafely(self.Center);
		if (tileSafely != null)
			self.behindBackWall = tileSafely.WallType > WallID.None;
		//underworldHeight.SetValue(null, Main.maxTilesY - 200);
		RoguelikeBiomeHandle_ModPlayer modplayer = self.GetModPlayer<RoguelikeBiomeHandle_ModPlayer>();
		Point toTile = self.Center.ToTileCoordinates();
		Main.worldSurface = (toTile.Y - Main.screenHeight);
		Main.rockLayer = (toTile.Y - Main.screenHeight);
		if (modplayer.CurrentBiome.Contains(Bid.Forest)) {
			self.ZonePurity = true;
			self.ZoneOverworldHeight = true;
			Main.worldSurface = toTile.Y + Main.screenHeight / 16;
			Main.rockLayer = toTile.Y + Main.screenHeight / 16;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Corruption)) {
			self.ZoneCorrupt = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Crimson)) {
			self.ZoneCrimson = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.BeeNest)) {
			self.ZoneJungle = true;
			self.ZoneRockLayerHeight = true;
			self.ZoneHive = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Tundra)) {
			self.ZoneSnow = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Underworld)) {
			self.ZoneUnderworldHeight = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Jungle)) {
			self.ZoneJungle = true;
			self.ZoneRockLayerHeight = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Hallow)) {
			self.ZoneHallow = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Ocean)) {
			self.ZoneBeach = true;
		}
		if (modplayer.CurrentBiome.Contains(Bid.Space)) {
			self.ZoneSkyHeight = true;

		}
		else {
			self.gravity = Player.defaultGravity;
		}
		if (ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Contains(NPCID.WallofFlesh)) {
			Main.hardMode = true;
		}
		VanillaReimplementation(self, modplayer);
	}
}
internal class RoguelikeBiomeHandle_GlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		base.EditSpawnPool(pool, spawnInfo);
	}
	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
		base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
	}
	public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY) {
		base.EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY);
	}
}
