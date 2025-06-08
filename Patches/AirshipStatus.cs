using HarmonyLib;

using CrewmateOrImpostor.Roles.Core;

namespace CrewmateOrImpostor
{
    //参考元:https://github.com/yukieiji/ExtremeRoles/blob/master/ExtremeRoles/Patches/AirShipStatusPatch.cs
    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.PrespawnStep))]
    public static class AirshipStatusPrespawnStepPatch
    {
        public static bool Prefix()
        {
            if (PlayerControl.LocalPlayer.Is(CustomRoles.GM))
            {
                RandomSpawn.AirshipSpawn(PlayerControl.LocalPlayer);
                // GMは湧き画面をスキップ
                return false;
            }
            return true;
        }
    }
}