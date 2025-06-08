using AmongUs.GameOptions;
using CrewmateOrImpostor.Roles.Core;
using CrewmateOrImpostor.Roles.Core.Interfaces;

namespace CrewmateOrImpostor.Roles.Vanilla;

public sealed class Phantom : RoleBase, IImpostor
{
    public Phantom(PlayerControl player) : base(RoleInfo, player) { }
    public static readonly SimpleRoleInfo RoleInfo = SimpleRoleInfo.CreateForVanilla(typeof(Phantom), player => new Phantom(player), RoleTypes.Phantom);
}
