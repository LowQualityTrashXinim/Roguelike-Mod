using Terraria;
using System.IO;
using Terraria.ID;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Items.Consumable.Potion;
using Roguelike.Contents.Items.Consumable.SpecialReward;
using Roguelike.Contents.Transfixion.Artifacts;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ReviveSystem;
using Roguelike.Contents.Items.NoneSynergy;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike {
	partial class Roguelike {
		internal enum MessageType : byte {
			SkillIssuePlayer,
			DrugSyncPlayer,
			NoHitBossNum,
			GambleAddiction,
			GodUltimateChallenge,
			Perk,
			Skill,
			Artifact,
			PlayerStatsHandle,
			Revive,
			ReviveSync
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			MessageType msgType = (MessageType)reader.ReadByte();
			byte playernumber;
			switch (msgType) {
				case MessageType.NoHitBossNum:
					playernumber = reader.ReadByte();
					NoHitPlayerHandle nohitplayer = Main.player[playernumber].GetModPlayer<NoHitPlayerHandle>();
					nohitplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						nohitplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.SkillIssuePlayer:
					playernumber = reader.ReadByte();
					SkillIssuedArtifactPlayer SkillISsue = Main.player[playernumber].GetModPlayer<SkillIssuedArtifactPlayer>();
					SkillISsue.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						SkillISsue.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.DrugSyncPlayer:
					playernumber = reader.ReadByte();
					WonderDrugPlayer drugplayer = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
					drugplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						drugplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.GambleAddiction:
					playernumber = reader.ReadByte();
					GamblePlayer gamble = Main.player[playernumber].GetModPlayer<GamblePlayer>();
					gamble.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						gamble.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.GodUltimateChallenge:
					playernumber = reader.ReadByte();
					ModdedPlayer moddedplayer = Main.player[playernumber].GetModPlayer<ModdedPlayer>();
					moddedplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						moddedplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.Perk:
					playernumber = reader.ReadByte();
					PerkPlayer perkplayer = Main.player[playernumber].GetModPlayer<PerkPlayer>();
					perkplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						perkplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				//case MessageType.Skill:
				//	SkillHandlePlayer skillplayer = Main.player[playernumber].GetModPlayer<SkillHandlePlayer>();
				//	skillplayer.ReceivePlayerSync(reader);
				//	if (Main.netMode == NetmodeID.Server) {
				//		skillplayer.SyncPlayer(-1, whoAmI, false);
				//	}
				//	break;
				case MessageType.Artifact:
					playernumber = reader.ReadByte();
					ArtifactPlayer artifactPlayer = Main.player[playernumber].GetModPlayer<ArtifactPlayer>();
					artifactPlayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						artifactPlayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.PlayerStatsHandle:
					playernumber = reader.ReadByte();
					PlayerStatsHandle statplayer = Main.player[playernumber].GetModPlayer<PlayerStatsHandle>();
					statplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						statplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;

				case MessageType.Revive:
				case MessageType.ReviveSync: {
						// Read the player id from the packet
						byte playerID = reader.ReadByte();

						// Pass the reader to the RevivePlayer
						Main.player[playerID].GetModPlayer<RevivePlayer>().ReceiveRevivePacket(reader);

						// Check if this is a normal revive packet received on the server.
						// => clients and singleplayer don't have to forward the packet.
						// => if this is a sync of the player, vanilla will take care of netcode
						//    by calling ModPlayer.SyncPlayer again.
						if (Main.netMode != NetmodeID.Server || msgType == MessageType.ReviveSync) {
							break;
						}

						// Forward it to other clients
						Main.player[playerID].GetModPlayer<RevivePlayer>().SendRevivePacket(-1, whoAmI);
					}
					break;
			}
		}
	}
}
