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

    public GameObject textPrefab; // 텍스트 오브젝트

    private bool EndisSecondVideoPlaying = false; // 두 번째 동영상 재생 여부 확인
    private bool isBlinking = false; // 텍스트 깜빡임 상태 확인

    void Start()
    {
        // 첫 번째 동영상 설정 및 재생
        EndvideoPlayer.clip = EndVideo1;
        EndvideoPlayer.Play();

        // 동영상 끝날 때 이벤트 연결
        EndvideoPlayer.loopPointReached += OnVideoEnd;

        // 아웃트로 오브젝트 초기 비활성화
        textPrefab.SetActive(false);
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
            // 두 번째 동영상 끝난 후 오브젝트 활성화

            if (!isBlinking)
            {
                StartCoroutine(BlinkText()); // 텍스트 깜빡임 시작
            }
        }
    }

    private IEnumerator BlinkText()
    {
        isBlinking = true;

        while (true)
        {
            textPrefab.SetActive(true); // 텍스트 활성화
            yield return new WaitForSeconds(1f); // 0.5초 대기
            textPrefab.SetActive(false); // 텍스트 비활성화
            yield return new WaitForSeconds(1f); // 0.5초 대기
        }
    }
}
