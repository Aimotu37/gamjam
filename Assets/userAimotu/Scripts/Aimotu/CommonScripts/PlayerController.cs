using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("绉诲姩璁剧疆")]
    public float speed = 5f;
    [Header("闊虫晥璁剧疆")]
    public AudioSource footstepSource;

    [Header("鍦板浘杈圭晫闄愬埗")]
    public GameObject background;
    private float minX; // 宸﹁竟s
    private float maxX;  // 鍙宠竟

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;
    private Vector2 movement;
    private Animator animator;
    void Start()
    {
        //璇诲彇鍑虹敓鐐�
        if (GlobalData.HasSpawnOverride)
        {
            transform.position = GlobalData.NextSpawnPosition;
            GlobalData.HasSpawnOverride = false; // 鐢ㄥ畬娓呮帀锛岄槻姝笅娆¤鐢�
        }

        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // --- 鑷姩璁＄畻杈圭晫鐨勬牳蹇冮€昏緫 ---
        if (background != null)
        {
            // 1. 鑾峰彇鑳屾櫙鍜屼富瑙掔殑娓叉煋鍣� (Renderer)

            Renderer bgRenderer = background.GetComponent<Renderer>();
            Renderer playerRenderer = GetComponent<Renderer>();

            if (bgRenderer != null && playerRenderer != null)
            {
                // 2. 鑾峰彇涓昏鐨勪竴鍗婂搴� (extents.x 灏辨槸鐗╀綋瀹藉害鐨勪竴鍗�)
                float playerHalfWidth = playerRenderer.bounds.extents.x;

                // 3. 璁＄畻宸﹁竟鐣岋細鑳屾櫙鐨勬渶宸﹁竟 + 涓昏鍗婂
                // bounds.min.x 鏄墿浣撳湪涓栫晫鍧愭爣涓渶宸﹁竟鐨勭偣
                minX = bgRenderer.bounds.min.x + playerHalfWidth;

                // 4. 璁＄畻鍙宠竟鐣岋細鑳屾櫙鐨勬渶鍙宠竟 - 涓昏鍗婂
                // bounds.max.x 鏄墿浣撳湪涓栫晫鍧愭爣涓渶鍙宠竟鐨勭偣
                maxX = bgRenderer.bounds.max.x - playerHalfWidth;

                Debug.Log($"杈圭晫宸茶嚜鍔ㄨ绠�: 宸� {minX} / 鍙� {maxX}");
            }
            else
            {
                Debug.LogError("閿欒锛氳儗鏅垨涓昏缂哄皯 Renderer 缁勪欢锛�");
            }
        }
        else
        {
            Debug.LogError("璇峰湪 Inspector 闈㈡澘涓妸銆愯儗鏅墿浣撱€戞嫋杩涘幓锛�");
        }
    }

    void Update()
    {
        // 1. 鎺ユ敹杈撳叆锛屼笉澶勭悊鐗╃悊绉诲姩

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // 搴旇鍦ㄨ繖閲屽姞鍒ゆ柇锛氬璇�/寮圭獥鏈熼棿涓嶈兘绉诲姩
        var manager = FindAnyObjectByType<SceneManagerBase>() as IGameManager;
        if (manager != null && manager.IsUIBlocking) horizontalInput = 0;
        movement.x = horizontalInput;
        movement.y = 0;

        // 2. 鍔ㄧ敾鎺у埗锛氱珛鍗冲搷搴旓紝鏃犲欢杩�
        bool isWalking = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool("IsWalking", isWalking);
        // 3. 澶勭悊浜虹墿缈昏浆 
        if (horizontalInput != 0)
        {
            mySpriteRenderer.flipX = horizontalInput < 0;
        }
        // --- 鑴氭澹伴€昏緫 ---
        if (isWalking)
        {
            if (!footstepSource.isPlaying) footstepSource.Play();
        }
        else
        {
            if (footstepSource.isPlaying) footstepSource.Stop();
        }
    }
    void FixedUpdate()
    {
        if (movement.x == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        }

        // 闄愬埗浣嶇疆鑼冨洿
        Vector2 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);

        // 濡傛灉瓒呭嚭杈圭晫锛屽己鍒朵慨姝ｄ綅缃�
        if (Mathf.Abs(clampedPos.x - rb.position.x) > 0.01f)
        {
            rb.position = clampedPos;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }




}
