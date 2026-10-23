using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.General;
using Roguelike.Common.Systems.Skill;
using Roguelike.Texture;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Roguelike.Common.Utils {
	public static partial class ModUtils {
		public static Rectangle GetSource(this Texture2D texture, int verticalFrames, int index) {
			int frameHeight = texture.Height / verticalFrames;
			return new Rectangle(0, (index % verticalFrames) * frameHeight, texture.Width, frameHeight);
		}
		public static string GetTheSameTextureAsEntity<T>() where T : class {
			var type = typeof(T);
			string NameSpace = type.Namespace;
			if (NameSpace == null) {
				return ModTexture.MissingTexture_Default;
			}
			return NameSpace.Replace(".", "/") + "/" + type.Name;
		}
		public static string GetTheSameTextureAs<T>(string altName = "") where T : class {
			var type = typeof(T);
			if (string.IsNullOrEmpty(altName)) {
				altName = type.Name;
			}
			string NameSpace = type.Namespace;
			if (NameSpace == null) {
				return ModTexture.MissingTexture_Default;
			}
			return NameSpace.Replace(".", "/") + "/" + altName;
		}
		public static float Scale_OuterTextureWithInnerTexture(Vector2 size1, Vector2 size2, float adjustment) => size1.Length() / size2.Length() * adjustment;
		public static string GetVanillaTexture<T>(int EntityType) where T : class => $"Terraria/Images/{typeof(T).Name}_{EntityType}";
		public static void Draw_SetUpToDrawGlow(SpriteBatch spriteBatch) {
			if (ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
				return;
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		}
		public static void Draw_SetUpToDrawGlowAdditive(SpriteBatch spriteBatch) {
			if (ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
				return;
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
		}
		public static void Draw_ResetToNormal(SpriteBatch spriteBatch) {
			if (ModContent.GetInstance<RogueLikeConfig>().LowerQuality) {
				return;
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
		}
		public static Asset<Texture2D> Get_SkillbackgroundTexture(int skillType) {
			ModSkill skill = SkillModSystem.GetSkill(skillType);

			if (skill == null)
				return TextureAssets.InventoryBack10;

			switch (skill.Skill_Type) {
				case SkillTypeID.Projectile:
					return TextureAssets.InventoryBack;
				case SkillTypeID.Stats:
					return TextureAssets.InventoryBack2;
				case SkillTypeID.Modify:
					return TextureAssets.InventoryBack3;
				case SkillTypeID.Empowered:
					return TextureAssets.InventoryBack15;
				case SkillTypeID.Utility:
					return TextureAssets.InventoryBack5;
				case SkillTypeID.Summon:
					return TextureAssets.InventoryBack6;
				case SkillTypeID.Bundle:
					return TextureAssets.InventoryBack7;
				default:
					return TextureAssets.InventoryBack10;
			}
		}
	}
}
