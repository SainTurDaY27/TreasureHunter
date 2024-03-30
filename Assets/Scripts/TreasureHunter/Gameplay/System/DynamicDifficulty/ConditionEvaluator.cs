using System.Linq;
using TreasureHunter.Core.Data;

namespace TreasureHunter.Gameplay.System.DynamicDifficulty
{
    public class ConditionEvaluator
    {
        public static bool EvaluateDynamicCondition(DynamicCondition condition)
        {
            var gameData = DataManager.Instance.GameData;
            var playerData = DataManager.Instance.PlayerData;

            // Check treasure first
            if (condition.useTreasureCondition && gameData.TreasureCount < condition.requiredTreasure) return false;
            if (condition.useSkillCondition)
            {
                if (condition.skillConditions.Any(skill => !playerData.HasSkill(skill)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}