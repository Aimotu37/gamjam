// 1. 收集密码线索时同步写入 GlobalData
// 2. 收集日记时调用 GlobalData.UnlockDiary()
using UnityEngine;
using TMPro;
namespace S5
{
    public class Task_S5 : MonoBehaviour, TaskModule
    {
        //这里内容先复制的S4，记得改


        [Header("UI 文本引用")]
        public TextMeshProUGUI passwordTaskText;
        public TextMeshProUGUI diaryTaskText;
        private const int PASSWORD_TOTAL = 3;
        private const int DIARY_TOTAL = 1;

        // 每个位的密码状态
        private bool FishCollected => GlobalData.D1_Fish;
        private bool DollCollected => GlobalData.D1_Doll;
        private bool AwardCollected => GlobalData.D1_Award;
        private bool DiaryCollected => GlobalData.IsDiaryUnlocked(DiaryID.Diary1_FishAndBeads);


        private void Start()
        {
            // 必须在这里加一个延迟或初始化确保 GameManager 已经存在
            if (GameManager.Instance != null)
            {
                GameManager.OnRoomStateChanged += HandleStateChanged;
            }
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
        // 修改：每个密码位独立记录，防止玩家重复点击同一个物品导致计数错误。
        public void CollectPassword(ItemType type)
        {
            Debug.Log($"[Task_S4] CollectPassword 被调用：{type}，D1_Fish={GlobalData.D1_Fish}");

            bool changed = false;

            switch (type)
            {
                case ItemType.FishTank:
                    if (!GlobalData.D1_Fish) { GlobalData.D1_Fish = true; changed = true; }
                    break;
                case ItemType.Doll:
                    if (!GlobalData.D1_Doll) { GlobalData.D1_Doll = true; changed = true; }
                    break;
                case ItemType.Award:
                    if (!GlobalData.D1_Award) { GlobalData.D1_Award = true; changed = true; }
                    break;
            }
            if (changed)
            {
                int count = CalculatePasswordCount();
                Debug.Log($"<color=green>[Task S4]</color> 成功收集物件: {type}，当前总进度: {count}/3");
            }
            UpdateTaskUI();
        }
        private int CalculatePasswordCount()
        {
            return (FishCollected ? 1 : 0) + (DollCollected ? 1 : 0) + (AwardCollected ? 1 : 0);
        }

        public void CollectDiary()
        {
            if (!DiaryCollected)
            {
                GlobalData.UnlockDiary(DiaryID.Diary1_FishAndBeads);
                Debug.Log("<color=green>[Task S4]</color> 日记1 已解锁");
                UpdateTaskUI();
            }
        }

        public void ShowTaskUI()
        {
            gameObject.SetActive(true);
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }

            UpdateTaskUI();
        }

        private void UpdateTaskUI()
        {
            if (passwordTaskText == null || diaryTaskText == null) return;
            RoomState currentState = GameManager.Instance.CurrentState;
            if (currentState == RoomState.Intro || currentState == RoomState.ReadyToExit) return;
            gameObject.SetActive(true);
            int passwordCount = CalculatePasswordCount();
            passwordTaskText.text = $"找到 {passwordCount} / {PASSWORD_TOTAL} 位密码";
            diaryTaskText.text = $"找回 {(DiaryCollected ? 1 : 0)} / {DIARY_TOTAL} 篇日记";
            if (passwordCount == PASSWORD_TOTAL) passwordTaskText.text += " √";
            if (DiaryCollected) diaryTaskText.text += " √";
            // 全部完成时自动推进状态
            if (IsAllCompleted() && currentState == RoomState.PasswordCollecting)
            {
                Debug.Log("<color=lime>[Task S4]</color> 全部完成，切换到 AllTasksDone");
                GameManager.Instance.EnterState(RoomState.AllTasksDone);
            }
        }
        public bool IsAllCompleted() => FishCollected && DollCollected && AwardCollected && DiaryCollected;
        public void UpdateUI() => UpdateTaskUI();


    }

}