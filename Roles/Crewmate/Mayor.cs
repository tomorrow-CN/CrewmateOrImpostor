using AmongUs.GameOptions;

using CrewmateOrImpostor.Roles.Core;

namespace CrewmateOrImpostor.Roles.Crewmate;
public sealed class Mayor : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(Mayor),
            player => new Mayor(player),
            CustomRoles.Mayor,
            () => OptionHasPortableButton.GetBool() ? RoleTypes.Engineer : RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            20200,
            SetupOptionItem,
            "my",
            "#204d42",
            introSound: () => GetIntroSound(RoleTypes.Crewmate)
        );
    public Mayor(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        AdditionalVote = OptionAdditionalVote.GetInt();
        HasPortableButton = OptionHasPortableButton.GetBool();
        NumOfUseButton = OptionNumOfUseButton.GetInt();

        LeftButtonCount = NumOfUseButton;
    }

    private static OptionItem OptionAdditionalVote;
    private static OptionItem OptionHasPortableButton;
    private static OptionItem OptionNumOfUseButton;
    enum OptionName
    {
        MayorAdditionalVote,
        MayorHasPortableButton,
        MayorNumOfUseButton,
    }
    public static int AdditionalVote;
    public static bool HasPortableButton;
    public static int NumOfUseButton;

    public int LeftButtonCount;
    private static void SetupOptionItem()
    {
        OptionAdditionalVote = IntegerOptionItem.Create(RoleInfo, 10, OptionName.MayorAdditionalVote, new(1, 99, 1), 1, false)
            .SetValueFormat(OptionFormat.Votes);
        OptionHasPortableButton = BooleanOptionItem.Create(RoleInfo, 11, OptionName.MayorHasPortableButton, false, false);
        OptionNumOfUseButton = IntegerOptionItem.Create(RoleInfo, 12, OptionName.MayorNumOfUseButton, new(1, 99, 1), 1, false, OptionHasPortableButton)
            .SetValueFormat(OptionFormat.Times);
    }
    public override void ApplyGameOptions(IGameOptions opt)
    {
        AURoleOptions.EngineerCooldown =
            LeftButtonCount <= 0
            ? 255f
            : opt.GetInt(Int32OptionNames.EmergencyCooldown);
        AURoleOptions.EngineerInVentMaxTime = 1;
    }
    public override void OnReportDeadBody(PlayerControl reporter, NetworkedPlayerInfo target)
    {
        if (Is(reporter) && target == null) //ボタン
            LeftButtonCount--;
    }
    public override bool OnEnterVent(PlayerPhysics physics, int ventId)
    {
        if (LeftButtonCount > 0)
        {
            var user = physics.myPlayer;
            //ホスト視点、vent処理中に会議を呼ぶとベントの矢印が残るので遅延させる
            _ = new LateTask(() => user?.ReportDeadBody(null), 0.1f, "MayerPortableButton");

            //ポータブルボタン時はベントから追い出す必要はない
            return true;
        }

        return false;
    }
    public override (byte? votedForId, int? numVotes, bool doVote) ModifyVote(byte voterId, byte sourceVotedForId, bool isIntentional)
    {
        // 既定値
        var (votedForId, numVotes, doVote) = base.ModifyVote(voterId, sourceVotedForId, isIntentional);
        if (voterId == Player.PlayerId)
        {
            numVotes = AdditionalVote + 1;
        }
        return (votedForId, numVotes, doVote);
    }
}