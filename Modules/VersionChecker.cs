using System;
using UnityEngine;

namespace CrewmateOrImpostor.Modules;

public static class VersionChecker
{
    public static bool IsSupported { get; private set; } = true;

    public static void Check()
    {
        // 直接设置为支持的版本
        IsSupported = true;
    }
}
