#if ML
using System;
using System.IO;
using MelonLoader;
using UnityExplorer;
using UnityExplorer.Config;
using UnityExplorer.Loader.ML;

#if CPP
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.IL2CPP)]
#else
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.MONO)]
#endif

[assembly: MelonInfo(typeof(ExplorerMelonMod), ExplorerCore.NAME, ExplorerCore.VERSION, ExplorerCore.AUTHOR)]
[assembly: MelonGame(null, null)]
[assembly: MelonColor(ConsoleColor.DarkCyan)]

namespace UnityExplorer
{
    public class ExplorerMelonMod : MelonMod, IExplorerLoader
    {
        public string ExplorerFolderName => ExplorerCore.DEFAULT_EXPLORER_FOLDER_NAME;
        public string ExplorerFolderDestination
        {
            get
            {
                // In MelonLoader 0.71+, Location points to the mod DLL
                // Mods directory is the parent directory of the mod DLL
                var modsDir = Path.GetDirectoryName(Location);
                if (string.IsNullOrEmpty(modsDir))
                {
                    // Fallback: try to find Mods directory relative to game directory
                    var gameDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    if (!string.IsNullOrEmpty(gameDir))
                    {
                        modsDir = Path.Combine(gameDir, "Mods");
                    }
                }
                return modsDir ?? "Mods";
            }
        }

        public string UnhollowedModulesFolder
        {
            get
            {
                // Try to find MelonLoader/Managed directory
                var modsDir = ExplorerFolderDestination;
                var gameDir = Path.GetDirectoryName(modsDir);
                if (string.IsNullOrEmpty(gameDir))
                {
                    gameDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                return Path.Combine(Path.Combine(gameDir ?? ".", "MelonLoader"), "Managed");
            }
        }

        public ConfigHandler ConfigHandler => _configHandler;
        public MelonLoaderConfigHandler _configHandler;

        public Action<object> OnLogMessage => MelonLogger.Msg;
        public Action<object> OnLogWarning => MelonLogger.Warning;
        public Action<object> OnLogError   => MelonLogger.Error;

        public override void OnApplicationStart()
        {
            _configHandler = new MelonLoaderConfigHandler();
            ExplorerCore.Init(this);
        }
    }
}
#endif