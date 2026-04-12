using UnityEngine;
using S5;

namespace S5
{
    public class GameManager : SceneManagerBase
    {
        public static GameManager Instance;
        public Task_S5 taskS5;
        public override GameObject TaskModuleObject => taskS5 != null ? taskS5.gameObject : null;

        //S5 应该是要建一个新的room state？
        protected override RoomState InitialState => RoomState.Intro;
        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }


    }
}