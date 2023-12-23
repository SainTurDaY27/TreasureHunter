using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Movements
{
    public class WaypointsFlyingMovement : MonoBehaviour
    {
        public float flyingSpeed = 3f;

        // It is not going to be exact.
        public float waypointReachedDistance = 0.2f;
        public List<Transform> waypoints = new();

        private Animator _animator;
        private int waypointNumber = 0;
        private Transform _nextWaypoint;
        private Rigidbody2D _rb;

        private bool CanMove => _animator.GetBool(AnimationStrings.CanMove);
        private bool LockVelocity => _animator.GetBool(AnimationStrings.LockVelocity);

        private void Start()
        {
            if (waypoints.Count > 0)
            {
                _nextWaypoint = waypoints[waypointNumber];
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (CanMove && !LockVelocity)
            {
                Fly();
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }

        private void Fly()
        {
            if (!_nextWaypoint) return;

            Vector2 nextWayPointPosition = _nextWaypoint.position;
            Vector2 currentPosition = transform.position;
            Vector2 direction = (nextWayPointPosition - currentPosition).normalized;
            float distance = Vector2.Distance(nextWayPointPosition, currentPosition);
            _rb.velocity = direction * flyingSpeed;
            UpdateDirection();

            // Reached
            if (distance <= waypointReachedDistance)
            {
                waypointNumber++;
                if (waypointNumber >= waypoints.Count) waypointNumber = 0;
                _nextWaypoint = waypoints[waypointNumber];
            }
        }

        private void UpdateDirection()
        {
            Vector3 localScale = transform.localScale;
            Vector2 currentVelocity = _rb.velocity;

            if ((localScale.x > 0 && currentVelocity.x < 0) || (localScale.x < 0 && currentVelocity.x > 0))
            {
                transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            }
        }
    }
}