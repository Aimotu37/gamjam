// 放在 CommonScripts/ 下
// 使用已有的 PopupSystem 弹出物件简介

using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Actions/Show Item Info Popup")]
public class ShowItemInfoAction : StateAction
{
    [Header("弹窗内容")]
    public StickerType stickerType;

    [TextArea(3, 6)]
    public string content;

    public override IEnumerator Execute()
    {
        if (PopupSystem.Instance == null)
        {
            Debug.LogWarning("[ShowItemInfoAction] PopupSystem.Instance 为空");
            yield break;
        }

        PopupSystem.Instance.Open(content, stickerType);

        // 等弹窗关闭（UIBlock 释放）再继续后续 Action
        yield return new WaitUntil(() => !PopupSystem.Instance.IsOpen);
    }
}
