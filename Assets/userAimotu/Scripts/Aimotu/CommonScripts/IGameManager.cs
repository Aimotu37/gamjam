

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public interface IGameManager
{
    // 状态机
    RoomState CurrentState { get; }
    void EnterState(RoomState newState);

    // UI 阻断
    bool IsUIBlocking { get; }
    void PushUIBlock(string source);
    void PopUIBlock(string source);

    // 立绘
    Sprite GetCharacterPortrait(PortraitOption option);

    // 子系统引用
    DialogueManager Dialog { get; }
    TaskModule TaskModule { get; }

    // 视频播放（供 PlayClipAction 使用）
    RawImage uiRawImage { get; }
    VideoPlayer uiVideoPlayer { get; }
    RenderTexture uiRenderTexture { get; }
    CanvasGroup transitionMaskGroup { get; }

    // 音效
    void PlayGlobalSFX(AudioClip clip, float volume = 1.0f);
}
