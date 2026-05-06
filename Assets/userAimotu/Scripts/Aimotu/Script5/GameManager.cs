using UnityEngine;
using S5;

namespace S5
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;

        public Task_S5 taskS5_Instance;
        public override GameObject TaskModuleObject =>
            taskS5_Instance != null ? taskS5_Instance.gameObject : null;
        public Task_S5 TaskS5 => taskS5_Instance;

        //S5独有状态trigger event





        //S5 应该是要建一个新的room state？
        protected override RoomState InitialState => RoomState.S5_Intro;
        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            Debug.Log("<color=green>[S5 GameManager]</color> 初始化完成");
        }


    }
}