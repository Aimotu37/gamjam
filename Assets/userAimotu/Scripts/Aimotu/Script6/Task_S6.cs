using UnityEngine;
using TMPro;
namespace S6
{
    public class Task_S6 : MonoBehaviour, TaskModule
    {
        [Header("UI 文本引用")]
        public TextMeshProUGUI diaryTaskText; // 对应左上角 "找回 0 / 6 篇日记"   
                                              // DiaryID 和显示名的对应关系，顺序和游戏流程一致
        private static readonly (DiaryID id, string name)[] DiarySlots =
        {
            (DiaryID.Diary2_SnackCart,          "小吃摊"),
            (DiaryID.Diary3_SummerTV,           "暑假电视"),
            (DiaryID.Diary4_SnackGachaToyPhone, "零食扭蛋玩具电话"),
            (DiaryID.Diary5_Stationery,         "文具店的笔"),
            (DiaryID.Diary6_BooksAndMagazines,  "杂志和教辅书"),
            (DiaryID.Diary7_Internet,           "因特网"),
        };

        private void Start()
        {// 必须在这里加一个延迟或初始化确保 GameManager 已经存在
            if (GameManager.Instance != null) GameManager.OnRoomStateChanged += HandleStateChanged;
        }
        private void OnDestroy()
        {
            // 记得取消注册，否则切关会报错
            GameManager.OnRoomStateChanged -= HandleStateChanged;
        }
        private void HandleStateChanged(RoomState newState)
        {
            // 每次切换状态时，强行刷一次 UI 和检查
            UpdateTaskUI();
        }


        // index 对应 DiarySlots 数组的下标（0~5）
        public void CollectDiary(int index, string customName = "")
        {
            if (index < 0 || index >= DiarySlots.Length) return;

            var (id, defaultName) = DiarySlots[index];

            if (!GlobalData.IsDiaryUnlocked(id))
            {
                string displayName = string.IsNullOrEmpty(customName) ? defaultName : customName;
                GlobalData.UnlockDiary(id); // 写入全局，同时触发 OnDiaryUnlocked 事件
                Debug.Log($"<color=green>[Task S6]</color> 日记解锁：{displayName}");
                UpdateTaskUI();
            }
        }
        public void ShowTaskUI()
        {
            this.gameObject.SetActive(true);
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }

            UpdateTaskUI();
        }

        private void UpdateTaskUI()
        {
            if (diaryTaskText == null) return;

            RoomState currentState = GameManager.Instance.CurrentState;
            // S6 场景的状态前缀判断
            bool isS6Scene = currentState.ToString().StartsWith("S6_")
                          || currentState == RoomState.Dream2_Bedroom
                          || currentState == RoomState.Dream2_Street
                          || currentState == RoomState.Dream2_TaskCompleted;
            if (!isS6Scene) return;
            gameObject.SetActive(true);
            int currentCount = CalculateCount();
            int total = DiarySlots.Length;
            diaryTaskText.text = $"找回 {currentCount} / {total} 篇日记";
            if (currentCount == total) diaryTaskText.text += " <color=yellow>√</color>";
        }
        private int CalculateCount()
        {
            int count = 0;
            foreach (var (id, _) in DiarySlots)
            {
                if (GlobalData.IsDiaryUnlocked(id)) count++;
            }
            return count;
        }
        public bool IsAllCompleted() => CalculateCount() == DiarySlots.Length;

        public void UpdateUI() => UpdateTaskUI();

    }
}