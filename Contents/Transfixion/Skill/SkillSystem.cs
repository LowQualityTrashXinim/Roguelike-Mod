using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Roguelike.Common.Global;
using Roguelike.Common.Systems;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Roguelike.Contents.Transfixion.Skill;
public static class SkillTypeID {
	public const byte Skill_None = 0;
	/// <summary>
	/// If your skill is projectile type, then the skill will work differently compared to other skill<br/>
	/// - Energy require now a constant cost, if your energy requirement is met then the projectile skill is activated<br/>
	/// - Cool down now used as cool down between shot for the projectile skill bundle.<br/>
	/// - Duration work the same as other skill and will add on to universal duration.
	/// </summary>
	public const byte Projectile = 1;
	public const byte Stats = 2;
	public const byte Summon = 3;
	public const byte Empowered = 4;
	public const byte Bundle = 5;
	public const byte Utility = 6;
	public const byte Modify = 7;
}
public abstract class ModSkill : ModType {
	public static int GetSkillType<T>() where T : ModSkill {
		return ModContent.GetInstance<T>().Type;
	}
	public int Type { get; private set; }
	public byte Skill_Type { get; protected set; }
	/// <summary>
	/// For skill which are <see cref="SkillTypeID.Skill_Projectile"/><br/>
	/// Uses to determined how fast a projectile can automatically shoot
	/// </summary>
	protected int Skill_CoolDown = 0;
	public int Cooldown { get => Skill_CoolDown; }
	/// <summary>
	/// This is handle automatically so no need to worry about doing it yourself
	/// </summary>
	protected int Skill_Duration = 0;
	public int Duration { get => Skill_Duration; }
	protected int Skill_EnergyRequire = 0;
	public int EnergyRequire { get => Skill_EnergyRequire; }
	protected float Skill_EnergyRequirePercentage = 0;
	public float EnergyRequirePercentage { get => Skill_EnergyRequirePercentage; }
	protected int Skill_ShootType = 0;
	public int ShootType { get => Skill_ShootType; }
	protected int Skill_Damage = 0;
	public int Damage { get => Skill_Damage; }
	protected float Skill_KnockBack = 0;
	public float Knockback { get => Skill_KnockBack; }
	public bool CanBeSelect { get => Skill_CanBeSelect; }

