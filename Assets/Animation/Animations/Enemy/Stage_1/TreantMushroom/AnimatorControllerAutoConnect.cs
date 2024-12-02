using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

public class AnimatorControllerAutoConnect : MonoBehaviour
{
    [MenuItem("Tools/Generate Animator Controller with Transitions")]
    public static void GenerateAnimatorController()
    {
        // 1. 애니메이터 컨트롤러 생성
        var controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/CustomAnimatorController.controller");

        // 2. 파라미터 추가
        controller.AddParameter("isMoving", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isAttacking", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isTread", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isSpawn", AnimatorControllerParameterType.Bool);
        controller.AddParameter("direction", AnimatorControllerParameterType.Int);

        // 3. 상태 설정
        var rootStateMachine = controller.layers[0].stateMachine;

        // 이동 상태
        var moveDown = rootStateMachine.AddState("Treant_Mushroom_Move_Down");
        var moveTop = rootStateMachine.AddState("Treant_Mushroom_Move_Top");
        var moveLeft = rootStateMachine.AddState("Treant_Mushroom_Move_Left");
        var moveRight = rootStateMachine.AddState("Treant_Mushroom_Move_Right");

        // 공격 상태
        var attackDown = rootStateMachine.AddState("Treant_Mushroom_Attack_Down");
        var attackTop = rootStateMachine.AddState("Treant_Mushroom_Attack_Top");
        var attackLeft = rootStateMachine.AddState("Treant_Mushroom_Attack_Left");
        var attackRight = rootStateMachine.AddState("Treant_Mushroom_Attack_Right");

        // 밟기 상태
        var treadDown = rootStateMachine.AddState("Treant_Mushroom_Tread_Down");
        var treadTop = rootStateMachine.AddState("Treant_Mushroom_Tread_Top");
        var treadLeft = rootStateMachine.AddState("Treant_Mushroom_Tread_Left");
        var treadRight = rootStateMachine.AddState("Treant_Mushroom_Tread_Right");

        // 스폰 상태
        var spawnDown = rootStateMachine.AddState("Treant_Mushroom_Spawn_Down");
        var spawnTop = rootStateMachine.AddState("Treant_Mushroom_Spawn_Top");
        var spawnLeft = rootStateMachine.AddState("Treant_Mushroom_Spawn_Left");
        var spawnRight = rootStateMachine.AddState("Treant_Mushroom_Spawn_Right");

        // 상태 매핑
        var moveStates = new Dictionary<int, AnimatorState>
        {
            { 0, moveDown },
            { 1, moveTop },
            { 2, moveLeft },
            { 3, moveRight }
        };

        var attackStates = new Dictionary<int, AnimatorState>
        {
            { 0, attackDown },
            { 1, attackTop },
            { 2, attackLeft },
            { 3, attackRight }
        };

        var treadStates = new Dictionary<int, AnimatorState>
        {
            { 0, treadDown },
            { 1, treadTop },
            { 2, treadLeft },
            { 3, treadRight }
        };

        var spawnStates = new Dictionary<int, AnimatorState>
        {
            { 0, spawnDown },
            { 1, spawnTop },
            { 2, spawnLeft },
            { 3, spawnRight }
        };

        // 방향 리스트
        var directions = new int[] { 0, 1, 2, 3 };

        // 4. 이동 상태 간 전이 설정
        foreach (var fromDir in moveStates.Keys)
        {
            foreach (var toDir in moveStates.Keys)
            {
                if (fromDir != toDir)
                {
                    SetupTransitions(moveStates[fromDir], moveStates[toDir], toDir, isMoving: true);
                }
            }
        }

        // 5. 이동 -> 공격 전이 설정 (모든 방향)
        foreach (var fromDir in moveStates.Keys)
        {
            foreach (var toDir in attackStates.Keys)
            {
                SetupTransitions(moveStates[fromDir], attackStates[toDir], toDir, isAttacking: true);
            }
        }

        // 6. 공격 -> 이동 전이 설정 (모든 방향)
        foreach (var fromDir in attackStates.Keys)
        {
            foreach (var toDir in moveStates.Keys)
            {
                SetupTransitions(attackStates[fromDir], moveStates[toDir], toDir, isAttacking: false);
            }
        }

        // 7. 이동 -> 밟기 전이 설정 (모든 방향)
        foreach (var fromDir in moveStates.Keys)
        {
            foreach (var toDir in treadStates.Keys)
            {
                SetupTransitions(moveStates[fromDir], treadStates[toDir], toDir, isTread: true);
            }
        }

        // 8. 밟기 -> 이동 전이 설정 (모든 방향)
        foreach (var fromDir in treadStates.Keys)
        {
            foreach (var toDir in moveStates.Keys)
            {
                SetupTransitions(treadStates[fromDir], moveStates[toDir], toDir, isTread: false);
            }
        }

        // 9. 이동 -> 스폰 전이 설정 (모든 방향)
        foreach (var fromDir in moveStates.Keys)
        {
            foreach (var toDir in spawnStates.Keys)
            {
                SetupTransitions(moveStates[fromDir], spawnStates[toDir], toDir, isSpawn: true);
            }
        }

        // 10. 스폰 -> 이동 전이 설정 (모든 방향)
        foreach (var fromDir in spawnStates.Keys)
        {
            foreach (var toDir in moveStates.Keys)
            {
                SetupTransitions(spawnStates[fromDir], moveStates[toDir], toDir, isSpawn: false);
            }
        }

        Debug.Log("Animator Controller 생성 및 전이 설정 완료!");
    }

    private static void SetupTransitions(
        AnimatorState fromState,
        AnimatorState toState,
        int direction,
        bool isMoving = false,
        bool isAttacking = false,
        bool isTread = false,
        bool isSpawn = false)
    {
        var transition = fromState.AddTransition(toState);
        transition.hasExitTime = false;

        // 필요한 조건 추가
        if (isMoving)
            transition.AddCondition(AnimatorConditionMode.If, 0, "isMoving");
        if (isAttacking)
            transition.AddCondition(AnimatorConditionMode.If, 0, "isAttacking");
        if (isTread)
            transition.AddCondition(AnimatorConditionMode.If, 0, "isTread");
        if (isSpawn)
            transition.AddCondition(AnimatorConditionMode.If, 0, "isSpawn");

        if (!isMoving && !isAttacking && !isTread && !isSpawn)
        {
            // 이동으로 돌아가는 경우 (예: isAttacking false)
            if (fromState.name.Contains("Attack"))
                transition.AddCondition(AnimatorConditionMode.IfNot, 0, "isAttacking");
            else if (fromState.name.Contains("Tread"))
                transition.AddCondition(AnimatorConditionMode.IfNot, 0, "isTread");
            else if (fromState.name.Contains("Spawn"))
                transition.AddCondition(AnimatorConditionMode.IfNot, 0, "isSpawn");
        }

        // 항상 방향 조건 추가
        transition.AddCondition(AnimatorConditionMode.Equals, direction, "direction");
    }
}
