using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 모든 대화를 관리하는 스크립트입니다. 데이터 저장소의 역할을 하고 있습니다.
 * 이 스크립트는 어디에도 속하지 않습니다.
 * 대화의 Set은 id로 관리해주시길 바랍니다.
 * 완전 Static으로 구성됨.
 */
public class TalkManager : MonoBehaviour
{
    private static Dictionary<int, string[]> TalkData;

    /*
     * Stage 대화 추가하는 방법
     * Ex) Stage 1이면 1xx으로 인덱싱
     * 뒤에 붙는 2자리 수는 순서에 맞추어 아이디로 기입한다.
     */
    public static void SetTalkData()
    {
        TalkData = new Dictionary<int, string[]>();
        
        TalkData.Add(101, new string[]
        {
            "개발자님, 목적지 행성에 도착했습니다.",
            "주변 환경 분석 결과, 선봉대가 설치했던 터렛들을 발견했습니다.",
            "전력 수급에 한계가 있겠지만 우리의 제어 장치에 있는 전력을 사용한다면,\n터렛들을 활성화 시킬 수 있을 것입니다.",
            "제어 장치 활성화까지 적들을 부디 막아주세요.",
            "(통신 종료)"
        });
    }

    public static string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == TalkData[id].Length)
        {
            return null;
        }

        return TalkData[id][talkIndex];
    }
}
