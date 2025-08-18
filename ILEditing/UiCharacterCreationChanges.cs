using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Roguelike.ILEditing;
public class UiCharacterCreationChanges : ModSystem
{

    public const byte cursedDifficulty = 133;

    public override void Load()
    {

        // Put Warning in non hardcore difficulty's Description
        IL_UICharacterCreation.UpdateDifficultyDescription += HookUpdateDifficultyDescription;

        // set player.difficulty to hardcore by default instead of classic
        IL_UICharacterCreation.ctor += HookUICharacterCreationCtor;

    }

    private void HookUpdateDifficultyDescription(ILContext il)
    {

        try
        {
            ILCursor c = new ILCursor(il);

            c.GotoNext(MoveType.Before, i => i.MatchLdcI4(31));

            c.Index--;
            c.RemoveRange(4);

            c.EmitLdstr("Mods.Roguelike.SystemTooltip.NotSupported.Warning");
            c.Emit(OpCodes.Call, typeof(Language).GetMethod(nameof(Language.GetText)));
            c.EmitStloc0();

            c.GotoNext(MoveType.Before, i => i.MatchLdcI4(31));

            c.Index--;
            c.RemoveRange(4);

            c.GotoNext(MoveType.Before, i => i.MatchLdcI4(30));

            c.Index--;
            c.RemoveRange(4);

            c.GotoNext(MoveType.Before, i => i.MatchLdstr("UI.CreativeDescriptionPlayer"));

            c.RemoveRange(3);

        }
        catch (Exception e)
        {
            MonoModHooks.DumpIL(ModContent.GetInstance<Roguelike>(), il);
        }
    }

    private void HookUICharacterCreationCtor(ILContext il)
    {

        try
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(i => i.MatchRet());
            c.Index -= 4;
            c.Remove();
            c.EmitLdcI4(2);
        }
        catch (Exception e)
        {
            MonoModHooks.DumpIL(ModContent.GetInstance<Roguelike>(), il);
        }
    }
}
