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

        private Coroutine _timedSummonCoroutine;

        public void InitiateSummon()
        {
            if (summonInSequence)
            {
                if (_timedSummonCoroutine == null)
                {
                    _timedSummonCoroutine = StartCoroutine(TimedSummon());
                }
            }
            else
            {
                foreach (var point in summonPoints)
                {
                    Summon(point);
                }
            }
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