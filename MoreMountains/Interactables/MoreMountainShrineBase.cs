using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Hologram;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;


namespace MoreMountains.Interactables
{
    public abstract class MoreMountainShrineBase<T> : MoreMountainShrineBase where T : MoreMountainShrineBase<T>
    {
        public static T instance { get; private set; }

        public MoreMountainShrineBase()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting PurchaseInteractableBase/Interactable was instantiated twice");
            instance = this as T;
        }
    }

    public abstract class MoreMountainShrineBase
    {
        public abstract string InteractableName { get; }

        public abstract string InteractableMessage { get; }

        public abstract string InteractableMessage2P { get; }

        public abstract string SpawnCardName { get; }
        public abstract string PrefabName { get; }

        public abstract string InteractableLangToken { get; }

        public GameObject InteractableModel;

        public abstract Color IconColor { get; }

        public abstract int NumberOfMountainDirectorStacksAdded { get; }

        public string InteractableContext = "This shrine looks different than usual...are you sure?";

        public GameObject InteractableBodyModelPrefab;

        public InteractableSpawnCard InteractableSpawnCard;

        public void Init(ConfigFile config, GameObject mountainShrinePrefab)
        {
            CreateFromMountainPrefab(mountainShrinePrefab);
            CreateInteractableSpawnCard(InteractableBodyModelPrefab, SpawnCardName, MountainShrineUtil.GetAllStageList());
            CreateLang();
        }

        private void CreateFromMountainPrefab(GameObject mountainShrine)
        {
            InteractableModel = PrefabAPI.InstantiateClone(mountainShrine, PrefabName);

            var vanillaShrineBossBehavior = InteractableModel.GetComponent<ShrineBossBehavior>();
            UnityEngine.Object.Destroy(vanillaShrineBossBehavior);

            InteractableBodyModelPrefab = InteractableModel;
            var purchaseInteraction = InteractableBodyModelPrefab.GetComponent<PurchaseInteraction>();
            purchaseInteraction.displayNameToken = $"INTERACTABLE_{InteractableLangToken}_NAME";
            purchaseInteraction.contextToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";

            var pingInfoProvider = InteractableModel.AddComponent<PingInfoProvider>();
            pingInfoProvider.pingIconOverride = LegacyResourcesAPI.Load<Sprite>("Textures/MiscIcons/texShrineIconOutlined");

            var genericNameDisplay = InteractableBodyModelPrefab.GetComponent<GenericDisplayNameProvider>();
            genericNameDisplay.displayToken = $"INTERACTABLE_{InteractableLangToken}_NAME";

            Transform icon = setUpBillboardIcon(InteractableBodyModelPrefab, IconColor);
            changeColorOfShrine(InteractableBodyModelPrefab, IconColor);

            var myManager = InteractableBodyModelPrefab.AddComponent<MoreMountainShrineBehavior>();
            myManager.PurchaseInteraction = purchaseInteraction;
            myManager.IconIndicator = icon;
            myManager.NumberOfMountainDirectorStacksToAdd = this.NumberOfMountainDirectorStacksAdded;
            myManager.InteractableLangToken = InteractableLangToken;
        }

        private InteractableSpawnCard CreateInteractableSpawnCard(GameObject aInteractableModel, string name, List<DirectorAPI.Stage> stageList)
        {
            InteractableSpawnCard interactableSpawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            interactableSpawnCard.name = name;
            interactableSpawnCard.prefab = aInteractableModel;
            interactableSpawnCard.sendOverNetwork = true;
            interactableSpawnCard.hullSize = HullClassification.Golem;
            interactableSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            interactableSpawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            interactableSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoShrineSpawn;
            interactableSpawnCard.directorCreditCost = 20;
            interactableSpawnCard.occupyPosition = true;
            interactableSpawnCard.orientToFloor = false;
            interactableSpawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            DirectorCard directorCard = new DirectorCard
            {
                selectionWeight = 0,
                spawnCard = interactableSpawnCard,
            };

            foreach (DirectorAPI.Stage stage in stageList)
            {
                DirectorAPI.Helpers.AddNewInteractableToStage(directorCard, DirectorAPI.InteractableCategory.Shrines, stage);
            }

            return interactableSpawnCard;
        }


        private static void changeColorOfShrine(GameObject normalModel, Color iconColor)
        {            
            var meshRenderer = normalModel.transform.Find("Base").GetChild(0).gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material.SetColor("_Color", iconColor);
        }

        private static Transform setUpBillboardIcon(GameObject normalModel, Color iconColor)
        {
            // shows an icon on top of the interactable
            var icon = normalModel.transform.Find("Symbol");
            var billboard = icon.gameObject.AddComponent<Billboard>();
            icon.gameObject.AddComponent<NetworkIdentity>();

            // applying hopoo shader to the icon
            Material material = LegacyResourcesAPI.Load<SpawnCard>("spawncards/interactablespawncard/iscShrineBoss").prefab.transform.Find("Symbol").GetComponent<MeshRenderer>().material;
            MeshRenderer component = icon.GetComponent<MeshRenderer>();


            Texture texture = material.mainTexture;

            component.material = new Material(material.shader);

            component.material.CopyPropertiesFromMaterial(material);

            component.material.mainTexture = texture;

            component.material.SetColor("_TintColor", iconColor);

            return icon;
        }

        protected void CreateLang()
        {
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_NAME", InteractableName);
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_CONTEXT", InteractableContext);
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_MESSAGE", InteractableMessage);
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_MESSAGE_2P", InteractableMessage2P);
        }

        //Unused. Will Use for any future Custom Models
        private void CreateInteractable()
        {
            InteractableBodyModelPrefab = InteractableModel;
            InteractableBodyModelPrefab.AddComponent<NetworkIdentity>();

            var purchaseInteraction = InteractableBodyModelPrefab.AddComponent<PurchaseInteraction>();
            purchaseInteraction.displayNameToken = $"INTERACTABLE_{InteractableLangToken}_NAME";
            purchaseInteraction.contextToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";
            purchaseInteraction.costType = CostTypeIndex.None;
            purchaseInteraction.automaticallyScaleCostWithDifficulty = false;
            purchaseInteraction.cost = 0;
            purchaseInteraction.available = true;
            purchaseInteraction.setUnavailableOnTeleporterActivated = true;
            purchaseInteraction.isShrine = true;
            purchaseInteraction.isGoldShrine = false;


            var pingInfoProvider = InteractableModel.AddComponent<PingInfoProvider>();
            pingInfoProvider.pingIconOverride = LegacyResourcesAPI.Load<Sprite>("Textures/MiscIcons/texShrineIconOutlined");

            var genericNameDisplay = InteractableBodyModelPrefab.AddComponent<GenericDisplayNameProvider>();
            genericNameDisplay.displayToken = $"INTERACTABLE_{InteractableLangToken}_NAME";

            Transform icon = setUpBillboardIcon(InteractableBodyModelPrefab, IconColor);

            var myManager = InteractableBodyModelPrefab.AddComponent<MoreMountainShrineBehavior>();
            myManager.PurchaseInteraction = purchaseInteraction;
            myManager.IconIndicator = icon;
            myManager.NumberOfMountainDirectorStacksToAdd = this.NumberOfMountainDirectorStacksAdded;
            myManager.InteractableLangToken = InteractableLangToken;

            var entityLocator = InteractableBodyModelPrefab.GetComponentInChildren<BoxCollider>().gameObject.GetComponent<RoR2.EntityLocator>();
            entityLocator.entity = InteractableBodyModelPrefab;

            var modelLocator = InteractableBodyModelPrefab.AddComponent<ModelLocator>();
            modelLocator.modelTransform = InteractableBodyModelPrefab.transform;
            modelLocator.modelBaseTransform = InteractableBodyModelPrefab.transform.Find("Base");
            modelLocator.dontDetatchFromParent = true;
            modelLocator.autoUpdateModelTransform = true;

            var highlightController = InteractableBodyModelPrefab.GetComponent<RoR2.Highlight>();
            highlightController.targetRenderer = InteractableBodyModelPrefab.GetComponentsInChildren<MeshRenderer>().Where(x => x.gameObject.name.Contains("mdlShrineBoss")).First();
            highlightController.strength = 1;
            highlightController.highlightColor = RoR2.Highlight.HighlightColor.interactive;

            var hologramController = InteractableBodyModelPrefab.AddComponent<HologramProjector>();
            hologramController.hologramPivot = InteractableBodyModelPrefab.transform.Find("HologramPivot");
            hologramController.displayDistance = 10;
            hologramController.disableHologramRotation = true;

            var childLocator = InteractableBodyModelPrefab.AddComponent<ChildLocator>();

            PrefabAPI.RegisterNetworkPrefab(InteractableBodyModelPrefab);
        }
    }
}
