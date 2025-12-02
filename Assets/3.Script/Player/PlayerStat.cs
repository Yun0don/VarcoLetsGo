using UnityEngine;
using UnityEngine.Events;

public class PlayerStat : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    public float CurrentHp => currentHp;
    public float MaxHp => maxHp;

    [Header("Experience")]
    [SerializeField] private float maxExp = 100f;
    private float currentExp;
    [SerializeField] private int level = 1;
    public float CurrentExp => currentExp;
    public float MaxExp => maxExp;
    public int Level => level;

    [Header("Attack Stat")]
    [SerializeField] private float attackInterval = 1.0f; // 공격 속도
    [SerializeField] private float attackRange = 3.0f;    // 공격 범위
    [SerializeField] private float attackDamage = 10f;    // 공격력

    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;
    public float AttackDamage => attackDamage;

    // UI 업데이트를 위한 이벤트 (값이 변할 때 알림을 보냄)
    public UnityAction<float, float> OnHpChanged;   // (current, max)
    public UnityAction<float, float> OnExpChanged;  // (current, max)
    public UnityAction<int> OnLevelUp;              // (newLevel)

    private void Awake()
    {
        currentHp = maxHp;
        currentExp = 0;
    }

    private void Start()
    {
        // 시작 시 초기 상태 UI 갱신
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnExpChanged?.Invoke(currentExp, maxExp);
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp < 0) currentHp = 0;
        
        OnHpChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void GainExp(float amount)
    {
        currentExp += amount;
        
        // 경험치가 꽉 차면 레벨업 (반복문으로 한 번에 여러 레벨업 가능하게 처리)
        while (currentExp >= maxExp)
        {
            LevelUp();
        }
        
        OnExpChanged?.Invoke(currentExp, maxExp);
    }

    private void LevelUp()
    {
        currentExp -= maxExp;
        level++;
        maxExp *= 1.2f; // 레벨업 할 때마다 필요 경험치 20% 증가 (예시)
        
        // 레벨업 시 체력 회복 보너스?
        currentHp = maxHp;
        OnHpChanged?.Invoke(currentHp, maxHp);

        OnLevelUp?.Invoke(level);
        Debug.Log($"Level Up! Current Level: {level}");
    }

    private void Die()
    {
        Debug.Log("Player Died");
        GameManager.Instance.GameOver();
    }
}
