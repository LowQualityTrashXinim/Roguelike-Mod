using Microsoft.Xna.Framework;
using Roguelike.Common.Subworlds;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles {
	public abstract class Portal : ModProjectile {

		public sealed override void SetStaticDefaults() {
			ProjectileID.Sets.IsInteractable[Type] = true;
		}
		public virtual void Subworld_ToEnter() {

		}
		public sealed override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.FlyingPiggyBank);
			Projectile.aiStyle = -1;
			Projectile.hide = false;
		}

		public sealed override void AI() {
			if (++Projectile.frameCounter % 2 == 0)
				if (++Projectile.frame == 30)
					Projectile.frame = 0;
			if (!Projectile.Center.IsCloseToPosition(Main.player[Projectile.owner].Center, 2000)) {
				Projectile.Kill();
			}
		}

		public sealed override bool PreDraw(ref Color lightColor) {
			return false;
		}

		public sealed override void PostDraw(Color lightColor) {
			TryInteracting();
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, new Rectangle(256 * Projectile.frame, 0, 256, 256), Color.White, 0, new Vector2(128, 128), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
		}

		private void TryInteracting() {
			if (Main.gamePaused || Main.gameMenu) {
				return;
			}

			bool cursorHighlights = Main.SmartCursorIsUsed || PlayerInput.UsingGamepad;
			var localPlayer = Main.LocalPlayer;
			var compareSpot = localPlayer.Center;
			if (!localPlayer.IsProjectileInteractibleAndInInteractionRange(Projectile, ref compareSpot)) {
				return;
			}

			var matrix = Matrix.Invert(Main.GameViewMatrix.ZoomMatrix);
			var position = Main.ReverseGravitySupport(Main.MouseScreen);
			Vector2.Transform(Main.screenPosition, matrix);
			var realMouseWorld = Vector2.Transform(position, matrix) + Main.screenPosition;

			bool mouseDirectlyOver = Projectile.Hitbox.Contains(realMouseWorld.ToPoint());
			bool interactingWithThisProjectile = mouseDirectlyOver || Main.SmartInteractProj == Projectile.whoAmI;
			if (!interactingWithThisProjectile || localPlayer.lastMouseInterface) {
				return;
			}

			Main.HasInteractibleObjectThatIsNotATile = true;
			if (mouseDirectlyOver) {
				localPlayer.noThrow = 2;
			}

			if (PlayerInput.UsingGamepad) {
				localPlayer.GamepadEnableGrappleCooldown();
			}
			if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0) {
				Main.mouseRightRelease = false;
				localPlayer.tileInteractAttempted = true;
				localPlayer.tileInteractionHappened = true;
				localPlayer.releaseUseTile = false;

				Subworld_ToEnter();
			}
		}
	}
	public class Portal_CursedKingdom : Portal {
		public override string Texture => ModUtils.GetTheSameTextureAsEntity<Portal>();
		public override void Subworld_ToEnter() {
			SubworldSystem.Enter<CursedKingdomSubworld>();
		}
	}
}
