using System.Collections;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TreasureHunter.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;
        private TouchingDirections _touchingDirections;
        private Flash _flash;
        private Damageable _damageable;
        private Vector2 _moveInput;
        private ProjectileLauncher _projectileLauncher;
        private Vector2 _originalScale;

        [SerializeField] private bool isFacingRight = true;

        public float walkSpeed = 1f;
        public float runSpeed = 2f;
        public float jumpImpulse = 10f;
        public Vector2 wallJumpForce = new(10f, 10f);
        public int dashImpulse = 10;
        public float jumpCutMultiplier = 0.5f;
        public float fallGravityMultiplier = 2f;
        public float gravityScale = 2.5f;
        public float maxFallYVelocity = 30f;
        public float wallJumpLerpAmount = 0.1f;
        public float wallJumpTime = 0.25f;
        public float fireballCooldown = 1f;
        public Vector2 fireKnockback = new(10, 0);
        public float shrinkScale = 0.5f;


        private bool _isRunning = false;
        private int jumpCount = 0;
        private bool _isWallJumping = false;
        private bool _canFire = true;
        private float _wallJumpStartTime;
        private bool _canAirDash = false;
        private bool _isShrunken = false;
        private bool _canChangeSize  = false;
        private SizeChangeMode _nextSizeChangeMode = SizeChangeMode.Shrink;
        private Vector2 _sizeChangeDestination;


        public bool IsAlive => _animator.GetBool(AnimationStrings.IsAlive);
        public bool ZeroGravity => _animator.GetBool(AnimationStrings.ZeroGravity);


        public float CurrentSpeed =>
            _isRunning ? runSpeed : walkSpeed;

        public bool IsMoving
        {
            get => _animator.GetBool(AnimationStrings.IsMoving);
            set => _animator.SetBool(AnimationStrings.IsMoving, value);
        }

        public bool IsFacingRight
        {
            get => isFacingRight;
            set
            {
                if (isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }

                isFacingRight = value;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _damageable = GetComponent<Damageable>();
            _touchingDirections = GetComponent<TouchingDirections>();
            _projectileLauncher = GetComponent<ProjectileLauncher>();
            _flash = GetComponent<Flash>();
            _originalScale = transform.localScale;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            if (IsAlive)
            {
                IsMoving = _moveInput.x != 0;
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

        // TODO: Refactor - extract each jump type to its own method
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (DataManager.Instance.PlayerData.HasSkill(SkillKey.WallJump))
                {
                    if (IsAlive && _touchingDirections.IsOnWall)
                    {
                        // _damageable.LockVelocity = true;
                        _isWallJumping = true;
                        _wallJumpStartTime = Time.time;

                        // clear all y velocity before jumping
                        _rb.velocity = new Vector2(_rb.velocity.x, 0);
                        _animator.SetTrigger(AnimationStrings.JumpTrigger);
                        var actualWallJumpForce = wallJumpForce;
                        actualWallJumpForce.x = IsFacingRight ? -actualWallJumpForce.x : actualWallJumpForce.x;
                        _rb.AddForce(actualWallJumpForce, ForceMode2D.Impulse);
                        return;
                    }
                }

                if (DataManager.Instance.PlayerData.HasSkill(SkillKey.DoubleJump))
                {
                    if (IsAlive && jumpCount < 2)
                    {
                        // clear all y velocity before jumping
                        _rb.velocity = new Vector2(_rb.velocity.x, 0);
                        _animator.SetTrigger(AnimationStrings.JumpTrigger);
                        _rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    if (_touchingDirections.IsGround && IsAlive)
                    {
                        _animator.SetTrigger(AnimationStrings.JumpTrigger);
                        _rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
                        // _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
                    }
                }
            }
            else if (context.canceled)
            {
                if (jumpCount >= 2) return;
                jumpCount++;
                _rb.AddForce((1 - jumpCutMultiplier) * _rb.velocity.y * Vector2.down, ForceMode2D.Impulse);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _animator.SetTrigger(AnimationStrings.AttackTrigger);
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!DataManager.Instance.PlayerData.HasSkill(SkillKey.Dash))
            {
                return;
            }

            if (context.started)
            {
                if (_touchingDirections.IsGround || (!_touchingDirections.IsGround && _canAirDash))
                {
                    if (!_touchingDirections.IsGround)
                    {
                        _canAirDash = false;
                    }

                    _animator.SetTrigger(AnimationStrings.DashTrigger);
                    if (IsFacingRight)
                    {
                        _rb.AddForce(Vector2.right * dashImpulse, ForceMode2D.Impulse);
                    }
                    else
                    {
                        _rb.AddForce(Vector2.left * dashImpulse, ForceMode2D.Impulse);
                    }
                }
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (!DataManager.Instance.PlayerData.HasSkill(SkillKey.Fireball))
            {
                return;
            }

            if (context.started && _canFire)
            {
                Debug.Log("Fire");
                _animator.SetTrigger(AnimationStrings.FireTrigger);
                // Apply knockback
                var actualKnockback =
                    new Vector2(-fireKnockback.x * Mathf.Sign(transform.localScale.x), fireKnockback.y);
                _rb.AddForce(actualKnockback, ForceMode2D.Impulse);
                _projectileLauncher.FireProjectile();
                StartCoroutine(SetFireCooldown());
            }
        }

        public void OnShrink(InputAction.CallbackContext context)
        {
            if (context.started && DataManager.Instance.PlayerData.HasSkill(SkillKey.Shrink) && _canChangeSize)
            {
                if (_nextSizeChangeMode == SizeChangeMode.Shrink)
                {
                    transform.localScale = new Vector3(shrinkScale, shrinkScale);
                }
                else
                {

                    transform.localScale = new Vector3(_originalScale.x, _originalScale.y);
                }

                _isShrunken = !_isShrunken;

                // fix local scale direction
                if (!IsFacingRight)
                {
                    var transform1 = transform;
                    var originalScale = transform1.localScale;
                    transform1.localScale = new Vector3(-originalScale.x, originalScale.y);
                }

                transform.position = _sizeChangeDestination;
            }
        }

        public void OnEnterAltar(SizeChangeMode mode, Vector2 destinationLocation)
        {
            _canChangeSize = true;
            _sizeChangeDestination = destinationLocation;
            _nextSizeChangeMode = mode;
        }

        public void OnExitAltar()
        {
            _canChangeSize = false;
        }

        private IEnumerator SetFireCooldown()
        {
            _canFire = false;
            yield return new WaitForSeconds(fireballCooldown);
            _canFire = true;
            _flash.DisplayFlash();
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
                // if (IsMoving)
                // {
                // _rb.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb.velocity.y);
                if (IsAlive)
                {
                    if (_isWallJumping)
                    {
                        PhysicalRun(wallJumpLerpAmount);
                    }
                    else
                    {
                        PhysicalRun();
                    }
                }
                else
                {
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                }
                // _rb.AddForce(Vector2.right * (_moveInput.x * CurrentSpeed), ForceMode2D.Force);
                // }
                // else 
                // {
                //     _rb.velocity = new Vector2(0, _rb.velocity.y);
                // }
                // Debug.Log("Not locked");
            }

            // Jump gravity
            if (ZeroGravity)
            {
                _rb.gravityScale = 0;
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }
            else if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = gravityScale * fallGravityMultiplier;
            }
            else
            {
                _rb.gravityScale = gravityScale;
            }

            // Reset jump count when player lands on the ground
            if (_touchingDirections.IsGround)
            {
                jumpCount = 0;
                _canAirDash = true;
            }

            // Clamp y velocity
            var yVelocity = _rb.velocity.y;
            var clampedVelocity = Mathf.Clamp(yVelocity, -maxFallYVelocity, Mathf.Infinity);
            _rb.velocity = new Vector2(_rb.velocity.x, clampedVelocity);
        }

        private void Update()
        {
            if (_isWallJumping && Time.time - _wallJumpStartTime > wallJumpTime)
            {
                _isWallJumping = false;
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

        private void PhysicalRun(float lerpAmount = 1)
        {
            float targetSpeed = _moveInput.x * CurrentSpeed;
            targetSpeed = Mathf.Lerp(_rb.velocity.x, targetSpeed, lerpAmount);
            float speedDiff = targetSpeed - _rb.velocity.x;
            // Yes, 50 is a magic number.
            float movement = speedDiff * 50;
            _rb.AddForce(Vector2.right * movement, ForceMode2D.Force);
        }
    }
}