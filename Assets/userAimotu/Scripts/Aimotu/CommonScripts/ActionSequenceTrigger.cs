using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequenceTrigger : MonoBehaviour
{
    [Header("按顺序执行的 Action 列表")]
    public List<StateAction> sequence;
    private IGameManager GetManager() => FindAnyObjectByType<SceneManagerBase>() as IGameManager;
    public void TriggerSequence()
    {
        if (sequence == null || sequence.Count == 0)
        {
            // 【红灯 2】：如果这里打印了，说明你 Inspector 里的列表是空的
            Debug.LogWarning("[ActionSequenceTrigger]Sequence 列表是空的！");
            return;
        }
        // 额外检查：防止连点
        // if (GameManager.Instance != null && GameManager.Instance.IsUIBlocking)
        Debug.Log("[ActionSequenceTrigger] TriggerSequence 触发");

        var manager = GetManager();
        if (manager != null)
        {
            (manager as MonoBehaviour)?.StartCoroutine(ExecuteRoutine());
        }
        else
        {
            // 如果没找到接口，回退到自己的协程
            StartCoroutine(ExecuteRoutine());
        }
    }

    private IEnumerator ExecuteRoutine()
    {
        foreach (var action in sequence)
        {
            if (action != null)
            {
                Debug.Log($"ActionSequenceTrigger] 执行：{action.name}");
                yield return action.Execute();
                Debug.Log($"[ActionSequenceTrigger] 完成：{action.name}");
            }
            else
            {
                // 【红灯 3】：如果列表里有空槽位（Missing），也会卡住
                Debug.LogError("[ActionSequenceTrigger] 列表中有空 Action，请检查 Inspector");
            }

            Debug.Log("Action 序列执行完毕");
        }
    }
}
