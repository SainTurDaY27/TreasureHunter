using System;
using System.Collections;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Events;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    /// <summary>
    /// Summon attack that targets the player.
    /// </summary>
    public class TargetedSummonAttack : MonoBehaviour
    {
        public GameObject summonPrefab;
        public Vector2 summonOffset;
        public float summonTime = 3f;
        public float summonDelay = 5f;
        public UnityEvent<GameObject> afterSummon;

        private Animator _animator;
        private Coroutine _summonCoroutine;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Summon()
        {
            if (_summonCoroutine == null)
            {
                _summonCoroutine = StartCoroutine(nameof(SummonCoroutine));
            }
        }


        public IEnumerator SummonCoroutine()
        {
            yield return new WaitForSeconds(summonDelay);
            _animator.SetTrigger(AnimationStrings.Summon);
            var player = GameObject.FindWithTag("Player");
            if (player == null) yield break;
            Vector2 playerPosition = player.transform.position;
            var summonPosition = playerPosition + summonOffset;
            yield return new WaitForSeconds(summonTime);
            var summonedEnemy = Instantiate(summonPrefab, summonPosition, transform.rotation);
            afterSummon?.Invoke(summonedEnemy);
            _summonCoroutine = null;
        }
    }
}