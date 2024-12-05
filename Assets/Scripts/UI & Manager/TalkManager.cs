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
    public static Dictionary<int, string[]> TalkData_ENG;
    public static Dictionary<int, string[]> TalkData_KOR;

    public static int ForOpeningIdx;

    /*
     * Stage 대화 추가하는 방법
     * Ex) Stage 1이면 1xx으로 인덱싱
     * 뒤에 붙는 2자리 수는 순서에 맞추어 아이디로 기입한다.
     */
    public static void SetTalkData()
    {
        TalkData_ENG = new Dictionary<int, string[]>();
        TalkData_KOR = new Dictionary<int, string[]>();

        //  ENGLISH
        TalkData_ENG.Add(001, new string[]
        {
            "Holy Shit Mother Fxxk?",
            "갑자기 나보고 전쟁을 나가라고?",
            "이게 뭔 개소리야"
        });
        TalkData_ENG.Add(002, new string[]
        {
            "응 안해",
            "응 못해",
            "응 ㄲㅈ 안해"

        });
        TalkData_ENG.Add(003, new string[]
        {
            "Third Test",
            "오케이",
            "Shit"

        });
        
        //  KOREAN
        
        TalkData_KOR.Add(001, new string[]
        {
            "뭐가 어째고 저째?",
            "갑자기 나보고 전쟁을 나가라고?",
            "이게 뭔 개소리야"
        });
        TalkData_KOR.Add(002, new string[]
        {
            "응 안해",
            "응 못해",
            "응 ㄲㅈ 안해"
        });
        TalkData_KOR.Add(003, new string[]
        {
            "세 번째 테스트",
            "오케이",
            "응 ㄲㅈ 안해"
        });

        //  수동으로 설정해야 함 (한글 영어 동일하게)
        ForOpeningIdx = 3;

        TalkData_ENG.Add(101, new string[]
        {
            "Connecting to Headquarters...",
            $"Developer, You arrived at {StageInfoManager.GetCurStageName()}.",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Analyzing...",
            "Briefing Scanning Results...",
            //  터렛 숫자는 지금 넣으려면 조금 빡세서 나중으로 미룸
            "Found some Turrets and One Control Unit",
            "[Manufacturer : Von Neumann Industry]\nAssembled by Vanguard from Earth.",
            "Finished Briefing Scanning Results...",
            "Once the Control Unit activates, You can conquer the planet.",
            "You MUST protect it from the enemies until it activates.",
            "You can use the emergency power system of the Control Unit to activate turrets.",
            "Turrets will support You in eliminating enemies.",
            "You can face a lack of power. Use it wisely.",
            "Connection Lost..."
        });
        
        TalkData_ENG.Add(201, new string[]
        {
            "음 어서와요 뚜뚜 스테이지 2입니다!",
            $"Developer, You arrived at {StageInfoManager.GetCurStageName()}.",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Analyzing...",
            "Briefing Scanning Results...",
            //  터렛 숫자는 지금 넣으려면 조금 빡세서 나중으로 미룸
            "Found some Turrets and One Control Unit",
            "[Manufacturer : Von Neumann Industry]\nAssembled by Vanguard from Earth.",
            "Finished Briefing Scanning Results...",
            "Once the Control Unit activates, You can conquer the planet.",
            "You MUST protect it from the enemies until it activates.",
            "You can use the emergency power system of the Control Unit to activate turrets.",
            "Turrets will support You in eliminating enemies.",
            "You can face a lack of power. Use it wisely.",
            "Connection Lost..."
        });
        
        TalkData_KOR.Add(101, new string[]
        {
            "본부와 연결 중...",
            $"개발자님, {StageInfoManager.GetCurStageName()}에 도착했습니다.",
            "주변의 위험 물체를 스캔 중입니다.",
            "스캔 중...",
            "분석 중...",
            "스캔 결과를 브리핑합니다...",
            // 터렛 숫자는 지금 넣으려면 조금 빡세서 나중으로 미룸
            "몇 개의 터렛과 하나의 컨트롤 유닛을 발견했습니다.",
            "[제조사 : 폰 노이만 산업]\n지구에서 온 선발대에 의해 조립되었습니다.",
            "스캔 결과 브리핑을 완료했습니다...",
            "컨트롤 유닛을 활성화하면 행성을 정복할 수 있습니다.",
            "활성화될 때까지 반드시 적들로부터 보호해야 합니다.",
            "컨트롤 유닛의 비상 전력 시스템을 사용하여 터렛을 활성화할 수 있습니다.",
            "터렛은 적을 제거하는 데 도움을 줄 것입니다.",
            "전력이 부족해질 수 있습니다. 신중히 사용하세요.",
            "연결이 끊어졌습니다..."
        });
        
        TalkData_KOR.Add(201, new string[]
        {
            "한국어 버젼 스테이지 2입니다!",
            $"개발자님, {StageInfoManager.GetCurStageName()}에 도착했습니다.",
            "주변의 위험 물체를 스캔 중입니다.",
            "스캔 중...",
            "분석 중...",
            "스캔 결과를 브리핑합니다...",
            // 터렛 숫자는 지금 넣으려면 조금 빡세서 나중으로 미룸
            "몇 개의 터렛과 하나의 컨트롤 유닛을 발견했습니다.",
            "[제조사 : 폰 노이만 산업]\n지구에서 온 선발대에 의해 조립되었습니다.",
            "스캔 결과 브리핑을 완료했습니다...",
            "컨트롤 유닛을 활성화하면 행성을 정복할 수 있습니다.",
            "활성화될 때까지 반드시 적들로부터 보호해야 합니다.",
            "컨트롤 유닛의 비상 전력 시스템을 사용하여 터렛을 활성화할 수 있습니다.",
            "터렛은 적을 제거하는 데 도움을 줄 것입니다.",
            "전력이 부족해질 수 있습니다. 신중히 사용하세요.",
            "연결이 끊어졌습니다..."
        });
        
    }

    public static string GetTalk(int id, int talkIndex)
    {
        if (GameManager.Language == 0)
        {
            if (talkIndex == TalkData_ENG[id].Length)
            {
                return null;
            }
            
            return TalkData_ENG[id][talkIndex];
        }
        //  KOREAN
        else
        {
            if (talkIndex == TalkData_KOR[id].Length)
            {
                return null;
            }
            
            return TalkData_KOR[id][talkIndex];
        }
    }
}