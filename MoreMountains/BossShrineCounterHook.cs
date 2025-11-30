using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;

namespace MoreMountains
{
    internal static class BossShrineCounterHooks
    {
        public static void Hook()
        {
            On.RoR2.BossShrineCounter.RebuildIndicators += BossShrineCounter_RebuildIndicators;
        }

        private static void BossShrineCounter_RebuildIndicators(On.RoR2.BossShrineCounter.orig_RebuildIndicators orig, BossShrineCounter self)
        {
            orig(self);

            int redShrinesHit = ShrineActivationTracker.crimsonShrinesHit;
            int yellowShrinesHit = ShrineActivationTracker.amberShrinesHit;

            for (int i = self.targetTransform.childCount - 1; i >= 0; i--)
            {
                if (redShrinesHit <= 0 && yellowShrinesHit <= 0)
                    break;

                Transform indicator = self.targetTransform.GetChild(i);
                Transform symbol = indicator.Find("ShrineBossSymbol");
                if (!symbol)
                    continue;

                MeshRenderer mr = symbol.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material = new Material(mr.material);

                    if (redShrinesHit > 0)
                    {
                        mr.material.SetColor("_TintColor", Color.red);
                        redShrinesHit--;
                    }
                    else
                    {
                        mr.material.SetColor("_TintColor", Color.yellow);
                        yellowShrinesHit--;
                    }
                }
            }
        }

    }
}
