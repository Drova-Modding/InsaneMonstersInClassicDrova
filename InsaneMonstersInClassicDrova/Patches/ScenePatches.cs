using Il2Cpp;
using Il2CppDrova;

namespace InsaneMonstersInClassicDrova.Patches
{
    internal static class ScenePatches
    {
        internal static void Install(HarmonyLib.Harmony harmony)
        {
            PatchInstaller.TryPatch(harmony, typeof(SceneRoot), nameof(SceneRoot.Awake), typeof(ScenePatches),
                prefix: nameof(SceneRootAwakePrefix));
        }

        private static void SceneRootAwakePrefix(SceneRoot __instance)
        {
            if (!PatchInstaller.Is<SceneRoot>(__instance) || !ModeGate.SpawnGroupsActive())
            {
                return;
            }
            Il2CppSystem.Collections.Generic.List<SceneRef> insaneScenes = __instance._onlyInsaneScenes;
            Il2CppSystem.Collections.Generic.List<SceneRef> additiveScenes = __instance._additiveScenes;
            if (insaneScenes != null && additiveScenes != null && insaneScenes.Count > 0)
            {
                foreach (SceneRef scene in insaneScenes)
                {
                    additiveScenes.Add(scene);
                }
                __instance._loadAdditiveScenes = true;
            }
            insaneScenes?.Clear();
            __instance._onlyDefaultScenes?.Clear();
        }
    }
}
