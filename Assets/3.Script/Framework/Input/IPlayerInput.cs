using UnityEngine;

public interface IPlayerInput
{
    // --- Move ---
    /// 이동 방향 (조이스틱 / WASD / 패드 스틱)
    Vector2 Move { get; }
    // --- Attack ---
    /// 공격 버튼이 "이번 프레임에 막 눌렸는가?"
    /// (단발 공격, 애니메이션 트리거 등에 사용)
    bool AttackDown { get; }
    /// 공격 버튼이 눌려있는 상태인가?
    /// (차지 공격, 자동사격 등)
    bool AttackHeld { get; }

    // --- Jump ---
    bool JumpDown { get; }
    bool JumpHeld { get; }

    // --- Interact (상호작용: 문 열기, 상자 열기 등) ---
    bool InteractDown { get; }

    // --- Skill 1 ---
    bool Skill1Down { get; }
    bool Skill1Held { get; }
}