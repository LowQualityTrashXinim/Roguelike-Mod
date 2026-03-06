using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Skill;
using Roguelike.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
public abstract class GlobalFoodItem : GlobalItem {
	public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return (entity.buffType == BuffID.WellFed || entity.buffType == BuffID.WellFed2 || entity.buffType == BuffID.WellFed3) && entity.type == AppliesToFoodType();
	}
	public virtual int AppliesToFoodType() => ItemID.None;
	public virtual int CoolDownBetweenUse() => 0;
	public void SetBuff(Item item, int type, int time) {
		item.buffType = type;
		item.buffTime = time;
	}
	public GlobalFoodItem_ModPlayer Player_FoodPlayer(Player player) => player.GetModPlayer<GlobalFoodItem_ModPlayer>();
	public SkillHandlePlayer Player_SkillPlayer(Player player) => player.GetModPlayer<SkillHandlePlayer>();
	public sealed override void SetDefaults(Item entity) {
		SetFoodDefaults(entity);
	}
	public virtual void SetFoodDefaults(Item item) {

	}
}
public abstract class BaseFoodBuff : ModBuff {
	public virtual int TypeID => -1;
	public override string Texture => ModTexture.EMPTYBUFF;
	public sealed override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
		if (TypeID != -1) {
			if(!DenyDrawCondintion()) {
				return;
			}
			Main.instance.LoadItem(TypeID);
			Texture2D bgsTexture = drawParams.Texture;
			Vector2 size = bgsTexture.Size();
			Texture2D texture = TextureAssets.Item[TypeID].Value;
			Vector2 size2 = texture.Size();
			Vector2 size3 = new Vector2(size2.X, size2.Y / 3);
			Vector2 drawpos = drawParams.Position + size * .5f;
			spriteBatch.Draw(texture, drawpos, texture.Frame(1, 3), drawParams.DrawColor, 0, size3 * .5f, ModUtils.Scale_OuterTextureWithInnerTexture(size, size3, .67f), SpriteEffects.None, 0);
		}
	}
	public virtual bool DenyDrawCondintion() {
		return true;
	}
}
public abstract class FoodItemTier1 : BaseFoodBuff {
}
public abstract class FoodItemTier2 : BaseFoodBuff {
}
public abstract class FoodItemTier3 : BaseFoodBuff {
}
public class GlobalFoodItem_ModPlayer : ModPlayer {
	public int T1foodID = -1;
	public int T2foodID = -1;
	public int T3foodID = -1;
	/// <summary>
	/// Upon setting the buff type and tier, will also attempt to delete the existing tier buff
	/// </summary>
	/// <param name="type"></param>
	/// <param name="tier"></param>
	public void SetFoodBuff(int type, int tier) {
		switch (tier) {
			case 0:
				if (T1foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier1>(T1foodID);
					T1foodID = type;
				}
				break;
			case 1:
				if (T2foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier2>(T2foodID);
					T2foodID = type;
				}
				break;
			case 2:
				if (T3foodID != type) {
					AttemptToDeleteExistingFoodBuff<FoodItemTier3>(T3foodID);
					T3foodID = type;
				}
				break;

		}
	}
	private void AttemptToDeleteExistingFoodBuff<T>(int typeBefore) where T : class {
		for (int i = 0; i < Player.buffType.Length; i++) {
			int bufftype = Player.buffType[i];
			var modBuff = ModContent.GetModBuff(bufftype);
			if (modBuff == null) {
				continue;
			}
			if (modBuff is not T) {
				continue;
			}
			var buff = (BaseFoodBuff)modBuff;
			if (buff.TypeID == typeBefore) {
				Player.DelBuff(i);
				i--;
			}
		}
	}
}
