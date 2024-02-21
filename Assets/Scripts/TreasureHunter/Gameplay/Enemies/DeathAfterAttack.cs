using System;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public class DeathAfterAttack : MonoBehaviour

    {
        private Damageable _damageable;

        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
        }

        public void OnAttack()
        {
            _damageable.Health = 0;
        }
    }
}