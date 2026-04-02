using UnityEngine;
using System.Collections;

public abstract class StateAction : ScriptableObject
{
    [Header("在哪个状态下触发")]
    public RoomState triggerState;
    public abstract IEnumerator Execute();
    protected IGameManager GetManager()
    {
        var manager = Object.FindAnyObjectByType<SceneManagerBase>() as IGameManager;
        if (manager == null)
            Debug.LogError("[StateAction] 找不到 GameManager，请确认场景内有继承 SceneManagerBase 的组件");
        return manager;
    }

}
