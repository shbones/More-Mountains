using MoreMountains.Utils.Compatibility;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MoreMountains
{
    public class BossDropManager : NetworkBehaviour
    {
        public static int NumberRedDropsForRun;
        public static int NumberYellowDropsForRun;
        public static int NumberRedDropsForStage;
        public static int NumberYellowDropsForStage;

        private List<PickupIndex> _redAvailableDrops;
        private List<PickupIndex> _yellowAvailableDrops;
        private List<PickupIndex> _voidTier2AvailableDrops;
        private List<PickupIndex> _voidTier3AvailableDrops;
        private List<PickupIndex> _lunarAvailableDrops;

        private PickupIndex redPickupIndex;
        private PickupIndex yellowPickupIndex;
        private PickupIndex voidTier2PickupIndex;
        private PickupIndex voidTier3PickupIndex;
        private PickupIndex lunarPickupIndex;

        private Vector3 _teleporterBossPosition;


        public static BossDropManager Instance { get; private set; }


        public BossDropManager()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class BossDropManager was instantiated twice");
            Instance = this;
        }

        [Server]
        public void Init()
        {
            Hooks();
        }

        public void AddMoreMountainReward(string sourceObjectName)
        {
            if (sourceObjectName.Contains("CrimsonMountain"))
            {
                RedShrineActivated();
            }
            else if (sourceObjectName.Contains("AmberMountain"))
            {
                YellowShrineActivated();
            }
        }

        [Server]
        public void RedShrineActivated()
        {
            NumberRedDropsForStage += 1 * Run.instance.participatingPlayerCount;
            if (RiskyArtifactsCompatibility.enabled)
            {
                RiskyArtifactsCompatibility.addRedStackIfArrognanceEnabled();
            }
        }

        public void YellowShrineActivated()
        {
            NumberYellowDropsForStage += 1 * Run.instance.participatingPlayerCount;
        }

        [Server]
        private void ResetRunShrinesHit(Run obj)
        {
            NumberRedDropsForRun = 0;
            NumberYellowDropsForRun = 0;
            NumberRedDropsForStage = 0;
            NumberYellowDropsForStage = 0;
        }

        [Server]
        private void ResetStageShrinesHit(Stage obj)
        {
            NumberRedDropsForStage = 0;
            NumberYellowDropsForStage = 0;
            if (RiskyArtifactsCompatibility.enabled)
            {
                RiskyArtifactsCompatibility.stageStartArroganceCompat();
            }
        }

        [Server]
        private void TrackTeleporterBoss(On.RoR2.BossGroup.orig_Start orig, BossGroup self)
        {
            //Stores position of teleporter drop spawn so that they can be replaced with appropriate tiered awards
            //Also use this to check if teleporter boss rewards should be scaled by player count. I assume they always will, but maybe another mod changes that.
            if (self.gameObject.name.ToLower().Contains("teleporter"))
            {
                if (!self.scaleRewardsByPlayerCount)
                {
                    NumberRedDropsForStage /= Run.instance.participatingPlayerCount;
                }
                if (NumberRedDropsForStage > 0)
                {
                    int redDropIndex = UnityEngine.Random.Range(0, Run.instance.availableTier3DropList.Count - 1);
                    redPickupIndex = Run.instance.availableTier3DropList[redDropIndex];
                }
                if (NumberYellowDropsForStage > 0)
                {
                    int yellowDropIndex = UnityEngine.Random.Range(0, Run.instance.availableBossDropList.Count - 1);
                    yellowPickupIndex = Run.instance.availableBossDropList[yellowDropIndex];
                }
                _teleporterBossPosition = self.dropPosition.position;
            }
        }

        [Server]
        private void ReplaceTeleporterDropsIfNecessary(On.RoR2.PickupDropletController.orig_CreatePickupDroplet_PickupIndex_Vector3_Vector3 orig, PickupIndex pickupIndex, Vector3 position, Vector3 velocity)
        {
            if (position == _teleporterBossPosition)
            {
                if (NumberRedDropsForStage > 0)
                {
                    orig(redPickupIndex, position, velocity);
                    --NumberRedDropsForStage;
                    return;
                }
                if (NumberYellowDropsForStage > 0)
                {
                    orig(yellowPickupIndex, position, velocity);
                    --NumberYellowDropsForStage;
                    return;
                }
            }
            orig(pickupIndex, position, velocity);
        }

        private void Hooks()
        {
            Run.onRunStartGlobal += ResetRunShrinesHit;
            Stage.onStageStartGlobal += ResetStageShrinesHit;
            On.RoR2.BossGroup.Start += TrackTeleporterBoss;
            On.RoR2.PickupDropletController.CreatePickupDroplet_PickupIndex_Vector3_Vector3 += ReplaceTeleporterDropsIfNecessary;
        }
    }
}
