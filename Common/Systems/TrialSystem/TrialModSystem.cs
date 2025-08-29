using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Roguelike.Common.Systems.TrialSystem;

public class TrialGlobalNPC : GlobalNPC {
	public bool SpawnedByTrial = false;
	public override bool InstancePerEntity => true;
}
internal class TrialModSystem : ModSystem {
	public int TrialActivationAmount = 0;
	private static readonly List<ModTrial> _trial = new();
	public static int TotalCount => _trial.Count;
	public static int Register(ModTrial trial) {
		ModTypeLookup<ModTrial>.Register(trial);
		_trial.Add(trial);
		return _trial.Count - 1;
	}
	public static ModTrial GetTrial(int type) {
		return type >= 0 && type < _trial.Count ? _trial[type] : null;
	}
	public override void OnModLoad() {
		Trial = null;
		Trial_NPC = new List<NPC>();
	}
	public override void OnModUnload() {
		ResetTrial();
		Trial_NPC = null;
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["TrialActivationAmount"] = TrialActivationAmount;
	}
	public override void LoadWorldData(TagCompound tag) {
		TrialActivationAmount = (int)tag["TrialActivationAmount"];
	}
	/// <summary>
	/// Use this to activate a trial
	/// </summary>
	/// <param name="TrialID"></param>
	/// <param name="activatePosition"></param>
	public static void SetTrial(int TrialID, Vector2 activatePosition) {
		Trial = GetTrial(Math.Clamp(TrialID, 0, TotalCount));
		Trial_StartPos = activatePosition;
		ModContent.GetInstance<TrialModSystem>().TrialActivationAmount++;
		int halfW = Trial.Width / 2, halfH = Trial.Height / 2;
		ModContent.GetInstance<TrialModSystem>().Trial_ArenaSize = new((int)Trial_StartPos.X - halfW, (int)Trial_StartPos.Y + halfH, Trial.Width, Trial.Height);
	}
	public override void PostUpdateInvasions() {
		//Ensure trial will only start if trial data is not null
		if (Trial == null) {
			return;
		}
		//Check the trial type
		//Start trial 
		BattleTrialUpdate();
	}
	private void SpawnTrialNPC(Rectangle spawnRect) {
		List<TrialNPCSetting> listSet = Trial.SpawnTrialEnemey(spawnRect, TrialActivationAmount);
		foreach (var item in listSet) {
			NPC npc = NPC.NewNPCDirect(new EntitySource_Misc("trial"), item.SpawnPosition, item.NPCType);
			npc.GetGlobalNPC<TrialGlobalNPC>().SpawnedByTrial = true;
			Trial_NPC.Add(npc);
		}
	}
	public static ModTrial Trial = null;
	public static List<NPC> Trial_NPC = new List<NPC>();
	private static Vector2 Trial_StartPos = Vector2.Zero;
	private void ResetTrial() {
		Trial_NPC.Clear();
		Trial = null;
		Trial_StartPos = Vector2.Zero;
	}
	private void TrialNPCManage() {
		//Do a reverse iteration so that we can remove element safely
		for (int i = Trial_NPC.Count - 1; i >= 0; i--) {
			NPC npc = Trial_NPC[i];
			//Check if the NPC in the question is still active or not
			if (npc.active && npc.life > 0) {
				//Since the NPC is still alive and active, we move on
				continue;
			}
			//Since the NPC is dead, we remove them
			Trial_NPC.RemoveAt(i);
		}

		//Checking if the Trial NPC is empty, if it is not, do nothing
		if (Trial_NPC.Count < 1) {
			for (int i = 0; i < Main.player.Length; i++) {
				Player player = Main.player[i];
				if (player.dead || !player.active) {
					continue;
				}
				//Spawn reward for player
				Trial.TrialReward(player.GetSource_Misc("trial_reward"), player);
			}
			ResetTrial();
		}
	}
	private Rectangle Trial_ArenaSize = new();
	private void BattleTrialUpdate() {
		//Start trial
		if (Trial == null) {
			return;
		}
		else {
			SpawnTrialNPC(Trial_ArenaSize);
		}
		TrialNPCManage();
	}
}
public abstract class ModTrial : ModType {
	public string TrialRoom = null;
	public int Width, Height;
	/// <summary>
	/// Set this in <see cref="ModType.SetStaticDefaults"/> to set structure file to go along with it
	/// </summary>
	public string FilePath = string.Empty;
	public int Type { get; private set; }
	public static int GetTrialType<T>() where T : ModTrial {
		return ModContent.GetInstance<T>().Type;
	}
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = TrialModSystem.Register(this);
	}
	public virtual List<TrialNPCSetting> SpawnTrialEnemey(Rectangle trialSize, int progression) => null;

	public virtual void TrialReward(IEntitySource source, Player player) { }
}
public class TrialNPCSetting {
	public int NPCType = -1;
	public Vector2 SpawnPosition = Vector2.Zero;
}
