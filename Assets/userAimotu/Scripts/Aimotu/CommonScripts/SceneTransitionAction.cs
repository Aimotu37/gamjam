using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneTrans", menuName = "Actions/Scene Transition")]
public class SceneTransitionAction : StateAction
{
    public string targetSceneName;
    public float fadeDuration = 1.0f;
    public Vector3 playerSpawnPos; // 比如设置出现在最右侧
    public bool skipFadeOut = false;

    public override IEnumerator Execute()
    {
        if (!skipFadeOut)
            yield return FadeManager.Instance.FadeOut(fadeDuration);

        // skipFadeOut=true 时直接加载，不走 FadeOutAndLoad
        if (skipFadeOut)
            FadeManager.Instance.LoadSceneImmediate(targetSceneName);
        else
            FadeManager.Instance.TransitionToScene(targetSceneName);

        yield return null;
    }
}
