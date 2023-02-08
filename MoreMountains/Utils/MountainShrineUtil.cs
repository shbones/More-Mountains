using R2API;
using System.Collections.Generic;

namespace MoreMountains.Interactables
{
    public static class MountainShrineUtil
    {

        public static List<DirectorAPI.Stage> GetNormalStageList()
        {
            List<DirectorAPI.Stage> stageList = new List<DirectorAPI.Stage>();

            stageList.Add(DirectorAPI.Stage.DistantRoost);
            stageList.Add(DirectorAPI.Stage.AbyssalDepths);
            stageList.Add(DirectorAPI.Stage.TitanicPlains);
            stageList.Add(DirectorAPI.Stage.SunderedGrove);
            stageList.Add(DirectorAPI.Stage.SirensCall);
            stageList.Add(DirectorAPI.Stage.SkyMeadow);
            stageList.Add(DirectorAPI.Stage.SulfurPools);
            stageList.Add(DirectorAPI.Stage.ScorchedAcres);
            stageList.Add(DirectorAPI.Stage.AphelianSanctuary);

            return stageList;

        }

        public static List<DirectorAPI.Stage> GetSnowyStageList()
        {
            List<DirectorAPI.Stage> stageList = new List<DirectorAPI.Stage>();

            stageList.Add(DirectorAPI.Stage.SiphonedForest);
            stageList.Add(DirectorAPI.Stage.RallypointDelta);

            return stageList;

        }

        public static List<DirectorAPI.Stage> GetSandyStageList()
        {
            List<DirectorAPI.Stage> stageList = new List<DirectorAPI.Stage>();

            stageList.Add(DirectorAPI.Stage.AbandonedAqueduct);

            return stageList;

        }

        public static List<DirectorAPI.Stage> GetAllStageList()
        {
            List<DirectorAPI.Stage> stageList = new List<DirectorAPI.Stage>();

            stageList.Add(DirectorAPI.Stage.DistantRoost);
            stageList.Add(DirectorAPI.Stage.AbyssalDepths);
            stageList.Add(DirectorAPI.Stage.TitanicPlains);
            stageList.Add(DirectorAPI.Stage.SunderedGrove);
            stageList.Add(DirectorAPI.Stage.SirensCall);
            stageList.Add(DirectorAPI.Stage.SkyMeadow);
            stageList.Add(DirectorAPI.Stage.SulfurPools);
            stageList.Add(DirectorAPI.Stage.ScorchedAcres);
            stageList.Add(DirectorAPI.Stage.AphelianSanctuary);
            stageList.Add(DirectorAPI.Stage.SiphonedForest);
            stageList.Add(DirectorAPI.Stage.RallypointDelta);
            stageList.Add(DirectorAPI.Stage.AbandonedAqueduct);

            return stageList;

        }
    }
}
