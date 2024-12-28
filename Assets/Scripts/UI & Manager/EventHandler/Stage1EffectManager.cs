using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stage1EffectManager : MonoBehaviour
{
    [Header("Red Flicker Settings")]
    public Image redScreen; // 빨간색 이미지를 Stage_1 씬에서 할당
    public float flickerDuration = 5f; // 총 번쩍임 지속 시간
    public float interval = 0.5f; // 번쩍임 간격

    private void Start()
    {
        // 씬이 시작될 때 번쩍임 효과 실행
        StartCoroutine(FlickerEffect());
    }

    private IEnumerator FlickerEffect()
    {
        yield return new WaitForSeconds(0.1f);
        if (redScreen == null)
        {
            // Debug.LogError("Red Screen Image가 할당되지 않았습니다!");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < flickerDuration)
        {
            // 빨간 화면 활성화
            SetScreenColorAlpha(0.5f);
            yield return new WaitForSeconds(interval / 2);

            // 빨간 화면 비활성화
            SetScreenColorAlpha(0f);
            yield return new WaitForSeconds(interval / 2);

            elapsedTime += interval;
        }
    }

    private void SetScreenColorAlpha(float alpha)
    {
        if (redScreen != null)
        {
            Color color = redScreen.color;
            color.a = alpha; // Alpha 값 설정
            redScreen.color = color;
        }
    }
}