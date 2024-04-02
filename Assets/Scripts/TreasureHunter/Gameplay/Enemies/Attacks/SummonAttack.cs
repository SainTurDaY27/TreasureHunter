using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    public class SummonAttack : MonoBehaviour
    {
        public List<Transform> summonPoints;
        public GameObject summonPrefab;
        public bool summonInSequence;
        public float sequenceDelay;
        public float summonDelay;
        public float summonCooldown = 10f;


        private float _summonTimer = 0f;
        private bool _firstTime = true;
        private Coroutine _timedSummonCoroutine;
        private Coroutine _summonCoroutine;

        private void Update()
        {
            _summonTimer += Time.deltaTime;
        }


        public void InitiateSummon()
        {
            if (_firstTime || _summonTimer > summonCooldown)
            {
                _summonCoroutine ??= StartCoroutine(nameof(ActualSummon));
                _summonTimer = 0;
            }
        }

        private IEnumerator ActualSummon()
        {
            _firstTime = false;
            yield return new WaitForSeconds(summonDelay);
            if (summonInSequence)
            {
                _timedSummonCoroutine ??= StartCoroutine(TimedSummon());
            }
            else
            {
                foreach (var point in summonPoints)
                {
                    Summon(point);
                }
            }

            _summonCoroutine = null;
        }

        private GameObject Summon(Transform referencePoint)
        {
            return Instantiate(summonPrefab, referencePoint.position, summonPrefab.transform.rotation);
        }

        private IEnumerator TimedSummon()
        {
            foreach (var point in summonPoints)
            {
                yield return new WaitForSeconds(sequenceDelay);
                Summon(point);
            }

            _timedSummonCoroutine = null;
        }
    }
}