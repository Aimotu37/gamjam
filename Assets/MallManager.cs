using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallManager : MonoBehaviour
{

    //1.切换恐怖商场状态-走到商场最右边触发+播放视频
    //2.走回到电视机附近吓人
    //3.走回最左侧弹出回到街道选项

    //地图边界
    public GameObject background;
    private float _minX; // 左边s
    private float _maxX;  // 右边

    //玩家位置
    public Rigidbody2D _playerRb;
    public SpriteRenderer _playerSpriteRender;

    //是否已进入恐怖商场状态
    private bool _creepyMall = false;



    void Start()
    {
        if (background != null)
        {
            // 1. 获取背景和主角的渲染器 (Renderer)

            Renderer bgRenderer = background.GetComponent<Renderer>();

            if (bgRenderer != null && _playerSpriteRender != null)
            {
                // 2. 获取主角的一半宽度 (extents.x 就是物体宽度的一半)
                float playerHalfWidth = _playerSpriteRender.bounds.extents.x;

                // 3. 计算左边界：背景的最左边 + 主角半宽
                // bounds.min.x 是物体在世界坐标中最左边的点
                _minX = bgRenderer.bounds.min.x + playerHalfWidth;

                // 4. 计算右边界：背景的最右边 - 主角半宽
                // bounds.max.x 是物体在世界坐标中最右边的点
                _maxX = bgRenderer.bounds.max.x - playerHalfWidth;

                Debug.Log($"Mall边界已自动计算: 左 {_minX} / 右 {_maxX}");
            }
            else
            {
                Debug.LogError("错误：背景或主角缺少 Renderer 组件！");
            }
        }
        else
        {
            Debug.LogError("请在 Inspector 面板中把【背景物体】拖进去！");
        }
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
        Debug.Log("弹出对话");
        Debug.Log("播放视频");
        Debug.Log("切换背景");
        _creepyMall = true;
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

}
