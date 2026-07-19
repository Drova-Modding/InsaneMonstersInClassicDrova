using Il2CppDrova;

namespace InsaneMonstersInClassicDrova
{
    internal static class GameModePatches
    {
        private static int _spoofDepth;

        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(GameModeAccess), nameof(GameModeAccess.IsInsaneMode), typeof(GameModePatches),
                postfix: nameof(IsInsaneModePostfix));
        }

        internal static void EnterSpoof()
        {
            _spoofDepth++;
        }

        internal static void ExitSpoof()
        {
            if (_spoofDepth > 0)
            {
                _spoofDepth--;
            }
        }

        private static void IsInsaneModePostfix(ref bool __result)
        {
            if (!__result && _spoofDepth > 0 && ModeGate.IsClassicSide())
            {
                __result = true;
            }
        }
    }
}
