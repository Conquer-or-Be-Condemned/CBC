using System.Collections;
using UnityEngine;

public class Stage1Controller : MonoBehaviour
{
    public CanvasGroup monsterCanvasGroup; // Monster.png가 있는 UI
    public Transform monsterTransform; // Monster.png 오브젝트의 트랜스폼
    public float fadeDuration;
    public float scaleDuration;

    IEnumerator Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.OminousSound, true);
        yield return new WaitForSeconds(5f);
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.BossGrowl);
        
        // FadeIn과 ScaleUp을 하나의 코루틴에서 동시에 진행
        yield return StartCoroutine(FadeInAndScale(monsterCanvasGroup, monsterTransform, fadeDuration, scaleDuration));

        yield return StartCoroutine(FadeOut(monsterCanvasGroup, fadeDuration));

        yield return new WaitForSeconds(4f);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.BossGrowl);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.BossWalkingAppears);
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.OminousSound, false);
        
        yield return new WaitForSeconds(4f);
        AudioManager.Instance.PlayBGM(AudioManager.Bgm.Stage1, true);

    }

    IEnumerator FadeInAndScale(CanvasGroup group, Transform target, float fadeDuration, float scaleDuration)
    {
        float time = 0f;
        float fadeStart = 0f;
        float fadeEnd = 1f;

        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * 2f;

        // 두 연출 중 더 긴 시간을 기준으로 Loop
        float totalDuration = Mathf.Max(fadeDuration, scaleDuration);

        while (time < totalDuration)
        {
            time += Time.deltaTime;
            // 페이드 시간 비율
            float fadeT = Mathf.Clamp01(time / fadeDuration);
            // 스케일 시간 비율
            float scaleT = Mathf.Clamp01(time / scaleDuration);

            group.alpha = Mathf.Lerp(fadeStart, fadeEnd, fadeT);
            target.localScale = Vector3.Lerp(startScale, endScale, scaleT);

            yield return null;
        }

        // 종료 시점에 최종 값 보정
        group.alpha = fadeEnd;
        target.localScale = endScale;
    }

    IEnumerator FadeIn(CanvasGroup group, float duration)
    {
        float start = 0f;
        float end = 1f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        group.alpha = end;
    }

    IEnumerator FadeOut(CanvasGroup group, float duration)
    {
        float start = 1f;
        float end = 0f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        group.alpha = end;
    }

    IEnumerator ScaleUp(Transform target, Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            yield return null;
        }
        target.localScale = endScale;
    }
}
