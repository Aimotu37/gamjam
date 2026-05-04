// 1. 收集密码线索时同步写入 GlobalData
// 2. 收集日记时调用 GlobalData.UnlockDiary()
using UnityEngine;
using TMPro;

namespace S5
{
    public class Task_S5 : MonoBehaviour, TaskModule
    {

        // 每交互的完成状态
        private bool WindowInteracted;
        private bool SleepPillInteracted;
        private bool FishInteracted;

        private bool ComputerInteracted;


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
        }

        //S5 独有完成所有物品交互触发手机状态
        public void InteractedItem(ItemType type)
        {


            switch (type)
            {
                case ItemType.Window:
                    Debug.Log("WindowInteracted");
                    WindowInteracted = true;
                    break;
                case ItemType.SleepPill:
                    Debug.Log("SleepPillInteracted");
                    SleepPillInteracted = true;
                    break;

                case ItemType.Fish:
                    Debug.Log("FishInteracted");
                    FishInteracted = true;
                    break;
                case ItemType.ComputerS5:
                    Debug.Log("ComputerInteracted");
                    ComputerInteracted = true;
                    break;

            }


            if (WindowInteracted && SleepPillInteracted && ComputerInteracted && FishInteracted)
            {
                GameManager.Instance.EnterState(RoomState.S5_InteractionDone);
                Debug.Log("完成S5全部交互");
            }


        }





        public bool IsAllCompleted() => FishInteracted && WindowInteracted && SleepPillInteracted && ComputerInteracted;
        private void UpdateTaskUI()
        { }
        public void UpdateUI() => UpdateTaskUI();
    }




}