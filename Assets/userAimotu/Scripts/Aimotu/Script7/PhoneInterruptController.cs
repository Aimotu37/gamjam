using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhoneInterruptStep
{
    [Tooltip("本条消息的刺耳提示音")]
    public AudioClip alertSFX;
    [Range(0f, 1f)] public float volume = 1f;

    [Tooltip("出现前等待秒数（让玩家看一段日记）")]
    public float delayBefore = 1.5f;
}

// 监听 RoomState 切换；进入 triggerState 后顺序触发手机消息中断。
// PhoneUI.Messages[] 数组里的消息按本组件 steps 顺序对应。
public class PhoneInterruptController : MonoBehaviour
{
    [Header("启动状态")]
    public RoomState triggerState;

    [Header("中断步骤")]
    public List<PhoneInterruptStep> steps;

    [Header("背景嘈杂音（每步叠加 volumeStep）")]
    public AudioSource ambientLayer;
    [Range(0f, 1f)] public float ambientVolumeStep = 0.15f;

    [Header("全部结束后切换到的状态")]
    public RoomState afterAllInterrupts;

    [Header("结束前等待秒数")]
    public float endDelay = 1f;

    private bool _running;

    private void Start()
    {
        SceneManagerBase.OnRoomStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        SceneManagerBase.OnRoomStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(RoomState state)
    {
        if (state == triggerState && !_running)
            StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        _running = true;
        var manager = FindAnyObjectByType<SceneManagerBase>() as IGameManager;
        float ambientVol = ambientLayer != null ? ambientLayer.volume : 0f;

        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            yield return new WaitForSeconds(step.delayBefore);

            if (step.alertSFX != null)
                manager?.PlayGlobalSFX(step.alertSFX, step.volume);

            if (ambientLayer != null)
            {
                ambientVol = Mathf.Min(1f, ambientVol + ambientVolumeStep);
                ambientLayer.volume = ambientVol;
                if (!ambientLayer.isPlaying) ambientLayer.Play();
            }

            PhoneUI.Instance.Open();
            PhoneUI.Instance.OpenMessageWindow();
            PhoneUI.Instance.GetMessageContent();

            yield return new WaitUntil(() => !PhoneUI.Instance.IsOpen);

            if (i < steps.Count - 1)
                PhoneUI.Instance.NextMessage();
        }

        yield return new WaitForSeconds(endDelay);
        manager?.EnterState(afterAllInterrupts);
    }
}
