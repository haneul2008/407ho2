﻿using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Work.JW.Code.Entities
{
    public class EntityMover : NetworkBehaviour, IEntityComponent
    {
        public UnityEvent<float> OnXInput;
        protected Rigidbody2D _rigidCompo;

        public bool CanMove { get; set; } = true;

        protected virtual void FixedUpdate()
        {
            if (CanMove)
            {
                float targetSpeed = _movementX * _moveSpeed * _moveSpeedMultiplier;
                float speedDifference = targetSpeed - _rigidCompo.linearVelocityX;
                float accelerationRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : deceleration;
                
                float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) *
                                 Mathf.Sign(speedDifference);

                _rigidCompo.AddForce(Vector2.right * movement, ForceMode2D.Force);
            }
            
            _rigidCompo.linearVelocityY = Mathf.Clamp(_rigidCompo.linearVelocityY, -_limitYSpeed, _limitYSpeed);
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            
            _rigidCompo = entity.GetComponent<Rigidbody2D>();

            _moveSpeedMultiplier = 1;
            _originalGravityScale = _rigidCompo.gravityScale;
        }

        public virtual float SetMoveSpeedMultiplier(float speed)
        {
            return _moveSpeedMultiplier = speed;
        }

        public virtual float SetLimitYSpeed(float speed)
        {
            return _limitYSpeed = speed;
        }

        public virtual void SetGravityScale(float value)
        {
            _rigidCompo.gravityScale = value;
        }

        public void AddVelocityY(float value) => _rigidCompo.linearVelocityY += value;

        public float GetJumpPower() => jumpPower;
        public void SetJumpPower(float value) => jumpPower = value;

        public virtual void AddForce(Vector2 force) => _rigidCompo.AddForce(force, ForceMode2D.Impulse);

        public virtual void AddJump() => AddForce(new Vector2(0, jumpPower));

        public virtual void SetMovementX(float xMovement)
        {
            _movementX = Mathf.Abs(xMovement) > 0 ? Mathf.Sign(xMovement) : 0;
            OnXInput?.Invoke(_movementX);
        }

        public void ResetVelocity(bool isVelocityY)
        {
            if (isVelocityY) _rigidCompo.linearVelocityY = 0;
            else _rigidCompo.linearVelocityX = 0;
        }

        public void StopImmediately(bool isYInclude)
        {
            if (isYInclude)
                _rigidCompo.linearVelocity = Vector2.zero;
            else
                _rigidCompo.linearVelocityX = 0;

            _movementX = 0;
        }

        public Vector2 GetVelocity() => _rigidCompo.linearVelocity;

        public bool IsWallDetected(float facingDir) =>
            Physics2D.Raycast(wallCheckerTrm.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

        public virtual bool IsGroundDetected()
        {
            float boxHeight = 0.05f;
            Vector2 boxSize = new Vector2(groundBoxWidth, boxHeight);
            return Physics2D.BoxCast(groundCheckerTrm.position, boxSize, 0, Vector2.down, groundCheckDistance,
                whatIsGround);
        }

        public void AddMaxJumpTime(float time) => _curMaxJumpTime = time + maxJumpTime;
        public void AddMinJumpTimeOut(float time) => _curMinJumpTimeOut = time + minJumpTimeOut;
        public bool IsMaxJumpTimeOver() => _curMaxJumpTime < Time.time;
        public bool IsMinJumpTimeOutOver() => _curMinJumpTimeOut < Time.time;

        #region Data

        protected Entity _entity;
        protected float _movementX;
        protected float _moveSpeed = 5f;
        protected float _moveSpeedMultiplier;

        protected float acceleration = 10f; // 가속도
        public float deceleration = 100f; // 감속도
        protected float velocityPower = 0.82f; // 부드러운 가속을 위한 보정값

        [SerializeField] private float _limitYSpeed = 9.8f;
        private float _originalGravityScale;

        [Header("Jump Data")] [SerializeField] private float jumpPower = 6f;
        [SerializeField] private float maxJumpTime = 0.5f;
        [SerializeField] private float minJumpTimeOut = 0.1f;
        private float _curMaxJumpTime;
        private float _curMinJumpTimeOut;

        #endregion

        #region Check section

        [Header("Wall checker")] [SerializeField]
        private Transform wallCheckerTrm;

        [SerializeField] private float wallCheckDistance;

        [Header("Ground checker")] [SerializeField]
        Transform groundCheckerTrm;

        [SerializeField] private float groundCheckDistance, groundBoxWidth;
        [SerializeField] private LayerMask whatIsGround;

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (wallCheckerTrm != null)
            {
                Vector2 fromVec = wallCheckerTrm.position;
                Vector2 toVec = new Vector2(wallCheckerTrm.position.x + wallCheckDistance, wallCheckerTrm.position.y);
                Gizmos.DrawLine(fromVec, toVec);
            }

            Gizmos.color = Color.red;

            if (groundCheckerTrm != null)
            {
                Gizmos.DrawWireCube(groundCheckerTrm.position - new Vector3(0, groundCheckDistance * 0.5f),
                    new Vector3(groundBoxWidth, groundCheckDistance, 1f));
            }
        }
#endif
    }
}