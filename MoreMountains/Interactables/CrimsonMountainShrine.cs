using BepInEx.Configuration;
using UnityEngine;

namespace MoreMountains.Interactables
{
    public class CrimsonMountainShrine : MoreMountainShrineBase<CrimsonMountainShrine>

    {
        public override string InteractableName => "Crimson Shrine of the Mountain";

        public override string SpawnCardName => "iscShrineBossCrimson";

        public override string PrefabName => "CrimsonMountain.prefab";

        public override string InteractableLangToken => "CRIMSON_MOUNTAIN_SHRINE";

        public override string InteractableMessage => "<style=cShrine>{0} has invited the challenge of the Crimson Mountain..</color>";

        public override string InteractableMessage2P => "<style=cShrine>You have invited the challenge of the Crimson Mountain..</color>";

        public override Color IconColor => Color.red;

        public override int NumberOfMountainDirectorStacksAdded => MoreMountainsConfigManager.RedShrineMountainDifficultyStacks.Value;
    }
}
