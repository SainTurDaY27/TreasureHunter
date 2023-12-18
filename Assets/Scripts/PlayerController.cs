using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private TouchingDirections _touchingDirections;
    private Damageable _damageable;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float jumpImpulse = 10f;
    public float jumpCutMultiplier = .5f;
    public float fallGravityMultiplier = 2f;
    public float gravityScale = 1f;
    private Vector2 _moveInput;


    private bool _isRunning = false;

    public float CurrentSpeed
    {
        get
        {
            if (IsMoving && !_touchingDirections.IsOnWall)
            {
                if (_isRunning) return runSpeed;
                return walkSpeed;
            }

            return 0;
        }
    }

    public bool IsMoving
    {
        get => _animator.GetBool(AnimationStrings.IsMoving);
        set => _animator.SetBool(AnimationStrings.IsMoving, value);
    }

    [SerializeField] private bool isFacingRight = true;

    public bool IsFacingRight
    {
        get => isFacingRight;
        private set
        {
            if (isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            isFacingRight = value;
        }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _touchingDirections = GetComponent<TouchingDirections>();
    }

    public bool IsAlive => _animator.GetBool(AnimationStrings.IsAlive);

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = _moveInput != Vector2.zero;
            SetFacingDirection(_moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started && _touchingDirections.IsGround && IsAlive)
        {
            _isRunning = true;
        }
        else if (context.canceled)
        {
            _isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _touchingDirections.IsGround && IsAlive)
        {
            _animator.SetTrigger(AnimationStrings.JumpTrigger);
            _rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
            // _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
        else if (context.canceled)
        {
            _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetTrigger(AnimationStrings.AttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (IsAlive)
        {
            _rb.velocity = new Vector2(knockback.x, _rb.velocity.y + knockback.y);
        }
    }

    private void FixedUpdate()
    {
        if (!_damageable.LockVelocity)
        {
            if (IsMoving)
            {
                _rb.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }

        // Jump gravity
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            _rb.gravityScale = gravityScale;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)

        {
            // Face the left
            IsFacingRight = false;
        }
    }
}