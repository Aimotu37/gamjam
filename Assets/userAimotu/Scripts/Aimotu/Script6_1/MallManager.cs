using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallManager : MonoBehaviour
{

    //1.切换恐怖商场状态-走到商场最右边触发+播放视频！
    //2.走回到电视机附近吓人(播放视频！)
    //3.走回最左侧弹出回到街道选项

    //地图
    public GameObject _backgroundMall;
    public GameObject _backgroundScaryMall;

    //背景视频
    public GameObject _scrayTransitionVideo;
    public GameObject _scaryTVVideo;

    //玩家位置
    public Rigidbody2D _playerRb;
    public SpriteRenderer _playerSpriteRender;

    //是否已进入恐怖商场状态
    private bool _creepyMall = false;

    public List<StateAction> _ScaryTransitionActions;
    public List<StateAction> _ScaryTVActions;
    public List<StateAction> _exitMallActions;
    private bool isExecuting = false; // 类成员变量

    //是否往前走后回头
    private bool _walked = false;
    private bool _lookBack = false;
    private bool _inTV = false;



    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerRb.position.x <= -30)
        {
            LeaveMall();
        }
        else
        {
            _inTV = true;
            ScaryTV();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_playerRb.position.x > -20)
        {
            _inTV = false;
        }
    }

    private void OnMouseDown()
    {
        if (_inTV)
        {
            ScaryTV();
        }

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _walked = true;
        }
        if (_walked && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            EnterScaryMallState();
        }

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
            //_backgroundMall.SetActive(false);
            _scrayTransitionVideo.SetActive(true);
            _backgroundScaryMall.SetActive(true);

        }

    }

    public void ScaryTV()
    {
        if (_creepyMall && !isExecuting)
        {
            Debug.Log("电视闪屏");
            StartCoroutine(ExecuteActions(_ScaryTVActions));
            _scaryTVVideo.SetActive(true);

        }
    }

    void LeaveMall()
    {
        if (_creepyMall && !isExecuting)
        {
            Debug.Log("弹出选项");
            StartCoroutine(ExecuteActions(_exitMallActions));
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

