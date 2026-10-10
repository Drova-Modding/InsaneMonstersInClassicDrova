using Drova_Modding_API.Access;
using Drova_Modding_API.GlobalFields;
using MelonLoader;

[assembly: MelonInfo(typeof(InsaneMonstersInClassicDrova.Core), "InsaneMonstersInClassicDrova", "1.0.1", "TrustNoOneElse")]
[assembly: MelonGame("Just2D", "Drova")]
[assembly: MelonAdditionalDependencies("Drova_Modding_API")]

namespace InsaneMonstersInClassicDrova
{
    public class Core : MelonMod
    {
        private bool _localizationCreated;

        public override void OnInitializeMelon()
        {
            OptionMenuAccess.Instance.OnOptionMenuOpen += Options.BuildPanel;
            OptionMenuAccess.Instance.OnOptionMenuClose += Options.Refresh;
            Options.ShareWithCoopHost();
        }

        public override void OnLateInitializeMelon()
        {
            PatchInstaller.InstallAll(HarmonyInstance);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == SceneNames.MainMenu && !_localizationCreated)
            {
                _localizationCreated = true;
                Options.CreateLocalization();
            }
            else if (sceneName == SceneNames.GameplayMain)
            {
                Options.Refresh();
            }
        }
    }
}
