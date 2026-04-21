using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{

    //1.播放对话-视频-转

    public List<StateAction> _YaoyaoCarActions;
    private bool isExecuting = false; // 类成员变量




    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(ExecuteActions(_YaoyaoCarActions));
    }




    private IEnumerator ExecuteActions(List<StateAction> actions)
    {
        isExecuting = true;
        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute();
        }
        isExecuting = false; // 结束后解锁
    }

}

