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
            stageList.Add(DirectorAPI.Stage.WetlandAspect);
            stageList.Add(DirectorAPI.Stage.GildedCoast);
            stageList.Add(DirectorAPI.Stage.MomentFractured);
            stageList.Add(DirectorAPI.Stage.Bazaar);
            stageList.Add(DirectorAPI.Stage.VoidCell);
            stageList.Add(DirectorAPI.Stage.MomentWhole);
            stageList.Add(DirectorAPI.Stage.Commencement);
            stageList.Add(DirectorAPI.Stage.ThePlanetarium);
            stageList.Add(DirectorAPI.Stage.VoidLocus);
            stageList.Add(DirectorAPI.Stage.VerdantFalls);
            stageList.Add(DirectorAPI.Stage.ViscousFalls);
            stageList.Add(DirectorAPI.Stage.ShatteredAbodes);
            stageList.Add(DirectorAPI.Stage.DisturbedImpact);
            stageList.Add(DirectorAPI.Stage.ReformedAltar);
            stageList.Add(DirectorAPI.Stage.TreebornColony);
            stageList.Add(DirectorAPI.Stage.GoldenDieback);
            stageList.Add(DirectorAPI.Stage.HelminthHatchery);
            stageList.Add(DirectorAPI.Stage.PrimeMeridian);
            stageList.Add(DirectorAPI.Stage.ComputationalExchange);
            stageList.Add(DirectorAPI.Stage.ConduitCanyon);
            stageList.Add(DirectorAPI.Stage.IronAlluvium);
            stageList.Add(DirectorAPI.Stage.IronAuroras);
            stageList.Add(DirectorAPI.Stage.PretendersPrecipice);
            stageList.Add(DirectorAPI.Stage.RepurposedCrater);
            stageList.Add(DirectorAPI.Stage.NeuralSanctum);
            stageList.Add(DirectorAPI.Stage.SolutionalHaunt);

            return stageList;
        }

    }
}
