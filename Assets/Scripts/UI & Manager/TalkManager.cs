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
            "Due to Earth's overpopulation and resource depletion, I.S.C.A. was starting to colonize planets near Earth.",
            "To Colonize the planet they had to activate the Control Unit.",
            "Cause this process took a long time, I.S.C.A. needed armies to protect it.",
            "But strangely, there were few planets they failed every time.",
            "The chairman held \"The Developer\" of control unit to go and fix it immediately if he didn't want to get in trouble."
        });
        TalkData_ENG.Add(002, new string[]
        {
            "\"Hello guys, I'm \"The Developer\" of the control unit\"",
            "\"I'm heading to the first planet now\"",
            "\"How come they ask me to hold responsibility???\"",
            "\"It doesn't makes sense at all!!!\"",
            "\"I just dedicated myself to develop it!!!\"",
            "\"I'm soon arriving at the planet and have to prepare for laning\"",
            "\"Goodbye.... Hope to see you..... ALIVE....\""
        });
        // TalkData_ENG.Add(003, new string[]
        // {
        //     "Third Test",
        //     "오케이",
        //     "Shit"
        //
        // });
        
        //  KOREAN
        
        TalkData_KOR.Add(001, new string[]
        {
            "지구의 인구 과밀과 자원 고갈 때문에\nI.S.C.A.는 지구에서 가까운 행성부터\n정복하고 있었다.",
            "행성을 정복하기 위해서는 제어장치를\n활성화하는 절차를 진행해야 한다.",
            "이 절차를 시간이 꽤 오래 걸리기 때문에\nI.S.C.A.는 이 시간 동안 이를 지킬\n군인들을 필요로 했다.",
            "하지만 이상하게도, 항상 실패하는 행성들이\n몇 군데 있었다.",
            "I.S.C.A의 최고위원장은 \"The Developer\"에게\n험한 꼴을 당하고 싶지 않으면\n지금 당장 가서 제어장치를 고치라고 고함쳤다."
        });
        TalkData_KOR.Add(002, new string[]
        {
            "\"안녕? 난 제어장치의 개발자야 만나서 반가워\"",
            "\"지금 내가 정복해야 할 첫번째 행성으로\n이동하고 있어\"",
            "\"아니 어떻게 그 책임을 나한테 묻지???\"",
            "\"전혀 말이 안되잖아!!!\"",
            "\"난 그냥 제어장치를 개발하는데 전념했을 뿐인데!!!\"",
            "\"하..이제 곧 도착하니까 이만 착륙 준비하러 갈게\"",
            "\"잘가... 우리... 꼭.. 살아서.. 보자고...\""
        });
        // TalkData_KOR.Add(003, new string[]
        // {
        //     "세 번째 테스트",
        //     "오케이",
        //     "응 ㄲㅈ 안해"
        // });

        //  수동으로 설정해야 함 (한글 영어 동일하게)
        ForOpeningIdx = 2;

        TalkData_ENG.Add(101, new string[]
        {
            "Connecting to Headquarters...",
            $"Developer, You arrived at HJD-1029X2.",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Analyzing...",
            "Briefing Scanning Results...",
            "Found 12 Canon Turrets and 3 Missile Turrets.",
            "[Manufacturer : von Neumann Industries]\nAssembled by Vanguard from Earth.",
            "Current Temperature: 21.5\u00b0C",
            "Finished Briefing Scanning Results...",
            "Once the Control Unit activates, You can conquer the planet.",
            "You MUST protect it from the enemies until it activates.",
            "You can use the emergency power system of the Control Unit to activate turrets.",
            "Turrets will support You in eliminating enemies.",
            "You can face a lack of power. Use it wisely.",
            "Connection Lost..."
        });
        TalkData_ENG.Add(102, new string[]
        {
            "Developer, we are receiving a strong signal.",
            "All tower operations have been shut down. Please hurry to restart them.”"
        });
        TalkData_ENG.Add(201, new string[]
        {
            "Connecting to Headquarters...",
            $"Developer, You arrived at AJH-4001D2.",
            "You did a good job at the first planet! Well done!",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Analyzing...",
            "Briefing Scanning Results...",
            "Found 16 Canon Turrets and 4 Missile Turrets.",
            "Current Temperature: -21.7\u00b0C",
            "Finished Briefing Scanning Results...",
            "As I briefed you it's pretty cold outside take care.",
            "I believe you can success in the mission one more time.",
            "Connection Lost..."
        });
        TalkData_ENG.Add(202, new string[]
        {
            "Developer, we are receiving a strong signal.",
            "All tower operations have been shut down. Please hurry to restart them.”"
        });
        TalkData_ENG.Add(301, new string[]
        {
            "Connecting to Headquarters...",
            $"Developer, You arrived at JHS-8854xD.",
            "Finally it's our last planet!",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Analyzing...",
            "Briefing Scanning Results...",
            "Found 17 Canon Turrets and 4 Missile Turrets.",
            "Current Temperature: 89.3\u00b0C",
            "Finished Briefing Scanning Results...",
            "Unlike the planet before it's very hot outside",
            "Cause the control unit is placed at the right middle",
            "monsters will come out from everywhere. Watch Out!",
            "Connection Lost..."
        });
        TalkData_ENG.Add(302, new string[]
        {
            "Developer, we are receiving a strong signal.",
            "All tower operations have been shut down. Please hurry to restart them.”"
        });
        
        TalkData_KOR.Add(101, new string[]
        {
            "본부와 연결 중...",
            $"개발자님, HJD-1029X2에 도착했습니다.",
            "주변의 위험 물체를 스캔 중입니다.",
            "스캔 중...",
            "분석 중...",
            "스캔 결과를 브리핑합니다...",
            "12개의 캐논 터렛과 3개의 미사일 터렛을 발견했습니다.",
            "[제조사 : 폰 노이만 산업]\n지구에서 온 선발대에 의해 조립되었습니다.",
            "외부 기온: 21.5\u00b0C",
            "스캔 결과 브리핑을 완료했습니다...",
            "컨트롤 유닛을 활성화하면 행성을 정복할 수 있습니다.",
            "활성화될 때까지 반드시 적들로부터 보호해야 합니다.",
            "컨트롤 유닛의 비상 전력 시스템을 사용하여\n터렛을 활성화할 수 있습니다.",
            "터렛은 적을 제거하는 데 도움을 줄 것입니다.",
            "전력이 부족해질 수 있습니다.\n신중히 사용하세요.",
            "연결이 끊어졌습니다..."
        });
        TalkData_KOR.Add(102, new string[]
        {
            "개발자님, 강력한 전파의 수신입니다.",
            "모든 타워의 운영이 종료됩니다. 서둘러 재개시키십시오."
        });
        
        TalkData_KOR.Add(201, new string[]
        {
            "본부와 연결 중...",
            $"개발자님, AJH-4001D2에 도착했습니다.",
            "첫번째 행성을 성공적으로 정복하셨군요!\n축하드립니다!",
            "주변의 위험 물체를 스캔 중입니다.",
            "스캔 중...",
            "분석 중...",
            "스캔 결과를 브리핑합니다...",
            "16개의 캐논 터렛과 4개의 미사일 터렛을 발견했습니다.",
            "외부 기온: -21.7\u00b0C",
            "스캔 결과 브리핑을 완료했습니다...",
            "제가 말씀드렸듯이 밖에는 현재 기온이 매우 낮습니다.",
            "추위 잘 이기시고 건승을 빕니다.",
            "연결이 끊어졌습니다..."
        });
        TalkData_KOR.Add(202, new string[]
        {
            "개발자님, 강력한 전파의 수신입니다.",
            "모든 타워의 운영이 종료됩니다. 서둘러 재개시키십시오."
        });
        
        TalkData_KOR.Add(301, new string[]
        {
            "본부와 연결 중...",
            $"개발자님, JHS-8854xD에 도착했습니다.",
            "드디어 마지막 행성입니다!\n조금만 더 힘내세요!",
            "주변의 위험 물체를 스캔 중입니다.",
            "스캔 중...",
            "분석 중...",
            "스캔 결과를 브리핑합니다...",
            "17개의 캐논 터렛과 4개의 미사일 터렛을 발견했습니다.",
            "외부 기온: 89.3\u00b0C",
            "스캔 결과 브리핑을 완료했습니다...",
            "좀 전의 행성과는 다르게 여기는\n기온이 매우 높습니다.",
            "제어장치가 중앙에 있기 때문에 대응하는데\n어려움이 있을 것입니다.",
            "적들이 사방에서 나오니까 주의하세요!",
            "연결이 끊어졌습니다..."
        });
        TalkData_KOR.Add(302, new string[]
        {
            "개발자님, 강력한 전파의 수신입니다.",
            "모든 타워의 운영이 종료됩니다. 서둘러 재개시키십시오."
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