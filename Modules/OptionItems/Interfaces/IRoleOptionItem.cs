using CrewmateOrImpostor.Roles.Core;
using UnityEngine;

namespace CrewmateOrImpostor.Modules.OptionItems.Interfaces;

public interface IRoleOptionItem
{
    public CustomRoles RoleId { get; }
    public Color RoleColor { get; }
}
