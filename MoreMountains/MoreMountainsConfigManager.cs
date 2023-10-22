using BepInEx.Configuration;
using System.IO;

namespace MoreMountains
{
    public class MoreMountainsConfigManager
    {
        //For Vanilla Mountains
        public static ConfigEntry<int> VanillaMountainSpawnWeightMultiplier;
        public static ConfigEntry<int> VanillaMountainMaxSpawnsMultiplier;

        //Replacement Parameters
        public static ConfigEntry<int> RedShrineMountainReplacementPercent;
        public static ConfigEntry<int> YellowShrineMountainReplacementPercent;

        //Teleporter Director Credit Multiplier (Mountain Scaling)
        public static ConfigEntry<int> RedShrineMountainDifficultyStacks;
        public static ConfigEntry<int> YellowShrineMountainDifficultyStacks;

        public void Init(string configPath)
        {
            var mainConfig = new ConfigFile(Path.Combine(configPath, "shbones-MoreMountains.cfg"), true);

            VanillaMountainSpawnWeightMultiplier = mainConfig.Bind("1. Vanilla Mountain Config", "Spawn Weight Multiplier", 1, "Effectively scales the spawn rate of all mountain shrines. This will have effect whether the shrines added by this mod are enabled or not.");

            RedShrineMountainReplacementPercent = mainConfig.Bind("2. New Shrine Replacement Chance", "Red Mountain Replacement Chance", 10, "Percent chance (e.g value of 10 = 10% chance) to replace a Mountain Shrine that would be spawned with a Crimson Mountain Shrine. Value of 0 disables Crimson Mountain Shrine. Keep the sum of values in this section less than 100.");
            YellowShrineMountainReplacementPercent = mainConfig.Bind("2. New Shrine Replacement Chance", "Yellow Mountain Replacement Chance", 10, "Percent chance (e.g value of 15 = 10% chance) to replace a Mountain Shrine that would be spawned with ab Amber Mountain Shrine. Value of 0 disables Amber Mountain Shrine. Keep the sum of values in this section less than 100.");

            RedShrineMountainDifficultyStacks = mainConfig.Bind("3. New Shrine Difficulty Stacks", "Red Mountain Difficulty Stacks", 5, "Additional Stacks of credits given to the Teleporter Director. A Value of 5 here means the Teleporter difficulty will be equivalent to hitting 6 normal mountain shrines. Reward is always only 1");
            YellowShrineMountainDifficultyStacks = mainConfig.Bind("3. New Shrine Difficulty Stacks", "Yellow Mountain Difficulty Stacks", 4, "Additional Stacks of credits given to the Teleporter Director. A Value of 5 here means the Teleporter difficulty will be equivalent to hitting 6 normal mountain shrines. Reward is always only 1");
        }
    }
}
