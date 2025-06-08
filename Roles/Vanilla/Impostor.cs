using AmongUs.GameOptions;

using CrewmateOrImpostor.Roles.Core;
using CrewmateOrImpostor.Roles.Core.Interfaces;

namespace CrewmateOrImpostor.Roles.Vanilla;

public sealed class Impostor : RoleBase, IImpostor
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.CreateForVanilla(
            typeof(Impostor),
            player => new Impostor(player),
            RoleTypes.Impostor
        );
    public Impostor(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    { }
}