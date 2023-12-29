using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TreasureHunter.Gameplay.System
{
    public class CameraController : MonoBehaviour
    {
        public float sensitivity = 1f;
        public CinemachineVirtualCamera virtualCamera;

        private float initialYOffset;
        private Vector2 initialOffset;

        private void Awake()
        {
            initialOffset = Vector2.zero;
            // If this does not exists, LeanTween will only work once.
            // The alternate solution is to reload domain.
            LeanTween.reset();
        }

        public void VerticalLook(InputAction.CallbackContext context)
        {
            if (!virtualCamera)
            {
                Debug.Log("Cannot find camera");
                return;
            }

            var lookInput = context.ReadValue<Vector2>();
            var framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            var offset = framingTransposer.m_TrackedObjectOffset;

            if (!framingTransposer)
            {
                Debug.Log("framing transposer went on vacation, never came back");
                return;
            }

            if (context.started)
            {
                // Pan camera to desired location.
                var destinationOffset = lookInput * sensitivity;
                LeanTween.value(gameObject, value => { framingTransposer.m_TrackedObjectOffset = value; },
                    offset, (Vector3)destinationOffset, 1f).setEaseInOutSine();
            }
            else if (context.canceled)
            {
                LeanTween.value(gameObject, value => { framingTransposer.m_TrackedObjectOffset = value; },
                    offset, Vector3.zero, 1f).setEaseInOutSine();
            }
        }
    }
}