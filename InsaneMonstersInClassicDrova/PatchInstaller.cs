using System;
using System.Reflection;
using HarmonyLib;
using MelonLoader;

namespace InsaneMonstersInClassicDrova
{
    internal static class PatchInstaller
    {
        private static MelonLogger.Instance _logger;

        internal static void InstallAll(HarmonyLib.Harmony harmony, MelonLogger.Instance logger)
        {
            _logger = logger;
            GameModePatches.Install(harmony);
            SpawnPatches.Install(harmony);
            ScenePatches.Install(harmony);
            MovesetPatches.Install(harmony);
            AbilityPatches.Install(harmony);
            TimingPatches.Install(harmony);
            _logger.Msg("Patches installed.");
        }

        internal static void TryPatch(HarmonyLib.Harmony harmony, Type targetType, string targetMethod, Type patchType,
            string prefix = null, string postfix = null, string finalizer = null, Type[] argumentTypes = null)
        {
            try
            {
                MethodInfo target = argumentTypes == null
                    ? AccessTools.Method(targetType, targetMethod)
                    : AccessTools.Method(targetType, targetMethod, argumentTypes);
                if (target == null)
                {
                    _logger.Warning($"Patch target not found: {targetType.FullName}.{targetMethod}");
                    return;
                }
                harmony.Patch(target,
                    prefix: prefix == null ? null : new HarmonyMethod(AccessTools.Method(patchType, prefix)),
                    postfix: postfix == null ? null : new HarmonyMethod(AccessTools.Method(patchType, postfix)),
                    finalizer: finalizer == null ? null : new HarmonyMethod(AccessTools.Method(patchType, finalizer)),
                    ilmanipulator: null);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to patch {targetType.FullName}.{targetMethod}: {e.Message}");
            }
        }
    }
}
