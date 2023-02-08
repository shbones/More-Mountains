using BepInEx.Configuration;
using UnityEngine;

namespace MoreMountains.Interactables
{
    public class AmberMountainShrine : MoreMountainShrineBase<AmberMountainShrine>

    {
        public override string InteractableName => "Amber Shrine of the Mountain";

        public override string SpawnCardName => "iscShrineBossAmber";

        public override string PrefabName => "AmberMountain.prefab";

        public override string InteractableLangToken => "AMBER_MOUNTAIN_SHRINE";

        public override string InteractableMessage => "<style=cShrine>{0} has invited the challenge of the Amber Mountain..</color>";

        public override string InteractableMessage2P => "<style=cShrine>You have invited the challenge of the Amber Mountain..</color>";

        public override Color IconColor => Color.yellow;

        public override int NumberOfMountainDirectorStacksAdded => MoreMountainsConfigManager.YellowShrineMountainDifficultyStacks.Value;
    }
}

