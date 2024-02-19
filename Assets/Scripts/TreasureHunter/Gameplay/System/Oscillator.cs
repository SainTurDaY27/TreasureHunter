using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class Oscillator : MonoBehaviour
    {
        
        public float oscillationValue = 0.75f;
        private Vector2 originalPosition;

        private void Start()
        {
            originalPosition = transform.position;
        }

        private void Update()
        {
            transform.position = new Vector2(transform.position.x,
                originalPosition.y + Mathf.Sin(Time.time) * oscillationValue);
        }
    }
}