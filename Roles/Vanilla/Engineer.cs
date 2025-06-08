using AmongUs.GameOptions;

using CrewmateOrImpostor.Roles.Core;

namespace CrewmateOrImpostor.Roles.Vanilla;

public sealed class Engineer : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.CreateForVanilla(
            typeof(Engineer),
            player => new Engineer(player),
            RoleTypes.Engineer,
            "#8cffff"
        );
    public Engineer(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    { }
}
