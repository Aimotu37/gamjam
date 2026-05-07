using UnityEngine;

namespace S7
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        [Header("任务面板（Script7 没有任务追踪，可留空）")]
        public GameObject taskUIRoot;
        public override GameObject TaskModuleObject => taskUIRoot;

        protected override RoomState InitialState => RoomState.S7_Intro;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S7 GameManager]</color> 初始化完成");
        }

        protected override void OnStateEntered(RoomState newState)
        {
            foreach (var item in FindObjectsOfType<InteractableItem>())
                item.RefreshInteractable();
        }
    }
}