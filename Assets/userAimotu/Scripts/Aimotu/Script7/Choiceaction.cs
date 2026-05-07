using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "S7ChoiceAction", menuName = "Actions/S7Choice")]
public class ChoiceAction : StateAction
{
    [TextArea(2, 5)]
    public string prompt;

    [Header("选项 A")]
    public string labelA;
    public ActionListContainer onChooseA;

    [Header("选项 B（留空 = 单按钮）")]
    public string labelB;
    public ActionListContainer onChooseB;

    public override IEnumerator Execute()
    {
        var panel = ChoicePanel.Instance;
        if (panel == null) { Debug.LogWarning("[ChoiceAction] 场景内没有 ChoicePanel"); yield break; }

        int choice = -1;
        panel.Show(prompt, labelA, labelB, idx => choice = idx);

        while (choice < 0) yield return null;

        var actions = (choice == 0) ? onChooseA?.actions : onChooseB?.actions;
        if (actions == null) yield break;

        foreach (var a in actions)
            if (a != null) yield return a.Execute();
    }
}