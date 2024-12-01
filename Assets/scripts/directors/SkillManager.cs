using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Image missileCoolDownBar; // 미사일 쿨다운 막대
    public Image skillChargeBar;    // 스킬 충전 막대

    private SkillGenerator skillGenerator;
    private float missileCharge = 0f; // 0 ~ 20
    private float timeCharge = 0f;    // 0 ~ 20

    private void Start()
    {
        skillGenerator = GameObject.Find("SkillGenerator").GetComponent<SkillGenerator>();
        if (skillGenerator == null)
        {
            Debug.Log("Error.GameUIProvider/SkillManager.cs. cannot find skillgenerator scripts");
        }
    }

    void Update()
    {
        // 막대 값 업데이트
        UpdateMissileCoolDownBar();
        UpdateSkillChargeBar();
    }

    private void UpdateMissileCoolDownBar()
    {
        if (skillGenerator != null)
        {
            missileCoolDownBar.fillAmount = skillGenerator.GetMissileSkillCharged() / 20f; // Slider는 0~1 값 사용
        }
    }

    private void UpdateSkillChargeBar()
    {
        if (skillGenerator != null)
        {
            skillChargeBar.fillAmount = skillGenerator.GetTimeSkillCharged() / 20f; // Slider는 0~1 값 사용
        }
    }
}
