using UnityEngine;

namespace S3
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        [Header("任务面板")]
        public Task_S3 taskS3_Instance;
        public override GameObject TaskModuleObject =>
            taskS3_Instance != null ? taskS3_Instance.gameObject : null;

        protected override RoomState InitialState => RoomState.S3_Intro;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S3 GameManager]</color> 初始化完成");
        }

        protected override void OnStateEntered(RoomState newState)
        {
            foreach (var item in FindObjectsOfType<InteractableItem>())
                item.RefreshInteractable();
        }
    }
}