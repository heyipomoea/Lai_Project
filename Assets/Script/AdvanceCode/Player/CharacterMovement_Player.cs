using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.AnnotationUtility;

namespace AdvanceCode
{
    [RequireComponent(typeof(Rigidbody), typeof(Health_Player))]
    public class CharacterMovement_Player : CharacterMovementBase
    {
        [Header("產簿笆把计")]
        private Vector2 deltaInput;
        public Vector2 DeltaInput => deltaInput;
        private Quaternion deltaRot;

        [SerializeField] private float JumpForce = 5f;
        private float groundCheckDistance = 0.1f;

        [Header("產ю阑2把计")]
        [SerializeField] private float Fire2Interval = .5f;
        [SerializeField] private float Fire2Damage = 3.0f;
        [SerializeField] private float Fire2Force = 5.0f;
        private float fire2Cooldown = 0f;

        private EventHandler<FireEventArgs> fire2Received;
        public event EventHandler<FireEventArgs> Fire2Received
        {
            add { fire2Received += value; }
            remove { fire2Received -= value; }
        }

        private EventHandler jumpReceived;
        public event EventHandler JumpReceived
        {
            add { jumpReceived += value; }
            remove { jumpReceived -= value; }
        }

        private Camera cam;
        private CapsuleCollider capsuleCollider;

        protected override void Start()
        {
            base.Start();
            cam = Camera.main;
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            if (deltaPos.magnitude != 0) rb.MovePosition(transform.position + deltaPos);
            if (deltaRot.eulerAngles.magnitude != 0) rb.MoveRotation(deltaRot);
        }

        protected override void Update()
        {
            base.Update();
            OnJump();
        }

        protected override void OnMove()
        {
            deltaInput.y = Input.GetAxis("Vertical");
            deltaInput.x = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(deltaInput.x, 0f, deltaInput.y);
            Quaternion noTiltRotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
            deltaPos = noTiltRotation * movement * MoveSpeed * Time.deltaTime;
        }

        protected override void OnFire()
        {
            if (Input.GetButtonDown("Fire1") && fire1Cooldown <= 0f) { Fire1(); }
            if (Input.GetButtonDown("Fire2") && fire2Cooldown <= 0f) { Fire2(); }
        }

        private void Fire2()
        {
            FireEventArgs fireEventArgs = new FireEventArgs(Fire2Damage, Fire2Force);
            fire2Received?.Invoke(this, fireEventArgs);
            fire2Cooldown = Fire2Interval;
        }

        protected override void OnCoolDown()
        {
            base.OnCoolDown();
            if (fire2Cooldown > 0f) { fire2Cooldown -= Time.deltaTime; }
        }

        protected override void OnRotation()
        {
            Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 targetPoint = cameraRay.GetPoint(rayLength);
                Quaternion targetRot = Quaternion.LookRotation(targetPoint - transform.position);
                targetRot = targetRot.normalized;
                targetRot.x = targetRot.z = 0;
                deltaRot = Quaternion.Slerp(transform.rotation, targetRot, RotationSpeed * Time.deltaTime);
            }
        }

        private void OnJump()
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                jumpReceived?.Invoke(this, EventArgs.Empty);
                rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
            }
        }

        public bool IsGrounded()
        {
            //capsuleCollider
            //if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
            if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, out _, capsuleCollider.bounds.extents.y + groundCheckDistance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            fire2Received = null;
            jumpReceived = null;
        }
    }

}

