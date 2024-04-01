using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies
{
    public interface ITargetable
    {
        public void SetTarget(Transform target);
    }
}