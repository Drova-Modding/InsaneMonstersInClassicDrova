using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using InsaneMonstersInClassicDrova.Patches;
using MelonLoader;

namespace InsaneMonstersInClassicDrova
{
    internal static class PatchInstaller
    {
        internal static void InstallAll(HarmonyLib.Harmony harmony)
        {
            GameModePatches.Install(harmony);
            SpawnPatches.Install(harmony);
            ScenePatches.Install(harmony);
            MovesetPatches.Install(harmony);
            AbilityPatches.Install(harmony);
            TimingPatches.Install(harmony);
            Melon<Core>.Logger.Msg("Patches installed.");
        }

        /// <summary>True when the hooked instance really is a <typeparamref name="T"/>.</summary>
        /// <remarks>
        /// The shipped build folds identical native method bodies together, so a hook on a short Awake/Start/getter
        /// also fires for unrelated types. Reading our fields off such an instance crashes with an AccessViolation.
        /// </remarks>
        internal static bool Is<T>(Il2CppObjectBase? instance) where T : Il2CppObjectBase
        {
            return instance != null && !instance.WasCollected && instance.TryCast<T>() != null;
        }

        internal static void TryPatch(HarmonyLib.Harmony harmony, Type targetType, string targetMethod, Type patchType,
            string? prefix = null, string? postfix = null, string? finalizer = null, Type[]? argumentTypes = null)
        {
            try
            {
                MethodInfo? target = argumentTypes == null
                    ? AccessTools.Method(targetType, targetMethod)
                    : AccessTools.Method(targetType, targetMethod, argumentTypes);
                if (target == null)
                {
                    Melon<Core>.Logger.Warning($"Patch target not found: {targetType.FullName}.{targetMethod}");
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
                Melon<Core>.Logger.Warning($"Failed to patch {targetType.FullName}.{targetMethod}: {e.Message}");
            }
        }
    }
}
