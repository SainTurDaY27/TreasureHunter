using System.Collections;
using System.Collections.Generic;
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

        public void InitiateSummon()
        {
            for (int i = 0; i < summonPoints.Count; i++)
            {
                var point = summonPoints[i];
                if (summonInSequence)
                {
                    StartCoroutine(TimedSummon(point, i * sequenceDelay));
                }
                else
                {
                    Summon(point);
                }
            }
        }

        private GameObject Summon(Transform referencePoint)
        {
            return Instantiate(summonPrefab, referencePoint.position, summonPrefab.transform.rotation);
        }

        private IEnumerator TimedSummon(Transform referencePoint, float delay)
        {
            yield return new WaitForSeconds(delay);
            Summon(referencePoint);
        }
    }
}