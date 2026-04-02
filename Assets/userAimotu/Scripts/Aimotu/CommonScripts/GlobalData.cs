//日记改用 DiaryID 枚举，不用数字索引
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    // ──────────────────────────────────────────
    //  梦境1（Script4）密码线索
    // ──────────────────────────────────────────
    public static bool D1_Fish;    // 热带鱼 → 密码第1位 5
    public static bool D1_Doll;    // 玩偶   → 密码第2位 2
    public static bool D1_Award;   // 奖状   → 密码第3位 8
  
    public static bool D1_PasswordComplete => D1_Fish && D1_Doll && D1_Award;
 
    //  日记解锁（全场景共用）
    // ──────────────────────────────────────────
    private static readonly HashSet<DiaryID> _unlockedDiaries = new();

    public static void UnlockDiary(DiaryID id)
    {
        if (_unlockedDiaries.Add(id))
        {
            Debug.Log($"[GlobalData] 日记解锁：{id}");
            OnDiaryUnlocked?.Invoke(id);
        }
    }
    // DiaryUI 订阅这个事件，收到通知后刷新页面
    public static bool IsDiaryUnlocked(DiaryID id) => _unlockedDiaries.Contains(id);
    public static System.Action<DiaryID> OnDiaryUnlocked;
    // ──────────────────────────────────────────
    //  全局重置（新游戏时调用）
    // ──────────────────────────────────────────
    public static void ResetAll()
    {
        D1_Fish = D1_Doll = D1_Award = false;
        _unlockedDiaries.Clear();
        Debug.Log("[GlobalData] 所有进度已重置");
    }
}
