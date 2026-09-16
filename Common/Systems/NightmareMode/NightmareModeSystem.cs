using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Consumable.Scrolls;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Roguelike.Common.Systems.NightmareMode;
internal class NightmareModeSystem : ModSystem {
	public override void Load() {
		On_NPC.NewNPC += On_NPC_NewNPC;
	}

	private int On_NPC_NewNPC(On_NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target) {
		int whoAmI = orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
		NPC npc = Main.npc[whoAmI];
		if (npc.boss || npc.friendly || npc.life <= 5 || Type == NPCID.TargetDummy || !RoguelikeWorldProperty.NightmareWorld) {
			return whoAmI;
		}
		else {
			if (source.Context != "MassSpawn") {
				if (Main.rand.NextBool(10)) {
					IEntitySource subsource = new EntitySource_Misc("MassSpawn");
					int amount = Main.rand.Next(4, 10);
					for (int i = 0; i < amount; i++) {
						NPC.NewNPC(subsource, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
					}
				}
			}
		}
		return whoAmI;
	}

	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		base.ModifyWorldGenTasks(tasks, ref totalWeight);
		if (!Main.masterMode && RoguelikeWorldProperty.NightmareWorld) {
			Main.ActiveWorldFileData.GameMode = GameModeID.Master;
		}
	}
	public int Cooldown = 0;
	public override void PostUpdateEverything() {
		if(!RoguelikeWorldProperty.NightmareWorld) {
			return;
		}
		bool AnyBossAlive = ModUtils.IsAnyVanillaBossAlive();
		if (AnyBossAlive) {
			Cooldown = ModUtils.CountDown(Cooldown);
			if (Cooldown <= 0) {
				Cooldown = ModUtils.ToSecond(Main.rand.Next(30, 40));
			}
			else {
				return;
			}
			Player player = Main.LocalPlayer;
			if (Main.hardMode) {
				ModObject.NewModObject(new EntitySource_Misc("Misc"), player.Center + Main.rand.NextVector2CircularEdge(300, 300), Vector2.Zero, ModObject.GetModObjectType<CelestialPortal>());
			}
			else {
				ModObject.NewModObject(new EntitySource_Misc("Misc"), player.Center + Main.rand.NextVector2CircularEdge(300, 300), Vector2.Zero, ModObject.GetModObjectType<HellSpawnObject>());
			}
		}
		else {
			Cooldown = 0;
		}
	}
}
public class CelestialPortal : ModObject {
	public override void SetDefaults() {
		timeLeft = ModUtils.ToSecond(20);
	}
	public override void AI() {
		float progress = timeLeft;
		Lighting.AddLight(Center, Color.Red.ToVector3());
		if (progress > ModUtils.ToSecond(19)) {
			return;
		}
		if (progress % 200 == 0) {
			for (int i = 0; i < 100; i++) {
				var dust = Dust.NewDustDirect(Center, 0, 0, DustID.DemonTorch);
				dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
				dust.noGravity = true;
				dust.scale += 1;
			}
			int NPCToSpawn = Main.rand.Next([
				NPCID.SolarCrawltipedeHead, NPCID.SolarDrakomire, NPCID.SolarDrakomireRider, NPCID.SolarGoop, NPCID.SolarSolenian, NPCID.SolarSpearman, NPCID.SolarSroller,NPCID.SolarCorite,
				NPCID.NebulaBeast, NPCID.NebulaBrain, NPCID.NebulaHeadcrab, NPCID.NebulaSoldier,
				NPCID.VortexHornet, NPCID.VortexHornetQueen, NPCID.VortexRifleman, NPCID.VortexSoldier,
				NPCID.StardustCellBig, NPCID.StardustJellyfishBig, NPCID.StardustSoldier, NPCID.StardustWormHead,
			]);
			NPC.NewNPC(new EntitySource_WorldGen(), (int)Center.X, (int)Center.Y, NPCToSpawn);
		}
	}
	public override void Draw(SpriteBatch spritebatch) {
		ModUtils.Draw_SetUpToDrawGlowAdditive(spritebatch);
		Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
		Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Center - Main.screenPosition;
		float length = origin.Length();
		for (int i = 0; i < 4; i++) {
			float rotationInner = MathHelper.PiOver2 * i + MathHelper.ToRadians(timeLeft);
			Vector2 drawPos = drawpos.PositionOFFSET(Vector2.UnitX.RotatedBy(rotationInner), length);
			Color col = Color.White;
			switch (i) {
				case 0:
					col = Color.Orange;
					break;
				case 1:
					col = Color.Cyan;
					break;
				case 2:
					col = Color.Pink;
					break;
				case 3:
					col = Color.SpringGreen;
					break;
			}
			spritebatch.Draw(texture, drawPos, null, col, rotationInner, origin, new Vector2(1, .5f), SpriteEffects.None, 0);
		}
		texture = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		origin = texture.Size() * .5f;
		spritebatch.Draw(texture, drawpos, null, Color.White with { A = 50 }, rotation, origin, 2f, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(spritebatch);
	}
}
