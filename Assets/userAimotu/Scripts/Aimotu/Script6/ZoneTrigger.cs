using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 挂在带 Collider2D (Is Trigger) 的 GameObject 上
// 玩家进入时执行 action 序列，可限制触发状态
public class ZoneTrigger : MonoBehaviour
{
    [Tooltip("为空则任意状态都触发")]
    public List<RoomState> eligibleStates = new List<RoomState>();
    public List<StateAction> actions = new List<StateAction>();
    public bool triggerOnce = true;

    private bool _triggered = false;
    private bool _isExecuting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && _triggered) return;
        if (_isExecuting) return;
        if (!IsEligible()) return;

        _triggered = true;
        StartCoroutine(Execute());
    }

    private bool IsEligible()
    {
        if (eligibleStates == null || eligibleStates.Count == 0) return true;
        var manager = FindAnyObjectByType<SceneManagerBase>() as IGameManager;
        if (manager == null) return false;
        return eligibleStates.Contains(manager.CurrentState);
    }

    private IEnumerator Execute()
    {
        _isExecuting = true;
        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute();
        }
        _isExecuting = false;
    }
}
