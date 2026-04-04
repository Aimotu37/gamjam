using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    [Header("音效配置")]
    public AudioClip clickSound;
    [Range(0f, 1f)]
    public float volume = 1.0f;

    void Start()
    {
        // 自动为当前按钮绑定点击事件
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlayButtonSound);
        }
    }

    private void PlayButtonSound()
    {
        if (clickSound == null) return;

        // 1. 获取当前场景的管理器 (IGameManager)
        // 逻辑参考了你现有的 DialogueManager 获取方式
        IGameManager manager = FindActiveManager();

        if (manager != null)
        {
            // 使用管理器统一的播放接口
            manager.PlayGlobalSFX(clickSound, volume);
        }
        else
        {
            // 如果没找到管理器（比如在测试场景），则直接在相机位置播放
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
            Debug.LogWarning($"[ButtonSFX] 场景中未找到 IGameManager，已使用默认播放方式。");
        }
    }

    private IGameManager FindActiveManager()
    {
        // 按照你项目中出现的命名空间顺序检查单例
        if (S6.GameManager.Instance != null) return (IGameManager)S6.GameManager.Instance;
        // 针对日志中出现的 S61 路径进行兼容
        // if (S61.GameManager.Instance != null) return S61.GameManager.Instance; 
        if (S4.GameManager.Instance != null) return (IGameManager)S4.GameManager.Instance;

        return null;
    }
}