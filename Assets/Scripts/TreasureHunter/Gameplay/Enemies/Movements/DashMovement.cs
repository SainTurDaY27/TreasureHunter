using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashMovement : MonoBehaviour
    {
        public Vector2 dashForce;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Dash()
        {
            float sign = Mathf.Sign(transform.localScale.x);
            _rb.AddForce(new Vector2(dashForce.x * sign, dashForce.y), ForceMode2D.Impulse);
            
            
        }

    }
}