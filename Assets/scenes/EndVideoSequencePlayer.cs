using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndVideoSequencePlayer : MonoBehaviour
{
    public VideoPlayer EndvideoPlayer; // VideoPlayer 컴포넌트
    public VideoClip EndVideo1; // 첫 번째 동영상
    public VideoClip EndVideo2; // 두 번째 동영상

    public GameObject OutroImagePrefab; // Outro 이미지 오브젝트

    private bool EndisSecondVideoPlaying = false; // 두 번째 동영상 재생 여부 확인
    private bool isBlinking = false; // 텍스트 깜빡임 상태 확인

    void Start()
    {
        // 첫 번째 동영상 설정 및 재생
        EndvideoPlayer.clip = EndVideo1;
        EndvideoPlayer.Play();

        // 동영상 끝날 때 이벤트 연결
        EndvideoPlayer.loopPointReached += OnVideoEnd;

        // 텍스트 및 Outro 이미지 초기 비활성화
        OutroImagePrefab.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!EndisSecondVideoPlaying)
        {
            // 두 번째 동영상 설정 및 재생
            EndvideoPlayer.clip = EndVideo2;
            EndvideoPlayer.Play();
            EndisSecondVideoPlaying = true;
        }
        else
        {
            // VideoPlayer 비활성화
            EndvideoPlayer.gameObject.SetActive(false);

            // Outro 이미지 활성화
            ShowEndTextAndOutro();
        }
    }

    private void ShowEndTextAndOutro()
    {
        if (!isBlinking)
        {
            isBlinking = true;

            // 텍스트와 Outro 이미지를 활성화
            OutroImagePrefab.SetActive(true);
        }
    }
}
