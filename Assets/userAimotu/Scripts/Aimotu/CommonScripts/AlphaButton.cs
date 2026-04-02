using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.5f; // 只有透明度高于 0.5 的地方才能被点到

    void Start()
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            // 这是核心代码
            img.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }
}