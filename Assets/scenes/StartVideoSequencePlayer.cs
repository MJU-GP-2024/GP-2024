using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
//s
public class StartVideoSequencer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트
    public VideoClip Video1; // 첫 번째 동영상
    public VideoClip Video2; // 두 번째 동영상

    public GameObject load1Image; // 6초 동안 표시할 이미지

    private bool isSecondVideoPlaying = false; // 두 번째 동영상 재생 여부 확인

    void Start()
    {
        // 코루틴 실행: 6초간 이미지 표시 후 첫 번째 동영상 재생
        StartCoroutine(ShowLoadImageAndPlayVideo());
    }

    IEnumerator ShowLoadImageAndPlayVideo()
    {
        // load1 이미지 활성화
        load1Image.gameObject.SetActive(true); // 이미지 보이기

        // 6초 대기
        yield return new WaitForSeconds(6f);

        // load1 이미지 비활성화
        if (load1Image != null)
        {
            load1Image.gameObject.SetActive(false); // 이미지 숨기기
        }

        // 첫 번째 동영상 설정 및 재생
        videoPlayer.clip = Video1;
        videoPlayer.Play();

        // 동영상 끝날 때 이벤트 연결
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!isSecondVideoPlaying)
        {
            // 두 번째 동영상 설정 및 재생
            videoPlayer.clip = Video2;
            videoPlayer.Play();
            isSecondVideoPlaying = true;
        }
        else
        {
            // 모든 동영상이 끝난 후 StartScene으로 전환
            SceneManager.LoadScene("StartScene");
        }
    }
}
