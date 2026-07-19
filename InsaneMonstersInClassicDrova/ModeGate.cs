using Il2CppDrova;

namespace InsaneMonstersInClassicDrova
{
    internal static class ModeGate
    {
        internal static bool IsClassicSide()
        {
            return SavegameGameHandler.TryGet(out SavegameGameHandler handler)
                && !handler.GetGameMode().IsInsaneMode();
        }

        internal static bool TryGetPretendInsaneMode(out GameMode pretendMode)
        {
            if (SavegameGameHandler.TryGet(out SavegameGameHandler handler))
            {
                GameMode current = handler.GetGameMode();
                if (!current.IsInsaneMode())
                {
                    pretendMode = current switch
                    {
                        GameMode.Immersive => GameMode.Insane_Immersive,
                        GameMode.Hardcore => GameMode.Insane_Hardcore,
                        _ => GameMode.Insane_Classic
                    };
                    return true;
                }
            }
            pretendMode = GameMode.Default;
            return false;
        }

        internal static bool SpawnGroupsActive()
        {
            return Options.SpawnGroups && IsClassicSide();
        }

        internal static bool MovesetsActive()
        {
            return Options.Movesets && IsClassicSide();
        }

        internal static bool AttackTimingsActive()
        {
            return Options.AttackTimings && IsClassicSide();
        }
    }
}