	protected bool Skill_CanBeSelect = true;
	public virtual string Texture => ModTexture.Get_MissingTexture("Skill");
	public string DisplayName => ModUtils.LocalizationText("ModSkill", $"{Name}.DisplayName");
	public string Description => ModUtils.LocalizationText("ModSkill", $"{Name}.Description");
	public ModSkill() {
		SetDefault();
	}
	protected sealed override void Register() {
		Type = SkillModSystem.Register(this);
	}
	/// <summary>
	/// This have the same functionality of <see cref="ModifySkillSet(Player, SkillHandlePlayer, ref int, ref StatModifier, ref StatModifier, ref StatModifier)"/><br/>
	/// But simpified, use this over ModifySkillSet if you are doing less logic
	/// </summary>
	/// <param name="energy"></param>
	/// <param name="duration"></param>
	/// <param name="cooldown"></param>
	public virtual void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration) {
		energy = new();
		duration = new();
	}
	/// <summary>
	/// Use this to set skill stat
	/// </summary>
	public virtual void SetDefault() { }
	/// <summary>
	/// Use this if you are modifying the skill set in a way<br/>
	/// This can be use to either skip a skill directly or loop but not recommend for looping as there are no support for that<br/>
	/// You can also directly modify energy cost, duration and cool down of the next skill
	/// </summary>
	/// <param name="player"></param>
	/// <param name="modplayer"></param>
	/// <param name="index"></param>
	public virtual void ModifySkillSet(Player player, SkillHandlePlayer modplayer, ref int index, ref StatModifier energy, ref StatModifier duration) { }
	/// <summary>
	/// Called upon player activate the skill when the skill requirement is fullfilled<br/>
	/// This is called before cool down, duration of the skill and energy subtraction is set
	/// </summary>
	/// <param name="player"></param>
	public virtual void OnTrigger(Player player, SkillHandlePlayer skillplayer, int duration, int energy) { }
	public virtual void ModifyShootProjectile(Player player, SkillHandlePlayer skillplayer, ref int amount, ref List<Vector2> position, ref List<Vector2> velocity) { }
	/// <summary>
	/// This only run when the duration of the skill is equal or below 1
	/// </summary>
	/// <param name="player"></param>
	public virtual void OnEnded(Player player) { }
	public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
	public virtual void ResetEffect(Player player) { }
	public virtual void Update(Player player, SkillHandlePlayer skillplayer) { }
	/// <summary>
	/// This will always update the skill, it work like UpdateEquip
	/// </summary>
	/// <param name="player"></param>
	/// <param name="skillplayer"></param>
	public virtual void AlwaysUpdate(Player player, SkillHandlePlayer skillplayer) { }
	public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
	public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitByAnything(Player player) { }
	public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
	public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
	public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
	public static int SkillDamage(Player player, int damage) {
		var skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		return (int)Math.Ceiling(skillplayer.skilldamage.ApplyTo(damage));
	}
	/// <summary>
	/// This run before skill get added, return false if you don't want to add this skill into active chain<br/>
	/// Otherwise return true
	/// </summary>
	/// <param name="player"></param>
	/// <param name="skillplayer"></param>
	/// <param name="activeskill"></param>
	/// <param name="currentindex"></param>
	public virtual bool OnAddSkill(Player player, SkillHandlePlayer skillplayer, int[] currentSkill, ref List<ModSkill> activeskill, int currentindex, ref int energy, ref int duration) {
		return true;
	}
}
public class SkillModSystem : ModSystem {
	private static List<ModSkill> _skill = new();
	public static Dictionary<byte, List<ModSkill>> dict_skill { get; private set; } = new();
	public static int TotalCount => _skill.Count;
	public static int Register(ModSkill skill) {
		ModTypeLookup<ModSkill>.Register(skill);
		_skill.Add(skill);
		if (dict_skill.ContainsKey(skill.Skill_Type)) {
			dict_skill[skill.Skill_Type].Add(skill);
		}
		else {
			dict_skill.Add(skill.Skill_Type, new() { skill });
		}
		//BossRush.Instance.Logger.Info($"Added skill :{_skill[_skill.Count - 1].Name}");
		return _skill.Count - 1;
	}
	public static ModSkill GetSkill(int type) {
		return type >= 0 && type < _skill.Count ? _skill[type] : null;
	}
	public static ModKeybind SkillActivation { get; private set; }

