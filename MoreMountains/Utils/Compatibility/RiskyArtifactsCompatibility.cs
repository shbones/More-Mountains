using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using MoreMountains;


namespace MoreMountains.Utils.Compatibility
{
    public static class RiskyArtifactsCompatibility
    {
        private static bool? _enabled;
        private static ArtifactIndex _arroganceIndex;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyArtifacts");
                }
                return (bool)_enabled;
            }
        }

        public static void addRedStackIfArrognanceEnabled()
        {
            if (isArroganceEnabled())
            {
                ++Risky_Artifacts.Artifacts.Arrogance.stageMountainCount;
                BossDropManager.NumberRedDropsForRun += 1 * Run.instance.participatingPlayerCount;
            }
        }
         
        public static void addYellowStackIfArrognanceEnabled()
        {
            if (isArroganceEnabled())
            {
                ++Risky_Artifacts.Artifacts.Arrogance.stageMountainCount;
                BossDropManager.NumberYellowDropsForRun += 1 * Run.instance.participatingPlayerCount;
            }
        }

        public static void stageStartArroganceCompat()
        {
            if (isArroganceEnabled())
            {
                BossDropManager.NumberRedDropsForStage = BossDropManager.NumberRedDropsForRun;
                int myMountainStacksPerRedShrine = MoreMountainsConfigManager.RedShrineMountainDifficultyStacks.Value;
                if (myMountainStacksPerRedShrine > 1)
                {
                    int myTotalStacksFromRedShrine = (myMountainStacksPerRedShrine - 1) * BossDropManager.NumberRedDropsForRun;
                    for (int i = 0; i < myTotalStacksFromRedShrine; ++i)
                    {
                        ++TeleporterInteraction.instance.shrineBonusStacks;
                    }
                }
                BossDropManager.NumberYellowDropsForStage = BossDropManager.NumberYellowDropsForRun;
                int myMountainStacksPerYellowShrine = MoreMountainsConfigManager.YellowShrineMountainDifficultyStacks.Value;
                if (myMountainStacksPerYellowShrine > 1)
                {
                    int myTotalStacksFromYellowShrine = (myMountainStacksPerYellowShrine - 1) * BossDropManager.NumberYellowDropsForRun;
                    for (int i = 0; i < myTotalStacksFromYellowShrine; ++i)
                    {
                        ++TeleporterInteraction.instance.shrineBonusStacks;
                    }
                }
            } else
            {
                BossDropManager.NumberRedDropsForRun = 0;
                BossDropManager.NumberYellowDropsForRun = 0;
            }
        }

        private static ArtifactIndex getArroganceArtifactIndex()
        {
            if (Risky_Artifacts.Artifacts.Arrogance.enabled)
            {
                return ArtifactCatalog.FindArtifactIndex("RiskyArtifactOfArrogance");
            }
            return ArtifactIndex.None;
        }

        private static bool isArroganceEnabled()
        {
            if (!Risky_Artifacts.Artifacts.Arrogance.enabled)
            {
                return false;
            }
            ArtifactIndex myIndex = getArroganceArtifactIndex();
            if (myIndex == ArtifactIndex.None)
            {
                return false;
            }
            return RunArtifactManager.instance.IsArtifactEnabled(myIndex);
        }
    }
}
