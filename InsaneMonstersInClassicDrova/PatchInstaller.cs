using System;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
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

        /// <summary>True when the hooked instance really is a <typeparamref name="T"/>.</summary>
        /// <remarks>
        /// The shipped IL2CPP build folds identical native method bodies together, so a hook on a short
        /// Awake/Start/getter also fires for unrelated types that compiled to the same code. Reading our
        /// serialized fields off such an instance hits arbitrary memory and kills the process with an
        /// AccessViolation, so every instance patch verifies the runtime class first.
        /// </remarks>
        internal static bool Is<T>(Il2CppObjectBase instance) where T : Il2CppObjectBase
        {
            return instance != null && !instance.WasCollected && instance.TryCast<T>() != null;
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
