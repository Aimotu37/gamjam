using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BigSubtitle", menuName = "Actions/Big Subtitle")]
public class BigSubtitleAction : StateAction
{
    [TextArea(2, 6)]
    public string content;

    [Header("时长（0 用 Overlay 默认）")]
    public float holdSeconds = 0f;

    [Header("阻塞模式")]
    [Tooltip("勾选则等淡入淡出全部完成才进入下一条；不勾则只触发一下立刻继续")]
    public bool waitFinish = true;

    public override IEnumerator Execute()
    {
        var overlay = BigSubtitleOverlay.Instance;
        if (overlay == null)
        {
            Debug.LogWarning("[BigSubtitleAction] 场景里没有 BigSubtitleOverlay");
            yield break;
        }

        float hold = holdSeconds > 0f ? holdSeconds : overlay.hold;

        if (waitFinish)
        {
            yield return overlay.StartCoroutine(overlay.ShowRoutine(content, hold));
        }
        else
        {
            overlay.Show(content, hold);
            yield break;
        }
    }
}
