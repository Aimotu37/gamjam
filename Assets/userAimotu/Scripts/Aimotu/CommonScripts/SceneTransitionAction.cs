using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneTrans", menuName = "Actions/Scene Transition")]
public class SceneTransitionAction : StateAction
{
    public string targetSceneName;
    public float fadeDuration = 1.0f;
    public Vector3 playerSpawnPos; // 比如设置出现在最右侧
    public override IEnumerator Execute()
    {
        yield return FadeManager.Instance.FadeOut(fadeDuration);
        FadeManager.Instance.TransitionToScene(targetSceneName);
    }
}
