using Il2CppDrova;
using Il2CppDrova.GlobalVarSystem;
using Il2CppDrova.Spawners;

namespace InsaneMonstersInClassicDrova
{
    internal static class SpawnPatches
    {
        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(SpawnController), nameof(SpawnController.Awake), typeof(SpawnPatches),
                prefix: nameof(SpawnControllerAwakePrefix));
            PatchInstaller.TryPatch(harmony, typeof(GameModeGlobalCondition), nameof(GameModeGlobalCondition.CheckCondition), typeof(SpawnPatches),
                postfix: nameof(CheckConditionPostfix));
        }

        private static void SpawnControllerAwakePrefix(SpawnController __instance)
        {
            if (!PatchInstaller.Is<SpawnController>(__instance) || !ModeGate.SpawnGroupsActive() || !__instance._overrideDataInInsaneMode)
            {
                return;
            }
            if (__instance._spawnDataInsane != null && __instance._spawnDataInsane.Count > 0)
            {
                __instance._spawnData = __instance._spawnDataInsane;
            }
            __instance._maxActiveActors = __instance._maxActiveActorsInsane;
            __instance._minMaxSpawnDelay = __instance._minMaxSpawnDelayInsane;
            __instance._spawnFromCamera = __instance._spawnFromCameraInsane;
            __instance._overrideDataInInsaneMode = false;
        }

        private static void CheckConditionPostfix(GameModeGlobalCondition __instance, ref bool __result)
        {
            if (!PatchInstaller.Is<GameModeGlobalCondition>(__instance) || !ModeGate.SpawnGroupsActive() || !ModeGate.TryGetPretendInsaneMode(out var pretendMode))
            {
                return;
            }
            __result = __instance._mode == pretendMode || __instance._mode.IsInsaneMode();
        }
    }
}
