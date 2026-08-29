using Il2CppDrova;

namespace InsaneMonstersInClassicDrova
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
            var insaneScenes = __instance._onlyInsaneScenes;
            var additiveScenes = __instance._additiveScenes;
            if (insaneScenes != null && additiveScenes != null && insaneScenes.Count > 0)
            {
                for (int i = 0; i < insaneScenes.Count; i++)
                {
                    additiveScenes.Add(insaneScenes[i]);
                }
                if (!__instance._loadAdditiveScenes)
                {
                    __instance._loadAdditiveScenes = true;
                }
            }
            insaneScenes?.Clear();
            __instance._onlyDefaultScenes?.Clear();
        }
    }
}
