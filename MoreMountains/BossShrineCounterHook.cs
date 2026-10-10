using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using System.Collections.Generic;
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

            // Collect only the mountain symbols, ignoring hex symbols from Hex of Strife
            var mountainSymbols = new List<MeshRenderer>();
            for (int i = 0; i < self.targetTransform.childCount; i++)
            {
                var mr = self.targetTransform.GetChild(i).GetComponent<MeshRenderer>();
                if (mr != null && !IsHexSymbol(mr))
                    mountainSymbols.Add(mr);
            }

            for (int i = mountainSymbols.Count - 1; i >= 0; i--)
            {
                if (redShrinesHit <= 0 && yellowShrinesHit <= 0)
                    break;

                MeshRenderer mr = mountainSymbols[i];
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

        private static bool IsHexSymbol(MeshRenderer mr)
        {
            var mat = mr.sharedMaterial;
            return mat != null && mat.name.Contains("Hex");
        }

    }
}
