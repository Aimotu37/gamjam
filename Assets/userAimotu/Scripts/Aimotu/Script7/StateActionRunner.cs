using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateActionRunner : MonoBehaviour
{
    [Header("依次执行的 Action 列表")]
    public List<StateAction> actions = new List<StateAction>();

    private bool _running;

    public void Run()
    {
        if (_running) return;
        StartCoroutine(RunRoutine());
    }

    private IEnumerator RunRoutine()
    {
        _running = true;
        foreach (var a in actions)
            if (a != null) yield return a.Execute();
        _running = false;
    }
}