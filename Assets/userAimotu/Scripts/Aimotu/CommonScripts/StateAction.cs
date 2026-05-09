using UnityEngine;
using System.Collections;

public abstract class StateAction : ScriptableObject
{
    [Header("在哪个状态下触发")]
    public RoomState triggerState;
    public abstract IEnumerator Execute();
    protected IGameManager GetManager()
    {
        var all = Object.FindObjectsByType<SceneManagerBase>(
        FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var m in all)
        {
            if (m.gameObject.scene.name != "DontDestroyOnLoad")
                return m as IGameManager;
        }

        Debug.LogError("[StateAction] 找不到有效的 GameManager");
        return null;
    }

}
