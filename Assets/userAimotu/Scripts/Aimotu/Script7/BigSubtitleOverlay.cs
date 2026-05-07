using System.Collections;
using TMPro;
using UnityEngine;

// 大字幕叠加显示。挂在场景里一个空 GameObject 上，
// 子节点包含 CanvasGroup + TextMeshProUGUI。
public class BigSubtitleOverlay : MonoBehaviour
{
    public static BigSubtitleOverlay Instance;

    [Header("UI 引用")]
    public CanvasGroup group;
    public TextMeshProUGUI text;

    [Header("默认时长")]
    public float fadeIn = 0.6f;
    public float hold = 2.5f;
    public float fadeOut = 0.6f;

    private Coroutine _running;

    private void Awake()
    {
        Instance = this;
        if (group != null) group.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Show(string content, float? customHold = null)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(ShowRoutine(content, customHold ?? hold));
    }

    public IEnumerator ShowRoutine(string content, float holdSec)
    {
        gameObject.SetActive(true);
        text.text = content;
        yield return Fade(0f, 1f, fadeIn);
        yield return new WaitForSeconds(holdSec);
        yield return Fade(1f, 0f, fadeOut);
        gameObject.SetActive(false);
        _running = null;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (group == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        group.alpha = to;
    }
}
