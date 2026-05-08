using UnityEngine;
using System.Collections;

public abstract class StateAction : ScriptableObject
{
    [Header("在哪个状态下触发")]
    public RoomState triggerState;
    public abstract IEnumerator Execute();
    protected IGameManager GetManager()
    {
        var manager = Object.FindAnyObjectByType<SceneManagerBase>(
            FindObjectsInactive.Exclude) as IGameManager; // 只加 Exclude 非激活
        if (manager == null)
            Debug.LogError("[StateAction] 找不到 GameManager");
        return manager;
    }

}
