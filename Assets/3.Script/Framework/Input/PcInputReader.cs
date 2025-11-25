using UnityEngine;
using UnityEngine.InputSystem;

public class PcInputReader : MonoBehaviour, IPlayerInput
{
    // ===== Properties exposed to PlayerController =====
    public Vector2 Move { get; private set; }

    public bool AttackDown { get; private set; }
    public bool AttackHeld { get; private set; }

    public bool JumpDown { get; private set; }
    public bool JumpHeld { get; private set; }

    public bool InteractDown { get; private set; }

    public bool Skill1Down { get; private set; }
    public bool Skill1Held { get; private set; }

    // --- Reset every frame ---
    void LateUpdate()
    {
        AttackDown = false;
        JumpDown = false;
        InteractDown = false;
        Skill1Down = false;
    }


    // ====== InputSystem Callbacks (from PlayerInput) ======
    
    public void OnMove(InputValue value)
    {
        Move = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        // "Down"은 처음 눌린 순간만 true
        if (value.isPressed && !AttackHeld)
            AttackDown = true;

        AttackHeld = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !JumpHeld)
            JumpDown = true;

        JumpHeld = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
            InteractDown = true;
    }

    public void OnSkill1(InputValue value)
    {
        if (value.isPressed && !Skill1Held)
            Skill1Down = true;

        Skill1Held = value.isPressed;
    }
}
