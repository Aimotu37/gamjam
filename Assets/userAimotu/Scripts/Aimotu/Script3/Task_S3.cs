using UnityEngine;

namespace S3
{
    public class Task_S3 : MonoBehaviour, TaskModule
    {
        private bool _notebookViewed;
        private bool _fishDecorViewed;
        private bool _computerViewed;
        private bool _melatoninViewed;

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.OnRoomStateChanged += HandleStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnRoomStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(RoomState newState) { }

        public void MarkViewed(ItemType type)
        {
            switch (type)
            {
                case ItemType.S3_PasswordNotebook: _notebookViewed = true; break;
                case ItemType.S3_FishDecor: _fishDecorViewed = true; break;
                case ItemType.S3_Computer: _computerViewed = true; break;
                case ItemType.S3_Melatonin: _melatoninViewed = true; break;
            }
            TryAdvanceState();
        }

        private void TryAdvanceState()
        {
            if (IsAllViewed() && GameManager.Instance?.CurrentState == RoomState.S3_Exploring)
                GameManager.Instance.EnterState(RoomState.S3_AllItemsViewed);
        }

        public bool IsAllViewed() =>
            _notebookViewed && _fishDecorViewed && _computerViewed && _melatoninViewed;

        public bool IsAllCompleted() => IsAllViewed();
        public void UpdateUI() { }
    }
}