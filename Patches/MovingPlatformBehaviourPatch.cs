using HarmonyLib;

namespace CrewmateOrImpostor.Patches;

[HarmonyPatch(typeof(MovingPlatformBehaviour))]
public static class MovingPlatformBehaviourPatch
{
    private static bool isDisabled = false;

    [HarmonyPatch(nameof(MovingPlatformBehaviour.Start)), HarmonyPrefix]
    public static void StartPrefix(MovingPlatformBehaviour __instance)
    {
        isDisabled = Options.DisableAirshipMovingPlatform.GetBool();

        if (isDisabled)
        {
            __instance.transform.localPosition = __instance.DisabledPosition;
            ShipStatus.Instance.Cast<AirshipStatus>().outOfOrderPlat.SetActive(true);
        }
    }
    [HarmonyPatch(nameof(MovingPlatformBehaviour.IsDirty), MethodType.Getter), HarmonyPrefix]
    public static bool GetIsDirtyPrefix(ref bool __result)
    {
        if (isDisabled)
        {
            __result = false;
            return false;
        }
        return true;
    }
    [HarmonyPatch(nameof(MovingPlatformBehaviour.Use), typeof(PlayerControl)), HarmonyPrefix]
    public static bool UsePrefix([HarmonyArgument(0)] PlayerControl player)
    {
        // プレイヤーがぬーん使用不可状態のときに使用をブロック
        if (!PlayerState.GetByPlayerId(player.PlayerId).CanUseMovingPlatform)
        {
            return false;
        }
        return !isDisabled;
    }
    [HarmonyPatch(nameof(MovingPlatformBehaviour.SetSide)), HarmonyPrefix]
    public static bool SetSidePrefix() => !isDisabled;
}
