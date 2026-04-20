using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallManager : MonoBehaviour
{

    //1.切换恐怖商场状态-走到商场最右边触发+播放视频！
    //2.走回到电视机附近吓人(播放视频！)
    //3.走回最左侧弹出回到街道选项

    //地图边界
    public GameObject backgroundMall;
    public GameObject backgroundScaryMall;
    private float _minX; // 左边s
    private float _maxX;  // 右边

    //玩家位置
    public Rigidbody2D _playerRb;
    public SpriteRenderer _playerSpriteRender;

    //是否已进入恐怖商场状态
    private bool _creepyMall = false;

    public List<StateAction> _ScaryTransitionActions;
    private bool isExecuting = false; // 类成员变量




    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnterScaryMallState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerRb.position.x <= -30)
        {
            LeaveMall();
        }
        else
        {
            ScaryTV();
        }

    }


    void Update()
    {

    }


    void EnterScaryMallState()
    {
        if (_creepyMall == false)
        {
            StartCoroutine(ExecuteActions(_ScaryTransitionActions));
            Debug.Log("弹出对话");
            Debug.Log("播放视频");
            Debug.Log("切换背景");
            _creepyMall = true;
            backgroundMall.SetActive(false);
            backgroundScaryMall.SetActive(true);
        }

    }

    void ScaryTV()
    {
        if (_creepyMall == true)
        {
            Debug.Log("电视闪屏");
        }
    }

    void LeaveMall()
    {
        if (_creepyMall == true)
        {
            Debug.Log("弹出选项");
        }
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

