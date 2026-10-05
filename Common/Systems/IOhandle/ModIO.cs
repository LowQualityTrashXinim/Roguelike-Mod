using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode.StructureHandler;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Contents.Items.Weapon;
using Stubble.Core.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Roguelike.Common.Systems.IOhandle;
public static class RoguelikeData {
	public static int Lootbox_AmountOpen = 0;
	public static int Run_Amount = 0;
	//currently there are no winning goal so this is not implemented
	public static int Win_Streak = 0;
	public static int Win_StreakRecord = 0;
	//This however can be implemented
	public static int Lose_Streak = 0;
	public static int Lose_StreakRecord = 0;
	/// <summary>
	/// Key : The inner name of the synergy weapon<br/>
	/// Value : Synergy bonus value<br>
	/// <br/>
	/// <b></b>
	/// </summary>
	public static Dictionary<string, List<SynergyBonus>> SynergyProgressTracker = new();
	/// <summary>
	/// Key : item ID<br/>
	/// Value : Has player seen at least once<br/>
	/// 
	/// <b>True:</b> if player seen at least once<br/>
	/// <b>False:</b> if player never seen it at all
	/// </summary>
	public static Dictionary<int, bool> EnchantmentProgressTracker = new();
	/// <summary>
	/// Keys : Item name + Mod<br/>
	/// Value : Has player seen at least once<br/>
	/// </summary>
	public static Dictionary<string, bool> WeaponProgressTracker = new();
}
class ModIO : ModSystem {
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "Everlasting_Data");
	private static string DataFilePath => Path.Join(DirectoryPath, "Data");
	private static string AchievementFilePath => Path.Join(DirectoryPath, "Achievements");
	public override void Load() {
		RoguelikeData.SynergyProgressTracker = new();
		RoguelikeData.EnchantmentProgressTracker = new();

		foreach (var type in Mod.Code.GetTypes()) {
			if (!type.IsAbstract) {
				if (type.IsAssignableTo(typeof(RoguelikeAchievement))) {
					var achievement = (RoguelikeAchievement)Activator.CreateInstance(type);
					AchievementSystem.Achievements.Add(achievement);
				}
				else if (type.IsAssignableTo(typeof(ModStructure))) {
					var structure = (ModStructure)Activator.CreateInstance(type);
					ModStructure_System.structures.Add(structure);
				}
			}
		}
		try {
			if (File.Exists(DataFilePath)) {
				var tag = TagIO.FromFile(DataFilePath);
				var type = typeof(RoguelikeData);
				var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
				foreach (var field in fields) {
					if (field.Name == "WeaponProgressTracker") {
						object obj = field.GetValue(null);
						if (obj is Dictionary<string, bool> tracker) {
							var keyObj = tag.Get<List<string>>("WeaponProgressTracker_Key");
							var valueObj = tag.Get<List<bool>>("WeaponProgressTracker_Value");
							tracker = keyObj.Zip(valueObj, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
							field.SetValue(null, tracker);
						}
						continue;
					}
					if (field.Name == "SynergyProgressTracker") {
						object obj = field.GetValue(null);
						if (obj is Dictionary<string, List<SynergyBonus>> tracker) {
							var keyObj = tag.Get<List<string>>("SynergyProgressTracker_Key");
							var valueObj = tag.Get<List<List<SynergyBonus>>>("SynergyProgressTracker_Value");
							tracker = keyObj.Zip(valueObj, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
							field.SetValue(null, tracker);
						}
						continue;
					}
					if (field.Name == "EnchantmentProgressTracker") {
						continue;
					}
					field.SetValue(null, tag[field.Name]);
				}
			}
			var v = RoguelikeData.WeaponProgressTracker;
			if (File.Exists(AchievementFilePath)) {
				var tag = TagIO.FromFile(AchievementFilePath);
				foreach (var achievement in AchievementSystem.Achievements) {
					if (tag.TryGet(achievement.Name, out bool value)) {
						achievement.Achieved = value;
					}
					else {
						achievement.Achieved = false;
					}
				}
			}
			else {
				File.Create(AchievementFilePath);
				foreach (var achievement in AchievementSystem.Achievements) {
					achievement.Achieved = false;
				}
			}
		}
		catch {

		}
		On_Main.Main_Exiting += On_Main_Main_Exiting;
	}
	public override void Unload() {
		SavingModData();
		RoguelikeData.SynergyProgressTracker = null;
		RoguelikeData.EnchantmentProgressTracker = null;
	}
	private void On_Main_Main_Exiting(On_Main.orig_Main_Exiting orig, Main self, object sender, EventArgs e) {
		SavingModData();
		orig(self, sender, e);
	}
	public void SavingModData() {
		if (!File.Exists(DataFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}
			File.Create(DataFilePath);
		}
		try {
			TagCompound tag = new();
			var type = typeof(RoguelikeData);
			var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var field in fields) {
				if (field.Name == "SynergyProgressTracker") {
					tag.Set(field.Name + "_Key", RoguelikeData.SynergyProgressTracker.Keys.ToList());
					tag.Set(field.Name + "_Value", RoguelikeData.SynergyProgressTracker.Values.ToList());
				}
				else if (field.Name == "WeaponProgressTracker") {
					tag.Set(field.Name + "_Key", RoguelikeData.WeaponProgressTracker.Keys.ToList());
					tag.Set(field.Name + "_Value", RoguelikeData.WeaponProgressTracker.Values.ToList());
				}
				else if (field.Name == "EnchantmentProgressTracker") {
					//tag.Set(field.Name + "_Key", RoguelikeData.WeaponProgressTracker.Keys.ToList());
					//tag.Set(field.Name + "_Value", RoguelikeData.WeaponProgressTracker.Values.ToList());
				}
				else {
					object objValue = field.GetValue(null);
					tag.Set(field.Name, field.GetValue(null));
				}
			}
			TagIO.ToFile(tag, DataFilePath);
		}
		catch {

		}

		var achievementTag = new TagCompound();
		foreach (var achievement in AchievementSystem.Achievements) {
			achievementTag.Set(achievement.Name, achievement.Achieved);
		}
		if (!File.Exists(AchievementFilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}

			File.Create(AchievementFilePath);
		}
		try {
			TagIO.ToFile(achievementTag, AchievementFilePath);
		}
		catch {

		}
	}
}
