using System.Linq;
using AmongUs.GameOptions;

using CrewmateOrImpostor.Roles.Core;

namespace CrewmateOrImpostor
{
    static class CustomRolesHelper
    {
        /// <summary>すべての役職(属性は含まない)</summary>
        public static readonly CustomRoles[] AllRoles = EnumHelper.GetAllValues<CustomRoles>().Where(role => role < CustomRoles.NotAssigned).ToArray();
        /// <summary>すべての属性</summary>
        public static readonly CustomRoles[] AllAddOns = EnumHelper.GetAllValues<CustomRoles>().Where(role => role > CustomRoles.NotAssigned).ToArray();
        /// <summary>スタンダードモードで出現できるすべての役職</summary>
        public static readonly CustomRoles[] AllStandardRoles = AllRoles.Where(role => role is not (CustomRoles.HASFox or CustomRoles.HASTroll)).ToArray();
        /// <summary>HASモードで出現できるすべての役職</summary>
        public static readonly CustomRoles[] AllHASRoles = { CustomRoles.HASFox, CustomRoles.HASTroll };
        public static readonly CustomRoleTypes[] AllRoleTypes = EnumHelper.GetAllValues<CustomRoleTypes>();

        public static bool IsImpostor(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Impostor;

            return false;
        }
        public static bool IsMadmate(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Madmate;
            return role == CustomRoles.SKMadmate;
        }
        public static bool IsImpostorTeam(this CustomRoles role) => role.IsImpostor() || role.IsMadmate();
        public static bool IsNeutral(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Neutral;
            return role is CustomRoles.HASTroll or CustomRoles.HASFox;
        }
        public static bool IsCrewmate(this CustomRoles role) => role.GetRoleInfo()?.CustomRoleType == CustomRoleTypes.Crewmate || (!role.IsImpostorTeam() && !role.IsNeutral());
        public static bool IsVanilla(this CustomRoles role)
        {
            return
                role is CustomRoles.Crewmate or
                CustomRoles.Engineer or
                CustomRoles.Scientist or
                CustomRoles.Tracker or
                CustomRoles.Noisemaker or
                CustomRoles.GuardianAngel or
                CustomRoles.Impostor or
                CustomRoles.Shapeshifter or
                CustomRoles.Phantom;
        }

        public static CustomRoleTypes GetCustomRoleTypes(this CustomRoles role)
        {
            CustomRoleTypes type = CustomRoleTypes.Crewmate;

            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType;

            if (role.IsImpostor()) type = CustomRoleTypes.Impostor;
            if (role.IsNeutral()) type = CustomRoleTypes.Neutral;
            if (role.IsMadmate()) type = CustomRoleTypes.Madmate;
            return type;
        }
        public static int GetCount(this CustomRoles role)
        {
            if (role.IsVanilla())
            {
                var roleOpt = Main.NormalOptions.RoleOptions;
                return role switch
                {
                    CustomRoles.Engineer => roleOpt.GetNumPerGame(RoleTypes.Engineer),
                    CustomRoles.Scientist => roleOpt.GetNumPerGame(RoleTypes.Scientist),
                    CustomRoles.Tracker => roleOpt.GetNumPerGame(RoleTypes.Tracker),
                    CustomRoles.Noisemaker => roleOpt.GetNumPerGame(RoleTypes.Noisemaker),
                    CustomRoles.Shapeshifter => roleOpt.GetNumPerGame(RoleTypes.Shapeshifter),
                    CustomRoles.Phantom => roleOpt.GetNumPerGame(RoleTypes.Phantom),
                    CustomRoles.GuardianAngel => roleOpt.GetNumPerGame(RoleTypes.GuardianAngel),
                    CustomRoles.Crewmate => roleOpt.GetNumPerGame(RoleTypes.Crewmate),
                    _ => 0
                };
            }
            else
            {
                return Options.GetRoleCount(role);
            }
        }
        public static int GetChance(this CustomRoles role)
        {
            if (role.IsVanilla())
            {
                var roleOpt = Main.NormalOptions.RoleOptions;
                return role switch
                {
                    CustomRoles.Engineer => roleOpt.GetChancePerGame(RoleTypes.Engineer),
                    CustomRoles.Scientist => roleOpt.GetChancePerGame(RoleTypes.Scientist),
                    CustomRoles.Noisemaker => roleOpt.GetChancePerGame(RoleTypes.Noisemaker),
                    CustomRoles.Tracker => roleOpt.GetChancePerGame(RoleTypes.Tracker),
                    CustomRoles.Shapeshifter => roleOpt.GetChancePerGame(RoleTypes.Shapeshifter),
                    CustomRoles.Phantom => roleOpt.GetChancePerGame(RoleTypes.Phantom),
                    CustomRoles.GuardianAngel => roleOpt.GetChancePerGame(RoleTypes.GuardianAngel),
                    CustomRoles.Crewmate => roleOpt.GetChancePerGame(RoleTypes.Crewmate),
                    _ => 0
                };
            }
            else
            {
                return Options.GetRoleChance(role);
            }
        }
        public static bool IsEnable(this CustomRoles role) => role.GetCount() > 0;
        public static bool CanMakeMadmate(this CustomRoles role)
        {
            if (role.GetRoleInfo() is SimpleRoleInfo info)
            {
                return info.CanMakeMadmate;
            }

            return false;
        }
        public static RoleTypes GetRoleTypes(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.BaseRoleType.Invoke();
            return role switch
            {
                CustomRoles.GM => RoleTypes.GuardianAngel,

                _ => role.IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate,
            };
        }
    }
    public enum CountTypes
    {
        OutOfGame,
        None,
        Crew,
        Impostor,
        Jackal,
    }
}