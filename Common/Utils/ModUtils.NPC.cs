using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;

namespace Roguelike.Common.Utils {
	public static partial class ModUtils {
		/// <summary>
		/// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="Position"></param>
		/// <param name="speed"></param>
		public static bool NPCMoveToPosition(this NPC npc, Vector2 Position, float speed, float roomforError = 20f) {
			Vector2 distance = Position - npc.Center;
			if (distance.Length() <= roomforError) {
				npc.velocity = Vector2.Zero;
				return true;
			}
			npc.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
			return false;
		}
		public static int NewHostileProjectile(IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int whoAmI = -1) {
			if (Main.expertMode)
				damage /= 2;
			else if (Main.masterMode)
				damage /= 3;
			if (damage < 1) {
				damage = 1;
			}
			int HostileProjectile = Projectile.NewProjectile(source, position, velocity, type, damage, knockback);

			Main.projectile[HostileProjectile].whoAmI = whoAmI;
			Main.projectile[HostileProjectile].hostile = true;
			Main.projectile[HostileProjectile].friendly = false;
			return HostileProjectile;
		}
		/// <summary>
		/// Useful for making heal type effect
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="healAmount"></param>
		/// <param name="texteffect"></param>
		public static void Heal(this NPC npc, int healAmount, bool texteffect = true) {
			int simulatehealing = npc.life + healAmount;
			if (npc.lifeMax <= simulatehealing) {
				npc.life = npc.lifeMax;
			}
			else {
				npc.life = simulatehealing;
			}
			if (texteffect) {
				CombatText.NewText(npc.Hitbox, CombatText.HealLife, healAmount);
			}
		}
		/// <summary>
		/// Useful for developer who want to do their own manual teleportation effect
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="newpos"></param>
		public static void TeleportCommon(this NPC npc, Vector2 newpos) {
			npc.position = newpos;
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 1, npc.whoAmI, newpos.X, newpos.Y);
		}
		public static int SpawnBoss(int spawnPositionX, int spawnPositionY, int Type, int targetPlayerIndex) {
			int num = 200;
			if (Type == 127 && NPC.mechQueen != -1) {
				num = (NPC.mechQueen = NPC.NewNPC(NPC.GetBossSpawnSource(targetPlayerIndex), spawnPositionX, spawnPositionY, Type, 100));
				Main.npc[NPC.mechQueen].ai[3] = NPC.mechQueen;
			}
			else {
				num = NPC.NewNPC(NPC.GetBossSpawnSource(targetPlayerIndex), spawnPositionX, spawnPositionY, Type, 1);
			}

			if (num == 200)
				return -1;

			Main.npc[num].target = targetPlayerIndex;
			Main.npc[num].timeLeft *= 20;
			string typeName = Main.npc[num].TypeName;
			if (Main.netMode == 2 && num < 200)
				NetMessage.SendData(23, -1, -1, null, num);

			if (Type == 134 || Type == 127 || Type == 126 || Type == 125)
				AchievementsHelper.CheckMechaMayhem();

			if (Type == 127 && NPC.mechQueen == num) {
				if (Main.netMode == 0)
					Main.NewText(Lang.misc[107].Value, 175, 75);
				else if (Main.netMode == 2)
					ChatHelper.BroadcastChatMessage(Lang.misc[107].ToNetworkText(), new Color(175, 75, 255));

				return num;
			}

			switch (Type) {
				case 125:
					if (Main.netMode == 0)
						Main.NewText(Lang.misc[48].Value, 175, 75);
					else if (Main.netMode == 2)
						ChatHelper.BroadcastChatMessage(Lang.misc[48].ToNetworkText(), new Color(175, 75, 255));
					return num;
				case 50:
				case 82:
				case 126:
				case 316:
				case 398:
				case 551:
				case 662:
					return num;
			}

			if (Main.netMode == 0)
				Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
			else if (Main.netMode == 2)
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[num].GetTypeNetName()), new Color(175, 75, 255));
			return num;
		}
	}
}
