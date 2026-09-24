using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.Contents;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.RoguelikeMode.StructureHandler;
internal class Tower_Structure_ModSystem : ModSystem {
	public HashSet<Point> Get_PortalSpawningPoint() {
		return [ new Point(26,16), //PortalPos1
			new Point(26,26), //PortalPos2
			new Point(5,46), //PortalPos3
			new Point(5,56), //PortalPos4
			new Point(9,72), //MegaPortalPos1
			new Point(6,96), //PortalPos6
			new Point(26,117),]; //PortalPos7,
	}
	/// <summary>
	/// Keys : Structure location<br/>
	/// Values : Whenever or not if the Mod Object is loaded.
	/// </summary>
	public Dictionary<Rectangle, bool> list_StructureLocation = new();
	public int[] Monster_Pool = [
		NPCID.DemonEye,
		NPCID.Zombie,
		NPCID.FireImp,
		NPCID.BlueSlime,
		NPCID.Harpy,
		NPCID.GiantShelly,
		NPCID.GreekSkeleton,
		NPCID.GiantFlyingAntlion,
		NPCID.EaterofSouls,
		NPCID.CaveBat,
		];
	public override void PostUpdateEverything() {
		if (!RoguelikeWorldProperty.RoguelikeWorld) {
			return;
		}
		foreach (var structure in list_StructureLocation.Keys) {
			var player = Main.LocalPlayer;
			if (list_StructureLocation[structure]) {
				continue;
			}
			if (player.Center.IsCloseToPosition(structure.Center().ToWorldCoordinates(), 1500)) {
				var points = Get_PortalSpawningPoint();
				foreach (var point in points) {
					var worldPos = (structure.Location + point).ToWorldCoordinates();
					ModObject obj = ModObject.NewModObject(worldPos, Vector2.Zero, ModObject.GetModObjectType<MonsterPortalObject>());
					if (obj != null) {
						if (obj is MonsterPortalObject portal) {
							int amount = Main.rand.Next(7, 13);
							int[] newArr = new int[amount];
							for (int i = 0; i < amount; i++) {
								newArr[i] = Main.rand.Next(Monster_Pool);
							}
							portal.Add_NPCToList(newArr);
						}
					}
				}
				list_StructureLocation[structure] = true;
			}
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["TowerLocation"] = list_StructureLocation.Keys.ToList();
		tag["TowerLoadState"] = list_StructureLocation.Values.ToList();
	}
	public override void LoadWorldData(TagCompound tag) {
		var TowerLocation = tag.Get<List<Rectangle>>("TowerLocation");
		var TowerLoadState = tag.Get<List<bool>>("TowerLoadState");
		list_StructureLocation = TowerLocation.Zip(TowerLoadState, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
	}
}
