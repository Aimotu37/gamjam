// 放在 Script6/ 文件夹下

using UnityEngine;
using S6;

namespace S6
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        // S6 独有：Task 引用
        [Header("任务面板")]
        public Task_S6 taskS6_Instance;
        public override GameObject TaskModuleObject =>
            taskS6_Instance != null ? taskS6_Instance.gameObject : null;
        public Task_S6 TaskS6 => taskS6_Instance;

        // S6 独有：初始状�?
        protected override RoomState InitialState => RoomState.S6_Bedroom_Intro;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S6 GameManager]</color> 初始化完");
        }
        protected override void OnStateEntered(RoomState newState)
        {
            if (newState == RoomState.Dream2_Bedroom)
                taskS6_Instance?.ShowTaskUI();

            foreach (var item in FindObjectsOfType<InteractableItem>())
                item.RefreshInteractable();
        }
    }
}
