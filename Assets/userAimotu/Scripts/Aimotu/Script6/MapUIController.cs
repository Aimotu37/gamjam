using UnityEngine;

public class MapUIController : MonoBehaviour
{
    public GameObject[] landmarks;

    public void ShowLandmarks()
    {
        foreach (var lm in landmarks)
            if (lm != null) lm.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (var lm in landmarks)
            if (lm != null) lm.SetActive(false);
    }
}