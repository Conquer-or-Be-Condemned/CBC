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
            "Connecting to Headquarters...",
            "Developer, You Arrived at Planet [행성이름 자리].",
            "Scanning Hazard Objects Nearby.",
            "Scanning...",
            "Finished Scanning...",
            "Analyzing...",
            "Finished Analyzing...",
            "Briefing Scanning Results....",
            "Found [num of turrets] Turrets and One Control Unit",
            "Manufacturer: [제조사 이름? 넣어도 ㄱㅊ을듯]\nAssembled by Vanguard from Earth.",//제조사 이름 빼도 무관
            "Finished Briefing Scanning Results...",
            "Once the Control Unit Activates, You can Conquer the Planet.",
            "You MUST Protect it from the Enemies until it Activates",
            "You Can Use the Emergency Power System of the Control Unit\n to Activate Turrets",
            "Turrets will Support You in Eliminating Enemies",
            "You Might Face a Lack of Power. Use it Wisely",
            "Connection Lost..."
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
