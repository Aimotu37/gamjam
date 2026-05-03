// 添加到四件非床物品的 defaultActions 列表末尾
// 执行后通知 Task_S3 记录该物品已查看，Task_S3 自动检测是否全部完成
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/S3 Mark Item Viewed")]
public class MarkS3ItemViewedAction : StateAction
{
    [Header("物品类型（选 S3_ 开头的选项）")]
    public ItemType itemType;

    public override IEnumerator Execute()
    {
        var task = Object.FindAnyObjectByType<S3.Task_S3>();
        if (task != null)
            task.MarkViewed(itemType);
        else
            Debug.LogWarning("[MarkS3ItemViewedAction] 找不到 S3.Task_S3，请确认场景中有 Task_S3 组件");
        yield break;
    }
}
