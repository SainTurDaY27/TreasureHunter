using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TreasureHunter.Gameplay.System
{
    public class CameraController : MonoBehaviour
    {
        public float sensitivity = 1f;
        public float maxDistance = 5f;
        public CinemachineVirtualCamera virtualCamera;

        private float initialYOffset;

        private void Awake()
        {
            // Currently, there is only one camera.
            // Change if there will be more.
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            initialYOffset = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset
                .y;
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
                var clampedOffset = Mathf.Clamp(offset.y + lookInput.y * sensitivity, initialYOffset - maxDistance,
                    initialYOffset + maxDistance);
                framingTransposer.m_TrackedObjectOffset =
                    new Vector3(offset.x, clampedOffset, offset.z);
            }
            else if (context.canceled)
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(offset.x, initialYOffset, offset.z);
            }
        }
    }
}