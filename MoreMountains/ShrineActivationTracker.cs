using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreMountains.Utils.Compatibility;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MoreMountains
{
    public class ShrineActivationTracker
    {
        public static int crimsonShrinesHit;
        public static int amberShrinesHit;

        public static ShrineActivationTracker Instance { get; private set; }


        public ShrineActivationTracker()
        {
            if (Instance != null) throw new InvalidOperationException("Singleton class ShineActivationTracker was instantiated twice");
            Instance = this;
        }

        [Server]
        public void Init()
        {
            Hooks();
        }

        public void TrackVariantMountainShrineHit(string sourceObjectName)
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

        public void RedShrineActivated()
        {
            crimsonShrinesHit += 1;
        }

        public void YellowShrineActivated()
        {
            amberShrinesHit += 1;
        }

        public static int AdjustShrineDirectorDifficulty(int originalShrineStacks)
        {
            int shrineDifficultyValue = originalShrineStacks
                + (crimsonShrinesHit * MoreMountainsConfigManager.RedShrineMountainDifficultyStacks.Value)
                + (amberShrinesHit * MoreMountainsConfigManager.YellowShrineMountainDifficultyStacks.Value);
            return shrineDifficultyValue;
        }

        private void Hooks()
        {
            On.RoR2.TeleporterInteraction.ChargingState.OnEnter += ChargingState_OnEnter;
        }

        private void ChargingState_OnEnter(On.RoR2.TeleporterInteraction.ChargingState.orig_OnEnter orig, TeleporterInteraction.ChargingState self)
        {
            orig(self);
            AdjustBossDirectorCredit(self);
        }

        private void AdjustBossDirectorCredit(TeleporterInteraction.ChargingState self)
        {
            if (self.bossDirector == null) return;

            int currentStacks = self.teleporterInteraction.shrineBonusStacks;
            int desiredStacks = AdjustShrineDirectorDifficulty(currentStacks);

            float baseValue = Math.Max(
                self.bossDirector.overrideCost,
                (int)(600f * Mathf.Pow(Run.instance.compensatedDifficultyCoefficient, 0.5f))
            );

            float desiredNum = baseValue * (1 + desiredStacks);
            float numOrigWillAdd = baseValue * (1 + currentStacks);

            self.bossDirector.monsterCredit += (desiredNum - numOrigWillAdd);
        }
    }
}