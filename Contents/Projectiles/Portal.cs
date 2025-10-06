using Microsoft.Xna.Framework;
using Roguelike.Common.Subworlds;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat.Commands;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Projectiles
{
    public class Portal : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsInteractable[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FlyingPiggyBank);
            Projectile.aiStyle = -1;
            Projectile.hide = false;
        }

        public override void AI()
        {
            if(++Projectile.frameCounter % 2 == 0)
                if(++Projectile.frame == 30)
                    Projectile.frame = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            TryInteracting();
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value,Projectile.Center - Main.screenPosition,new Rectangle(256 * Projectile.frame,0,256,256),Color.White,0,new Vector2(128,128),1f,Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
        }

        private void TryInteracting()
        {
            if (Main.gamePaused || Main.gameMenu)
            {
                return;
            }

            bool cursorHighlights = Main.SmartCursorIsUsed || PlayerInput.UsingGamepad;
            var localPlayer = Main.LocalPlayer;
            var compareSpot = localPlayer.Center;
            if (!localPlayer.IsProjectileInteractibleAndInInteractionRange(Projectile, ref compareSpot))
            {
                return;
            }

            var matrix = Matrix.Invert(Main.GameViewMatrix.ZoomMatrix);
            var position = Main.ReverseGravitySupport(Main.MouseScreen);
            Vector2.Transform(Main.screenPosition, matrix);
            var realMouseWorld = Vector2.Transform(position, matrix) + Main.screenPosition;

            bool mouseDirectlyOver = Projectile.Hitbox.Contains(realMouseWorld.ToPoint());
            bool interactingWithThisProjectile = mouseDirectlyOver || Main.SmartInteractProj == Projectile.whoAmI;
            if (!interactingWithThisProjectile || localPlayer.lastMouseInterface)
            {
                return;
            }

            Main.HasInteractibleObjectThatIsNotATile = true;
            if (mouseDirectlyOver)
            {
                localPlayer.noThrow = 2;
            }

            if (PlayerInput.UsingGamepad)
            {
                localPlayer.GamepadEnableGrappleCooldown();
            }
            if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
            {
                Main.mouseRightRelease = false;
                localPlayer.tileInteractAttempted = true;
                localPlayer.tileInteractionHappened = true;
                localPlayer.releaseUseTile = false;
                
                SubworldSystem.Enter<CursedKingdomSubworld>();

            }
        }

    }
}
