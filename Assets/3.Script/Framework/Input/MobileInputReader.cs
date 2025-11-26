using UnityEngine;
using UnityEngine.InputSystem;

public class MobileInputReader : MonoBehaviour, IPlayerInput
{
    [SerializeField]  DynamicFloatingJoystick joystick;

    public Vector2 Move { get; private set; }

    public bool AttackDown { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool JumpDown { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool InteractDown { get; private set; }
    public bool Skill1Down { get; private set; }
    public bool Skill1Held { get; private set; }

    void Update()
    {
        Move = joystick ? joystick.Direction : Vector2.zero;
    }

    void LateUpdate()
    {
        AttackDown   = false;
        JumpDown     = false;
        InteractDown = false;
        Skill1Down   = false;
    }

    // UI 버튼에서 호출
    public void OnAttackButtonDown() { AttackDown = true; AttackHeld = true; }
    public void OnAttackButtonUp()   { AttackHeld = false; }
    public void OnSkill1ButtonDown() { Skill1Down = true; Skill1Held = true; }
    public void OnSkill1ButtonUp()   { Skill1Held = false; }
    public void OnJumpButtonDown()   { JumpDown = true; JumpHeld = true; }
    public void OnJumpButtonUp()     { JumpHeld = false; }
    public void OnInteractButton()   { InteractDown = true; }
}
