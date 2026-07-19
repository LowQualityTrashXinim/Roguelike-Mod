using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Systems.ObjectSystem;
using Roguelike.Common.Systems.ObjectSystem.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Roguelike.Common.Global.Mechanic.Revive;
public class ReviveSystem : ModSystem {
	public static List<ModRevive> list_revive { get; private set; } = new();
	public static List<ModRevive> list_revive_Condition { get; private set; } = new();
	public static List<ModRevive> list_revive_Chance { get; private set; } = new();
	public static short Register(ModRevive revive) {
		ModTypeLookup<ModRevive>.Register(revive);
		list_revive.Add(revive);
		if (revive.ReviveChanceType) {
			list_revive_Chance.Add(revive);
		}
		else {
			list_revive_Condition.Add(revive);
		}
		return (short)(list_revive.Count - 1);
	}
}
public class RevivePlayer : ModPlayer {
	public bool?[] ReviveState = new bool?[] { null };
	public List<Item> listItem = new();
	public override void Initialize() {
		ReviveState = new bool?[ReviveSystem.list_revive.Count];
		Array.Fill(ReviveState, null);
	}
	public override void OnEnterWorld() {
		foreach (var item in ReviveSystem.list_revive) {
			if (ReviveState[item.Type] == null) {
				ReviveState[item.Type] = item.ReviveCondition(Player);
			}
		}
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		bool condition = false;
		foreach (var item in ReviveSystem.list_revive_Chance) {
			if (ReviveState[item.Type] != null) {
				if (ReviveState[item.Type] == true) {
					item.OnRevive(Player, damage, hitDirection, pvp, ref damageSource);
					ReviveState[item.Type] = item.ReviveCondition(Player);
					Main.NewText("Successfully revive based on chance");
					return false;
				}
				else {
					ReviveState[item.Type] = item.ReviveCondition(Player);
					Main.NewText("Rerolling the odd for revive");
				}
			}
			else {
				condition = item.ReviveCondition(Player);
				if (condition) {
					item.OnRevive(Player, damage, hitDirection, pvp, ref damageSource);
					ReviveState[item.Type] = item.ReviveCondition(Player);
					Main.NewText("Successfully revive based on chance");
					return false;
				}
				else {
					ReviveState[item.Type] = item.ReviveCondition(Player);
					Main.NewText("Rerolling the odd for revive");
				}
			}
		}
		foreach (var item in ReviveSystem.list_revive_Condition) {
			if (ReviveState[item.Type] != null) {
				if (item.ReviveCondition(Player)) {
					item.OnRevive(Player, damage, hitDirection, pvp, ref damageSource);
					ReviveState[item.Type] = false;
					Main.NewText("Successfully revive based on condition");
					return false;
				}
			}
			else {
				condition = item.ReviveCondition(Player);
				if (condition) {
					item.OnRevive(Player, damage, hitDirection, pvp, ref damageSource);
					ReviveState[item.Type] = condition;
					Main.NewText("Successfully revive based on condition");
					return false;
				}
			}
		}
		if (listItem != null && listItem.Count > 0) {
			int typeItem = listItem[0].type;

			ModObject.NewModObject(
			new EntitySource_AccessoryVisual(typeItem, Player),
			Player.Center,
			Vector2.Zero,
			ModObject.GetModObjectType<AccessoryVisualModObject>());

			listItem[0].TurnToAir();
			listItem.RemoveAt(0);
			Player.Heal(Player.statLifeMax2 / 2);
			return false;
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
public abstract class ModRevive : ModType {
	public short Type = 0;
	/// <summary>
	/// Set this to false if your <see cref="ModRevive"/> is not based on chance<bt/>
	/// Only enable this to true if it is true, this is for optimization<br/>
	/// Set this in <see cref="ModType.SetStaticDefaults"/>
	/// </summary>
	public bool ReviveChanceType = false;
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = ReviveSystem.Register(this);
	}
	/// <summary>
	/// Return true to prevent player from being killed.<br/><br/>
	/// By default this method return false.
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public virtual bool ReviveCondition(Player player) {
		return false;
	}
	public virtual void OnRevive(Player player, double damage, int hitDirection, bool pvp, ref PlayerDeathReason damageSource) {

	}
}
public class AccessoryVisualModObject : ModObject {
	public int AccType = -1;
	public int alpha = 255;
	public override void SetDefaults() {
		timeLeft = 120;
	}
	public override void OnSpawn(IEntitySource source) {
		if (source is EntitySource_AccessoryVisual visual) {
			AccType = visual.AccType;
		}
	}
	public override void AI() {
		velocity = -Vector2.UnitY * 2;
		alpha = (int)MathHelper.Lerp(0, 255, timeLeft / 120f);
	}
	public override void Draw(SpriteBatch spritebatch) {
		if (AccType < 0) {
			return;
		}
		float opacity = alpha / 255f;
		Main.instance.LoadItem(AccType);
		Texture2D texture = TextureAssets.Item[AccType].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = position - Main.screenPosition + origin;
		Color color = new Color(255, 255, 255, 0) * opacity;
		color.A = (byte)alpha;
		spritebatch.Draw(texture, drawPos, null, color, 0, origin, 1f, SpriteEffects.None, 0);
	}
}
