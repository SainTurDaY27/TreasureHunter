using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Collider2D))]
    public class SizeAltar : MonoBehaviour
    {
        [SerializeField]
        private PopupTextObject _popupTextObject;

        private Collider2D _collider2D;
        public Transform destinationAltar;
        public SizeChangeMode mode = SizeChangeMode.Shrink;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _popupTextObject.SetPopupAvailability(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var playerController = GetPlayerController(other);
            if (playerController == null) return;
            playerController.OnEnterAltar(mode, destinationAltar.position);
        }

        private PlayerController GetPlayerController(Collider2D other)
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("The player somehow has no player controller");
                return playerController;
            };
            return playerController;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null) return;
            playerController.OnExitAltar();
        }

        private void Update()
        {
            if (!DataManager.Instance.PlayerData.HasSkill(SkillKey.Shrink))
            {
                return;
            }
            if (_popupTextObject.IsPopupAvailable == true)
            {
                return;
            }
            _popupTextObject.SetPopupAvailability(true);
        }
    }
}