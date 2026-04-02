using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//简介上的贴纸
public enum StickerType
{
    None,
    Fishtank,       //鱼缸
    Doll,           //玩具熊
    Award,          //奖状
    Beads,          //鱼形串珠
    Cake,           //蛋糕
    Bubbletea,      //奶茶
    Barbecue        //蛋糕
}
public class PopupSystem : MonoBehaviour
{
    public static PopupSystem Instance;

    [Header("UI")]
    public GameObject popupPanel;
    public TextMeshProUGUI contentText;
    public Image stickerImage;
    public Button closeButton;

    [Header("Sticker Config")]
    public Sprite defaultSticker;
    public Sprite fishtankSticker;      //鱼缸
    public Sprite dollSticker;          //玩具熊
    public Sprite awardSticker;         //奖状
    public Sprite beadsSticker;         //鱼形串珠
    public Sprite cakeSticker;          //蛋糕
    public Sprite bubbleteaSticker;     //奶茶
    public Sprite barbecueSticker;      //蛋糕

    private Action onClosedCallback;
    public bool IsOpen => isOpen;
    private bool isOpen = false;
    private IGameManager GetManager() =>
       FindAnyObjectByType<SceneManagerBase>() as IGameManager;

    private void Awake()
    {
        Debug.Log($"[PopupSystem] Awake 执行，GameObject={gameObject.name}", this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        Debug.Log($"[PopupSystem] closeButton={closeButton?.name ?? "NULL"}");
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);
            Debug.Log("[PopupSystem] 关闭按钮已连接");
        }
        else
        {
            Debug.LogWarning("[PopupSystem] closeButton 未赋值，请在 Inspector 里拖入");
        }

        popupPanel.SetActive(false);
    }
    private void SetSticker(StickerType type)
    {
        if (stickerImage == null) return;

        Sprite target = type switch
        {
            StickerType.Fishtank => fishtankSticker,
            StickerType.Doll => dollSticker,
            StickerType.Award => awardSticker,
            StickerType.Beads => beadsSticker,
            StickerType.Cake => cakeSticker,
            StickerType.Bubbletea => bubbleteaSticker,
            StickerType.Barbecue => barbecueSticker,

            _ => defaultSticker
        };

        stickerImage.sprite = target;
        stickerImage.gameObject.SetActive(target != null);
    }

    /// 打开 Popup

    public void Open(string text, StickerType sticker, Action onClosed = null)
    {
        if (isOpen)
        {
            Debug.LogWarning("[PopupSystem] 已经打开，忽略重复 Open");
            return;
        }
        Debug.Log($"[PopupSystem] Open by {gameObject.name}", this);
        isOpen = true; 
        popupPanel.SetActive(true);
        GetManager()?.PushUIBlock("Popup");

        if (contentText != null) contentText.text = text.Replace("\\n", "\n");
        
        SetSticker(sticker);
        onClosedCallback = onClosed;

    }

    /// 关闭 Popup

    public void Close()
    {
        Debug.Log($"[PopupSystem] Close 被调用，isOpen={isOpen}");
        Debug.Log($"[PopupSystem] Close by {gameObject.name}", this);
        if (!isOpen) return;
        isOpen = false; // 标记关闭完成
        popupPanel.SetActive(false);
        GetManager()?.PopUIBlock("Popup");

        onClosedCallback?.Invoke();
        onClosedCallback = null;
    }
}
