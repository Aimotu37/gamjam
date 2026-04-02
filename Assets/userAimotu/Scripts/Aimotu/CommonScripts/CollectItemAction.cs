// 在 Inspector 里配置好物件类型，拖进 InteractableItem 的 defaultActions
// 执行后自动写入 GlobalData，触发 HUD 更新和日记解锁

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Collect Item")]

public class CollectItemAction : StateAction
{
    public enum CollectType { PasswordClue, Diary }

    [Header("收集类型")]
    public CollectType collectType;

    [Header("收集密码线索时填（CollectType = PasswordClue）")]
    public ItemType itemType; // FishTank / Doll / Award

    [Header("收集日记时填（CollectType = Diary）")]
    public DiaryID diaryID;

    public override IEnumerator Execute()
    {
        var manager = GetManager();
        if (manager == null) yield break;

        // 找到当前场景的 Task
        var taskS4 = Object.FindAnyObjectByType<S4.Task_S4>();
        var taskS6 = Object.FindAnyObjectByType<S6.Task_S6>();
        var taskS61 = Object.FindAnyObjectByType<S61.Task_S61>();

        if (collectType == CollectType.PasswordClue)
        {
            taskS4?.CollectPassword(itemType);
            Debug.Log($"[CollectItemAction] 密码线索：{itemType}");
        }
        else if (collectType == CollectType.Diary)
        {
            // 根据 DiaryID 分发到对应 Task
            switch (diaryID)
            {
                case DiaryID.Diary1_FishAndBeads:
                    taskS4?.CollectDiary();
                    break;
                case DiaryID.Diary2_SnackCart:
                    taskS61?.CompleteSnackCartTask();
                    break;
                case DiaryID.Diary3_SummerTV:
                    taskS6?.CollectDiary(1);
                    break;
                case DiaryID.Diary4_SnackGachaToyPhone:
                    taskS6?.CollectDiary(2);
                    break;
                case DiaryID.Diary5_Stationery:
                    taskS6?.CollectDiary(3);
                    break;
                case DiaryID.Diary6_BooksAndMagazines:
                    taskS6?.CollectDiary(4);
                    break;
                case DiaryID.Diary7_Internet:
                    taskS6?.CollectDiary(5);
                    break;
            }
            Debug.Log($"[CollectItemAction] 日记：{diaryID}");
        }

        yield break;
    }
}