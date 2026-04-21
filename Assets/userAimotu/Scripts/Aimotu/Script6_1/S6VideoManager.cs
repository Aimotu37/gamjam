using UnityEngine;
using UnityEngine.Video;

public class S6VideoManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject _backgroundScaryMall;

    void Awake()
    {

    }


    void Start()
    {
        //_backgroundScaryMall.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video has finished playing!");
        //_backgroundScaryMall.SetActive(true);
        this.gameObject.SetActive(false);

    }


}