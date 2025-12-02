using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStat stat;
    private float attackTimer;

    [Header("Settings")]
    [SerializeField] private LayerMask enemyLayer; // 공격할 적의 레이어 (Inspector에서 설정 필요)
    [SerializeField] private GameObject attackEffectPrefab; // 공격 이펙트 (선택 사항)

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();
        if (stat == null)
        {
            Debug.LogError("PlayerAttack: PlayerStat 컴포넌트를 찾을 수 없습니다!");
        }
    }

    private void Update()
    {
        if (stat == null) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= stat.AttackInterval)
        {
            attackTimer = 0f;
            PerformAreaAttack();
        }
    }

    private void PerformAreaAttack()
    {
        // 내 위치(transform.position)를 중심으로 반지름(AttackRange)만큼의 구체 안에 있는 적들을 찾음
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stat.AttackRange, enemyLayer);
        
        if (hitColliders.Length > 0)
        {
            // 공격 효과음이나 이펙트 재생 위치
            if (attackEffectPrefab != null)
            {
                Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            }

            Debug.Log($"[Attack] {hitColliders.Length}명의 적을 공격했습니다!");

            foreach (var hit in hitColliders)
            {
                // 적에게 데미지를 주는 로직
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(stat.AttackDamage);
                }
            }
        }
    }

    // 에디터에서 공격 범위를 눈으로 확인하기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        if (stat != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f); // 반투명 빨간색
            Gizmos.DrawSphere(transform.position, stat.AttackRange);
        }
        else
        {
            // stat이 없어도 대략적인 범위를 보기 위해 (기본값 3.0f 가정)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1.5f);
        }
    }
}
