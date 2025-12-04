using BepInEx;
using MoreMountains.Interactables;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace MoreMountains
{
    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInDependency(DirectorAPI.PluginGUID)]
    [BepInDependency("com.Moffein.RiskyArtifacts", BepInDependency.DependencyFlags.SoftDependency)]
    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    [R2APISubmoduleDependency(nameof(LanguageAPI), nameof(DirectorAPI), nameof(PrefabAPI))]

    //This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    public class MoreMountainsPlugin : BaseUnityPlugin
    {
        public static PluginInfo thePluginInfo { get; private set; }

        private static InteractableSpawnCard _crimsonSpawnCard;
        private static InteractableSpawnCard _amberSpawnCard;


        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginAuthor = "shbones";
        public const string PluginName = "MoreMountains";
        public const string PluginVersion = "1.2.1";
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            InitPlugin();
            Log.LogDebug("Beginning MoreMountains Init");

            var handler = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ShrineBoss/ShrineBoss.prefab");
            handler.Completed += mountainShrineLoadRequest =>
            {
                Log.LogDebug("Mountain Shrine Prefab Loaded");
                InitNewMountainShrines(mountainShrineLoadRequest.Result);
                InitHooks();
                DirectorCardCategorySelection.calcCardWeight += this.SpawnCardSubscription;
                On.RoR2.DirectorCore.TrySpawnObject += ReplaceVanillaMountains;
                Log.LogInfo(PluginGUID + " initialized.");
            };
        }


        private void InitNewMountainShrines(GameObject mountainShrinePrefab)
        {
            var InteractableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(MoreMountainShrineBase)));
            Log.LogInfo("-----------------Initializing More Mountains---------------------");
            foreach (var interactableType in InteractableTypes)
            {
                MoreMountainShrineBase interactable = (MoreMountainShrineBase)Activator.CreateInstance(interactableType);
                interactable.Init(Config, mountainShrinePrefab);

                Log.LogDebug("Interactable: " + interactable.InteractableName + " Initialized!");
            }
        }

        private void InitPlugin()
        {
            Log.Init(Logger);
            thePluginInfo = Info;
            new MoreMountainsConfigManager().Init(Paths.ConfigPath);
        }

        private void InitHooks()
        {
            BossDropManager DropManager = new BossDropManager();
            DropManager.Init();
            ShrineActivationTracker ActivationTracker = new ShrineActivationTracker();
            ActivationTracker.Init();
            BossShrineCounterHooks.Hook();
            Run.onRunStartGlobal += ResetRunShrinesHit;
            Stage.onStageStartGlobal += ResetStageShrinesHit;
        }

        private GameObject ReplaceVanillaMountains(On.RoR2.DirectorCore.orig_TrySpawnObject orig, DirectorCore self, DirectorSpawnRequest spawnRequest)
        {
            if (!(Run.instance is InfiniteTowerRun))
            {
                int redReplacementPercent = MoreMountainsConfigManager.RedShrineMountainReplacementPercent.Value;
                int yellowReplacementPercent = redReplacementPercent + MoreMountainsConfigManager.YellowShrineMountainReplacementPercent.Value;
                var card = spawnRequest.spawnCard;
                if (card.name.Contains("iscShrineBoss"))
                {
                    int rng = UnityEngine.Random.Range(1, 100);
                    if (rng <= redReplacementPercent)
                    {
                        Log.LogDebug("Replacing Mountain with Crimson Mountain!");
                        spawnRequest.spawnCard = _crimsonSpawnCard;
                    }
                    else if (rng <= yellowReplacementPercent)
                    {
                        Log.LogDebug("Replacing Mountain with Amber Mountain!");
                        spawnRequest.spawnCard = _amberSpawnCard;
                    }
                    Log.LogDebug("Spawning Normal Mountain!");
                }
            }
            return orig(self, spawnRequest);
        }

        private void SpawnCardSubscription(DirectorCard card, ref float weight)
        {
            if (card.spawnCard.name.Contains("iscShrineBossCrimson"))
            {
                _crimsonSpawnCard = (InteractableSpawnCard)card.spawnCard;
            }
            else if (card.spawnCard.name.Contains("iscShrineBossAmber"))
            {
                _amberSpawnCard = (InteractableSpawnCard)card.spawnCard;
            }
            else if (card.spawnCard.name.Contains("iscShrineBoss"))
            {
                weight = weight * MoreMountainsConfigManager.VanillaMountainSpawnWeightMultiplier.Value;
            }
        }


        private void ResetRunShrinesHit(Run obj)
        {
            ShrineActivationTracker.crimsonShrinesHit = 0;
            ShrineActivationTracker.amberShrinesHit = 0;
            BossDropManager.NumberRedDropsForStage = 0;
            BossDropManager.NumberYellowDropsForStage = 0;
        }

        private void ResetStageShrinesHit(Stage obj)
        {
            if (RunArtifactManager.instance != null && RunArtifactManager.instance.IsArtifactEnabled(DLC3Content.Artifacts.Prestige))
            {
                Log.LogInfo("Presige is enabled, not resetting mountain trackers.");
                BossDropManager.NumberRedDropsForStage = BossDropManager.NumberRedDropsForRun;
                BossDropManager.NumberYellowDropsForStage = BossDropManager.NumberYellowDropsForRun;
            }
            else
            {
                ShrineActivationTracker.crimsonShrinesHit = 0;
                ShrineActivationTracker.amberShrinesHit = 0;
                BossDropManager.NumberRedDropsForStage = 0;
                BossDropManager.NumberYellowDropsForStage = 0;
            }
        }
    }
}
