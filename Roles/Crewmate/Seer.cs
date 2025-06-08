using AmongUs.GameOptions;

using CrewmateOrImpostor.Roles.Core;
using CrewmateOrImpostor.Roles.Core.Interfaces;

namespace CrewmateOrImpostor.Roles.Crewmate;
public sealed class Seer : RoleBase, IKillFlashSeeable
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(Seer),
            player => new Seer(player),
            CustomRoles.Seer,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            21000,
            null,
            "se",
            "#61b26c"
        );
    public Seer(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    { }
}