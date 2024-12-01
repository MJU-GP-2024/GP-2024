using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    void Start()
    {
        // Animator 컴포넌트를 가져옵니다.
        Animator animator = GetComponent<Animator>();

        // 현재 애니메이션의 길이를 가져옵니다.
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // 애니메이션이 끝난 뒤 GameObject 삭제
        Destroy(gameObject, animationLength);
    }
}
