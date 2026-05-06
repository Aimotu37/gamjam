// 放在 Script6_1/ 文件夹下

using UnityEngine;
using S61;

namespace S61
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        // S61 独有：Task 引用
        [Header("任务面板")]
        public Task_S61 taskS61_Instance;
        public override GameObject TaskModuleObject =>
            taskS61_Instance != null ? taskS61_Instance.gameObject : null;
        public Task_S61 TaskS61 => taskS61_Instance;

        // S61 独有：初始状态
        protected override RoomState InitialState => RoomState.Dream2_Street;

        protected override void Awake()
        {

            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S61 GameManager]</color> 初始化完成");
        }
    }
}
