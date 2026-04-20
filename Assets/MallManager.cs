using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallManager : MonoBehaviour
{

    //地图边界
    public GameObject background;
    private float _minX; // 左边s
    private float _maxX;  // 右边

    //玩家位置
    public Rigidbody2D _playerRb;
    public SpriteRenderer _playerSpriteRender;


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





}
