using BepInEx;
using MoreMountains.Interactables;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MoreMountains
{
    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(R2API.R2API.PluginGUID)]
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
        public const string PluginVersion = "1.1.0";
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            InitPlugin();
            InitNewMountainShrines();
            DirectorCardCategorySelection.calcCardWeight += this.SpawnCardSubscription;
            On.RoR2.DirectorCore.TrySpawnObject += ReplaceVanillaMountains;
            Log.LogInfo(PluginGUID + " initialized.");
        }

        private void InitNewMountainShrines()
        {
            var InteractableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(MoreMountainShrineBase)));
            Log.LogInfo("-----------------Initializing More Mountains---------------------");
            foreach (var interactableType in InteractableTypes)
            {
                MoreMountainShrineBase interactable = (MoreMountainShrineBase)Activator.CreateInstance(interactableType);
                interactable.Init(Config);

                Log.LogDebug("Interactable: " + interactable.InteractableName + " Initialized!");
            }
        }

        private void InitPlugin()
        {
            Log.Init(Logger);
            thePluginInfo = Info;
            new MoreMountainsConfigManager().Init(Paths.ConfigPath);
            BossDropManager DropManager = new BossDropManager();
            DropManager.Init();
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
            if (card.spawnCard.name.Contains("iscShrineBossCrimson")) {
                _crimsonSpawnCard = (InteractableSpawnCard)card.spawnCard;
            } else if (card.spawnCard.name.Contains("iscShrineBossAmber"))
            {
                _amberSpawnCard = (InteractableSpawnCard)card.spawnCard;
            }
            else if (card.spawnCard.name.Contains("iscShrineBoss"))
            {
                weight *= (float)Math.Round((double)weight * MoreMountainsConfigManager.VanillaMountainSpawnWeightMultiplier.Value);
            }
        }
    }
}
