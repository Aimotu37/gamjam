using UnityEngine;
using S4;

namespace S4
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        // S4 独有：Task 引用
        [Header("任务面板")]
        public Task_S4 taskS4_Instance;
        public override GameObject TaskModuleObject =>
            taskS4_Instance != null ? taskS4_Instance.gameObject : null;
        public Task_S4 TaskS4 => taskS4_Instance;

        // S4 独有：初始状态
        protected override RoomState InitialState => RoomState.Intro;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S4 GameManager]</color> 初始化完成");
        }

        // S4 独有：进入状态时刷新可交互物件
        protected override void OnStateEntered(RoomState newState)
        {
            if (newState == RoomState.AllTasksDone || newState == RoomState.PasswordCollecting)
            {
                if (dialogueManager != null && !dialogueManager.dialoguePanel.activeInHierarchy)
                {
                    uiBlockCount = 0;
                }
            }

            foreach (var item in FindObjectsOfType<InteractableItem>())
                item.RefreshInteractable();
        }
    }
}
