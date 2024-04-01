using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies
{
    public interface ISetWaypointable
    {
        public void SetWaypoints(List<Transform> waypoints);
        public void ResetWaypoint();
    }
}