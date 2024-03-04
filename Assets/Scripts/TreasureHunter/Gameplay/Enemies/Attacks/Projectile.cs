using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        public Vector2 moveSpeed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // We don't use real physics here.
            var directionX = Mathf.Sign(transform.localScale.x);
            _rb.velocity = new Vector2(moveSpeed.x * directionX, moveSpeed.y);
        }
    }
}