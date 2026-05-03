// 追踪 Script3 成年卧室中四件物品是否已查看
// 四件物品全部查看后，自动切换到 S3_AllItemsViewed 状态（触发闪屏动画）
using UnityEngine;

namespace S3
{
    public class Task_S3 : MonoBehaviour
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
                case ItemType.S3_Notebook: _notebookViewed = true; break;
                case ItemType.S3_Fish: _fishDecorViewed = true; break;
                case ItemType.S3_Computer: _computerViewed = true; break;
                case ItemType.S3_Melatonin: _melatoninViewed = true; break;
            }
            Debug.Log($"<color=cyan>[Task S3]</color> MarkViewed: {type} | 进度: {ProgressCount()}/4");
            TryAdvanceState();
        }

        public bool IsAllViewed() => _notebookViewed && _fishDecorViewed && _computerViewed && _melatoninViewed;

        private int ProgressCount()
        {
            return (_notebookViewed ? 1 : 0) + (_fishDecorViewed ? 1 : 0)
                 + (_computerViewed ? 1 : 0) + (_melatoninViewed ? 1 : 0);
        }

        private void TryAdvanceState()
        {
            if (IsAllViewed() && GameManager.Instance?.CurrentState == RoomState.S3_Exploring)
            {
                Debug.Log("<color=lime>[Task S3]</color> 全部物品已查看，切换到 S3_AllItemsViewed");
                GameManager.Instance.EnterState(RoomState.S3_ReadyToSleep);
            }
        }
    }
}