	public override void Load() {
		SkillActivation = KeybindLoader.RegisterKeybind(Mod, "Skill activation", Keys.F);
	}
	public override void Unload() {
		SkillActivation = null;
		_skill = null;
		dict_skill = null;
	}
}
public class SkillLoadOut {
	public SkillLoadOut() {

	}
	public List<int> list_SkillLoadOut = new List<int>();
	public string Name = "Load Out";
}
public class SkillHandlePlayer : ModPlayer {
	public float ProjectileSpeedMultiplier = 1;
	public int ProjectileCritChance = 0;
	public int ProjectileEnergyRegain = 0;
	public float ProjectileCritDamage = 0;
	public int ProjectileTimeLeft = 0;
	public float ProjectileSpreadAmount = 5;
	public float ProjectileSpreadMultiplier = 1;
	/// <summary>
	/// This method automatically handle skill damage, critical chance, energy regain and anything related<br/>
	/// Uses this method over <see cref="Projectile.NewProjectile(IEntitySource, float, float, float, float, int, int, float, int, float, float, float)"/> or any that is related when making skill shoot projectile.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="position"></param>
	/// <param name="velNormalize"></param>
	/// <param name="speed"></param>
	/// <param name="type"></param>
	/// <param name="damage"></param>
	/// <param name="knockback"></param>
	/// <returns></returns>
	public Projectile NewSkillProjectile(IEntitySource source, Vector2 position, Vector2 velNormalize, float speed, int type, int damage, float knockback) {
		speed *= ProjectileSpeedMultiplier;

		Vector2 vel;

		float modifierSpread = ProjectileSpreadAmount * ProjectileSpreadMultiplier;
		vel = (velNormalize * speed).Vector2RotateByRandom(modifierSpread);

		var projectile = Projectile.NewProjectileDirect(source, position, vel, type, SkillDamage(damage), knockback, Player.whoAmI);
		projectile.CritChance += ProjectileCritChance;
		var roguelikeProj = projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>();
		roguelikeProj.CritDamage += ProjectileCritDamage;
		roguelikeProj.EnergyRegainOnHit += ProjectileEnergyRegain;
		projectile.timeLeft += ProjectileTimeLeft;

		return projectile;
	}
	public int SkillDamage(int damage) {
		return (int)Math.Ceiling(skilldamage.ApplyTo(damage));
	}
	public StatModifier skilldamage = new StatModifier();
	public int EnergyCap = 1500;
	public int Energy { get; private set; }
	public int Duration { get; private set; }
	public byte AvailableSkillActiveSlot = 3;
	public List<int> ActiveSkill = new();
	public Dictionary<int, int> SkillInventory = new();
	/// <summary>
	/// <b>Return : </b>true when skill is activated
	/// </summary>
	public bool Activate = false;
	public int Duplicate = 0;
	int RechargeDelay = 0;
	public int MaximumDuration = 0;
	public int BloodToPower = 0;
	public int Request_Repeat = 0;
	List<ModSkill> activeskill = new();
	List<List<ModSkill>> projectileskillActive = new();
	List<int> ProjectileShootCoolDown = new();
	public int AlwaysCostEnergy = 0;
	public int Skill_DirectionPlayerFaceBeforeSkillActivation = -1;
	public Vector2 Skill_PlayerLastPositionBeforeSkillActivation = Vector2.Zero;
	public override void OnEnterWorld() {
		activeskill = new();
		projectileskillActive = new();
		projectileskillActive = new();
		ProjectileShootCoolDown = new();
		Energy = 0;
		Duration = 0;
		AlwaysCostEnergy = 0;
	}
	public override void Initialize() {
		ActiveSkill = new();
		SkillInventory = new();
		AvailableSkillActiveSlot = 3;
		Activate = false;
		Energy = 0;
		Duration = 0;
		RechargeDelay = 0;
		Rechargebucket = 0;
		CurrentbucketAmount = 0;
		activeskill = new();
		projectileskillActive = new();
		ProjectileShootCoolDown = new();
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns>
	/// Return true when successfully increases skill slot
	/// Return false when skill slot can't no longer be increased
	/// </returns>
	public bool IncreasesSkillSlot() {
		AvailableSkillActiveSlot++;
		return true;
	}
	public bool RequestAddSkill_Inventory(int skillType, bool OnRandomizeChoose = true) {
		if (skillType < 0 && skillType >= SkillModSystem.TotalCount) {
			return false;
		}
		if (!SkillModSystem.GetSkill(skillType).CanBeSelect) {
			if (OnRandomizeChoose) {
				skillType = Main.rand.Next(SkillModSystem.TotalCount);
			}
			else {
				if (SkillInventory.ContainsKey(skillType)) {
					SkillInventory[skillType]++;
				}
				else {
					SkillInventory.Add(skillType, 1);
				}
				ModContent.GetInstance<UniversalSystem>().skillUIstate.Add_SkillToInventory(skillType);
				ModUtils.CombatTextRevamp(Player.Hitbox, Color.Aqua, $"Added skill : {SkillModSystem.GetSkill(skillType).DisplayName}");
				return true;
			}
		}
		if (SkillInventory.ContainsKey(skillType)) {
			SkillInventory[skillType]++;
		}
		else {
			SkillInventory.Add(skillType, 1);
		}
		ModContent.GetInstance<UniversalSystem>().skillUIstate.Add_SkillToInventory(skillType);
		ModUtils.CombatTextRevamp(Player.Hitbox, Color.Aqua, "Added a skill");
		return true;
	}
	public int SimulateSkillCost() {
		int energy = 0;
		int[] active = ActiveSkill.ToArray();
		float percentageEnergy = 1;
		StatModifier energyS = new(), durationS = new();
		int seperateEnergy = 0;
		for (int i = 0; i < active.Length; i++) {
			var skill = SkillModSystem.GetSkill(active[i]);
			if (skill == null) {
				continue;
			}
			if (skill.Type == ModSkill.GetSkillType<PowerSaver>()) {
				seperateEnergy += skill.EnergyRequire;
			}
			else {
				energy += (int)energyS.ApplyTo(skill.EnergyRequire);
			}
			percentageEnergy *= (1 + skill.EnergyRequirePercentage); ;
			skill.ModifyNextSkillStats(out energyS, out durationS);
			skill.ModifySkillSet(Player, this, ref i, ref energyS, ref durationS);
		}
		return (int)(energy * percentageEnergy) + seperateEnergy;
	}
	public void SkillStatTotal(bool simulated, out int energy, out int duration) {
		int[] active = ActiveSkill.ToArray();
		energy = 0;
		duration = 0;
		float percentageEnergy = 1;
		StatModifier energyS = new(), durationS = new();
		int seperateEnergy = 0;
		for (int i = 0; i < active.Length; i++) {
			var skill = SkillModSystem.GetSkill(active[i]);
			if (skill == null) {
				continue;
			}
			if (skill.Type == ModSkill.GetSkillType<PowerSaver>()) {
				seperateEnergy += skill.EnergyRequire;
			}
			else {
				energy += (int)energyS.ApplyTo(skill.EnergyRequire);
			}
			duration += (int)durationS.ApplyTo(skill.Duration);
			percentageEnergy *= (1 + skill.EnergyRequirePercentage);
			skill.ModifyNextSkillStats(out energyS, out durationS);
			skill.ModifySkillSet(Player, this, ref i, ref energyS, ref durationS);
			if (!simulated) {
				if (skill.OnAddSkill(Player, this, active, ref activeskill, i, ref energy, ref duration)) {
					activeskill.Add(skill);
				}
			}
		}
		var modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		duration = (int)modplayer.SkillDuration.ApplyTo(duration);
		energy = (int)(energy * percentageEnergy) + seperateEnergy;
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (SkillModSystem.SkillActivation.JustReleased && !Activate) {
			Activate = true;
			SkillStatTotal(false, out int energy, out int duration);
			Duration += duration;
			MaximumDuration = 0;
			if (energy > Energy) {
				ModUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "Not Enough energy !");
				Duration = 0;
				Activate = false;
			}
			else {
				Skill_DirectionPlayerFaceBeforeSkillActivation = Player.direction;
				Skill_PlayerLastPositionBeforeSkillActivation = Player.Center;
				ProjectileShootCoolDown.Clear();
				projectileskillActive.Clear();
				foreach (var item in activeskill) {
					item.OnTrigger(Player, this, duration, energy);
					if (item.Skill_Type == SkillTypeID.Modify) {
						if (projectileskillActive.Count < 1) {
							projectileskillActive.Add(new() { item });
							ProjectileShootCoolDown.Add(0);
						}
						else {
							projectileskillActive[projectileskillActive.Count - 1].Add(item);
						}

					}
					else if (item.Skill_Type == SkillTypeID.Projectile) {
						if (projectileskillActive.Count < 1) {
							projectileskillActive.Add(new() { item });
							ProjectileShootCoolDown.Add(0);
						}
						else {
							projectileskillActive[projectileskillActive.Count - 1].Add(item);
							projectileskillActive.Add(new());
							ProjectileShootCoolDown.Add(0);
						}
					}
				}
				MaximumDuration += Duration;
				Energy -= energy;
			}
		}
	}
	/// <summary>
	/// This is a direct way to modify energy amount
	/// </summary>
	/// <param name="amount"></param>
	public void Modify_EnergyAmount(int amount) {
		Energy = Math.Clamp(Energy + amount, 0, EnergyCap);
	}
	public override void PreUpdate() {
		RechargeDelay = ModUtils.CountDown(RechargeDelay);
		if (Duration <= 0) {
			Activate = false;
			activeskill.Clear();
		}
		else {
			Duration = ModUtils.CountDown(Duration);
			if (Duration <= 1) {
				foreach (var skill in activeskill) {
					skill.OnEnded(Player);
				}
			}
		}

	}
	public override void ResetEffects() {
		ProjectileCritChance = 0;
		ProjectileCritDamage = 0;
		ProjectileEnergyRegain = 0;
		ProjectileSpeedMultiplier = 1;
		ProjectileSpreadMultiplier = 1;
		ProjectileSpreadAmount = 5;
		skilldamage = StatModifier.Default;
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.ResetEffect(Player);
		}
	}
	public override void UpdateEquips() {
		int[] skillHolder = ActiveSkill.ToArray();
		for (int i = 0; i < skillHolder.Length; i++) {
			var skill = SkillModSystem.GetSkill(skillHolder[i]);
			if (skill == null) {
				continue;
			}
			skill.AlwaysUpdate(Player, this);
		}
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.Update(Player, this);
		}
		if (Energy <= 0) {
			return;
		}
		Vector2 alwaysShoot = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 5;
		IEntitySource source = Player.GetSource_Misc("Skill");
		for (int a = 0; a < projectileskillActive.Count; a++) {
			if (--ProjectileShootCoolDown[a] > 0) {
				continue;
			}
			List<ModSkill> skilllist = projectileskillActive[a];
			int amount = 1;
			List<Vector2> poslist = new() { Player.Center };
			List<Vector2> vellist = new() { alwaysShoot };
			foreach (var item in skilllist) {
				item.ModifyShootProjectile(Player, this, ref amount, ref poslist, ref vellist);
				if (item.Skill_Type == SkillTypeID.Projectile) {
					if (item.EnergyRequire >= Energy) {
						Energy = 0;
					}
					else if (Energy >= item.EnergyRequire) {
						Energy -= item.EnergyRequire;
					}
					for (int i = 0; i < amount; i++) {
						if (i >= poslist.Count) {
							continue;
						}
						NewSkillProjectile(source, poslist[i], vellist[i], vellist[i].Length(), item.ShootType, item.Damage, item.Knockback);
					}
					ProjectileShootCoolDown[a] = item.Cooldown;
				}
			}

		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!Activate) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		foreach (var skill in activeskill) {
			skill.Shoot(Player, item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.OnMissingMana(Player, item, neededMana);
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.ModifyHitNPCWithItem(Player, item, target, ref modifiers);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.OnHitNPCWithItem(Player, item, target, hit, damageDone);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		foreach (var skill in activeskill) {
			skill.OnHitNPCWithProj(Player, proj, target, hit, damageDone);
		}
	}
	int Rechargebucket = 0;
	int CurrentbucketAmount = 0;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		int eGain = (int)Math.Ceiling(modplayer.EnergyRecharge.ApplyTo(MathF.Ceiling(hit.Damage * .01f)));
		if (Rechargebucket == 0) {
			Rechargebucket = (int)Math.Ceiling(modplayer.RechargeEnergyCap.ApplyTo(hit.Damage));
		}
		CurrentbucketAmount += eGain;
		if (CurrentbucketAmount >= Rechargebucket) {
			CurrentbucketAmount = 0;
			Rechargebucket = 0;
			if (RechargeDelay <= 0) {
				eGain += hit.Damage;
				RechargeDelay = (int)(60 + 60 * hit.Damage * .01f);
			}
		}
		Energy = Math.Clamp(eGain + Energy, 0, EnergyCap);
	}
	public override void UpdateDead() {
		Activate = false;
		Energy = 0;
		Duration = 0;
		RechargeDelay = 0;
		Rechargebucket = 0;
		CurrentbucketAmount = 0;
		activeskill.Clear();
		projectileskillActive.Clear();
		ProjectileShootCoolDown.Clear();
		AlwaysCostEnergy = 0;
	}
	public override void SaveData(TagCompound tag) {
		tag.Add("ActiveSkill", ActiveSkill);
		tag["SkillStorage"] = SkillInventory.Keys.ToList();
		tag["SkillStack"] = SkillInventory.Values.ToList();
		tag.Add("AvailableSkillActiveSlot", AvailableSkillActiveSlot);
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("ActiveSkill", out List<int> activeskill)) {
			ActiveSkill = activeskill;
		}
		List<int> storage = new(), stack = new();
		if (tag.TryGet("SkillStorage", out List<int> Storage)) {
			storage = Storage;
		}
		if (tag.TryGet("SkillStack", out List<int> Stack)) {
			stack = Stack;
		}
		SkillInventory = storage.Zip(stack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		if (tag.TryGet("AvailableSkillActiveSlot", out byte AvailableSkillActiveSlot)) {
			this.AvailableSkillActiveSlot = AvailableSkillActiveSlot;
		}
	}
	//public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
	//	var packet = Mod.GetPacket();
	//	packet.Write((byte)Roguelike.MessageType.Skill);
	//	packet.Write((byte)Player.whoAmI);
	//	packet.Write(SkillInventory.Length);
	//	foreach (int item in SkillInventory) {
	//		packet.Write(item);
	//	}
	//	packet.Write(SkillHolder1.Length);
	//	foreach (int item in SkillHolder1) {
	//		packet.Write(item);
	//	}
	//	foreach (int item in SkillHolder2) {
	//		packet.Write(item);
	//	}
	//	foreach (int item in SkillHolder3) {
	//		packet.Write(item);
	//	}
	//	packet.Write(AvailableSkillActiveSlot);
	//	packet.Send(toWho, fromWho);
	//}
	//public void ReceivePlayerSync(BinaryReader reader) {
	//	Array.Fill(SkillInventory, -1);
	//	Array.Fill(SkillHolder1, -1);
	//	Array.Fill(SkillHolder2, -1);
	//	Array.Fill(SkillHolder3, -1);
	//	int countInventory = reader.ReadInt32();
	//	for (int i = 0; i < countInventory; i++) {
	//		SkillInventory[i] = reader.ReadInt32();
	//	}
	//	int countHolder = reader.ReadInt32();
	//	for (int i = 0; i < countHolder; i++)
	//		SkillHolder1[i] = reader.ReadInt32();
	//	for (int i = 0; i < countHolder; i++)
	//		SkillHolder2[i] = reader.ReadInt32();
	//	for (int i = 0; i < countHolder; i++)
	//		SkillHolder3[i] = reader.ReadInt32();
	//	AvailableSkillActiveSlot = reader.ReadByte();
	//}

	//public override void CopyClientState(ModPlayer targetCopy) {
	//	var clone = (SkillHandlePlayer)targetCopy;
	//	clone.SkillInventory = SkillInventory;
	//	clone.SkillHolder1 = SkillHolder1;
	//	clone.SkillHolder2 = SkillHolder2;
	//	clone.SkillHolder3 = SkillHolder3;
	//	clone.AvailableSkillActiveSlot = AvailableSkillActiveSlot;
	//}

	//public override void SendClientChanges(ModPlayer clientPlayer) {
	//	var clone = (SkillHandlePlayer)clientPlayer;
	//	if (SkillInventory != clone.SkillInventory
	//		|| SkillHolder1 != clone.SkillHolder1
	//		|| SkillHolder2 != clone.SkillHolder2
	//		|| SkillHolder3 != clone.SkillHolder3
	//		) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
	//}
}
public class SkillOrb : ModItem {
	public override void SetStaticDefaults() {
		ItemID.Sets.AnimatesAsSoul[Type] = true;
		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(3, 10));
	}
	public override void SetDefaults() {
		Item.width = Item.height = 50;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.autoReuse = false;
		Item.noUseGraphic = true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
		}
		return base.UseItem(player);
	}
}
