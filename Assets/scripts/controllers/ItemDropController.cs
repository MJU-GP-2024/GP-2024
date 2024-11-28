using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // 드랍 가능한 아이템 리스트



    //******************************************************************************************
    [Range(0f, 1f)] public float itemDropChance = 1f; // 아이템 드랍 확률 1->100%  , 0.5 -> 50% 조정가능
    //******************************************************************************************

    public float itemChangeInterval = 1f; // 아이템 변경 주기(초)
    public GameObject[] allPossibleItems; // 모든 가능한 아이템 배열

    private void Start()
    {
    }

    public void RequestItemDrop(Vector3 dropPosition)
    {

        if (Random.value <= itemDropChance)
        {
            int randomIndex = Random.Range(0, itemPrefabs.Count); // 리스트에서 랜덤 선택
            GameObject droppedItem = Instantiate(itemPrefabs[randomIndex], dropPosition, Quaternion.identity);
            StartCoroutine(ChangeItemOverTime(droppedItem)); // 떨어지는 동안 아이템 변경
        }
        else
        {
            Debug.Log("[RequestItemDrop] 확률 꽝! 아이템 드랍 실패.");
        }
    }

    private IEnumerator ChangeItemOverTime(GameObject droppedItem)
{
    int index = 0; // 순차적으로 아이템을 변경하기 위한 인덱스 변수

    while (true)
    {
        if (droppedItem == null) // 아이템이 삭제되었는지 확인
        {
            Debug.Log("[ChangeItemOverTime] 아이템이 삭제되었습니다. 코루틴 종료.");
            yield break;
        }

        yield return new WaitForSeconds(itemChangeInterval);

        // 순차적으로 아이템 변경
        GameObject newItemPrefab = allPossibleItems[index];
        index = (index + 1) % allPossibleItems.Length; // 다음 인덱스로 이동, 배열 끝에서 다시 처음으로 돌아감

        SpriteRenderer spriteRenderer = droppedItem.GetComponent<SpriteRenderer>();
        SpriteRenderer newItemSpriteRenderer = newItemPrefab.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = newItemSpriteRenderer.sprite;
    }
}

    
}

// 아이템이 화면 밖으로 나가면 삭제
public class AutoDestroy : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        Debug.Log($"[AutoDestroy] {gameObject.name} 화면 밖으로 나가서 삭제됨.");
    }
}
