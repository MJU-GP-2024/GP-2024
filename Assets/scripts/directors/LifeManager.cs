using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public GameObject[] heartImages;

    private int currentLife;
    private GameObject player;

    public void DecreaseLife()
    {
        SetLife(currentLife - 1);
    }

    public void IncreaseLife()
    {
        SetLife(currentLife + 1);
    }

    private void Start()
    {
        this.player = GameObject.Find("Player");

        if (player != null)
        {
            this.currentLife = player.GetComponent<PlayerController>().getHp();
            SetLife(currentLife);
        }
        else
        {
            this.currentLife = 0;
            Debug.Log("Error; Missing Player Object(from LifeManager)");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            this.currentLife = player.GetComponent<PlayerController>().getHp();
            SetLife(currentLife);
        }
    }

    private void SetLife(int life)
    {
        currentLife = Mathf.Clamp(life, 0, heartImages.Length);

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].SetActive(i < currentLife); // 현재 생명에 맞게 하트를 활성화/비활성화
        }
    }
}
