using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace S6
{
    [CreateAssetMenu(fileName = "ChoiceAction", menuName = "Game/State Actions/Choice Action")]
    public class ChoiceAction : StateAction
    {
        public string question;
        public string[] options;
        public List<ActionListContainer> branchActions; // 每个选项对应一组 Action

        public override IEnumerator Execute()
        {
            bool isFinished = false;

            while (!isFinished)
            {
                int selectedIndex = -1;
                // 显示 UI 并注册回调
                ChoiceUISystem.Instance.Show(question, options, (index) => selectedIndex = index);

                // 等待玩家点击任何一个按钮
                while (selectedIndex == -1) yield return null;

                // 执行对应分支里的所有 Action（比如播对白）
                if (selectedIndex >= 0 && selectedIndex < branchActions.Count)
                {
                    foreach (var action in branchActions[selectedIndex].actions)
                    {
                        if (action != null) yield return action.Execute();
                    }
                }

                // --- 核心逻辑判断 ---
                // 如果点的是炸串 (Index 0)，标记为结束，跳出大循环
                if (selectedIndex == 0)
                {
                    isFinished = true;
                }
                else
                {
                    // 如果点的是奶茶或蛋糕，大循环不结束，yield return null 后会回到开头重新执行 Show
                    Debug.Log("非结束项执行完毕，准备回到选项界面...");
                    yield return new WaitForSeconds(0.1f);
                }
            }
           
        }
    }
}