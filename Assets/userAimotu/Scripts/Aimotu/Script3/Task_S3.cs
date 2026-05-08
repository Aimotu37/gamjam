using UnityEngine;

namespace S3
{
    public class Task_S3 : MonoBehaviour, TaskModule
    {
        private bool _notebookViewed;
        private bool _fishDecorViewed;
        private bool _computerViewed;
        private bool _melatoninViewed;
        private void Awake()
        {
            GameManager.OnRoomStateChanged += HandleStateChanged;
        }


        private void Start()
        {
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
                case ItemType.S3_PasswordNotebook: GlobalData.S3_NotebookViewed = true; break;
                case ItemType.S3_FishDecor: GlobalData.S3_FishDecorViewed = true; break;
                case ItemType.S3_Computer: GlobalData.S3_ComputerViewed = true; break;
                case ItemType.S3_Melatonin: GlobalData.S3_MelatoninViewed = true; break;
            }
            TryAdvanceState();
        }

        private void TryAdvanceState()
        {
            Debug.Log($"[Task_S3] GameManager.Instance InstanceID={GameManager.Instance?.GetInstanceID()}");
            Debug.Log($"[Task_S3] TryAdvanceState: notebook={GlobalData.S3_NotebookViewed} fish={GlobalData.S3_FishDecorViewed} computer={GlobalData.S3_ComputerViewed} melatonin={GlobalData.S3_MelatoninViewed}"); 
            Debug.Log($"[Task_S3] CurrentState={GameManager.Instance?.CurrentState}, Instance={GameManager.Instance}");

            if (IsAllViewed() && GameManager.Instance?.CurrentState == RoomState.S3_Exploring)
            {
                Debug.Log("[Task_S3] 条件满足，准备切换状态");
                GameManager.Instance.EnterState(RoomState.S3_AllItemsViewed);
            }
            var all = Object.FindObjectsByType<SceneManagerBase>(
                FindObjectsInactive.Include, 
                FindObjectsSortMode.None);
            foreach (var m in all)
                Debug.Log($"[Found Manager] name={m.name} ID={m.GetInstanceID()} state={m.CurrentState} scene={m.gameObject.scene.name}");
        }
        public bool IsAllViewed() =>
             GlobalData.S3_NotebookViewed && GlobalData.S3_FishDecorViewed && GlobalData.S3_ComputerViewed && GlobalData.S3_MelatoninViewed;
        public bool IsAllCompleted() => IsAllViewed();
        public void UpdateUI() { }
    }
}