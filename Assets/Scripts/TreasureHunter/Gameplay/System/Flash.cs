using System;
using System.Collections;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class Flash : MonoBehaviour
    {
        public Color flashColor;
        public float flashAmount = 0.1f;
        private SpriteRenderer _spriteRenderer;
        private Material _material;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Init();

        }

        private void Init()
        {
            _material = _spriteRenderer.material;
        }

        public void DisplayFlash(float duration = 0.5f)
        {
            _material.SetColor("_FlashColor", flashColor);
            LeanTween.value(gameObject, f =>
            {
                _material.SetFloat("_FlashAmount", f);
            }, flashAmount, 0f, duration).setEaseInSine();
        }
    }
}