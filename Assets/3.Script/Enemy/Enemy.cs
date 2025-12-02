using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHp = 10f;
    private float currentHp;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float expValue = 10f; // 죽으면 플레이어에게 줄 경험치

    private Transform playerTarget;
    private bool isDead = false;

    private void OnEnable()
    {
        // 스폰될 때마다 초기화
        currentHp = maxHp;
        isDead = false;
        
        // 플레이어를 찾습니다. (태그나 싱글톤 등을 이용할 수 있지만, 일단 Find로 찾습니다)
        // 최적화를 위해 나중에는 WaveManager가 플레이어 Transform을 넘겨주는 방식이 좋습니다.
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
            }
        }
    }

    private void Update()
    {
        if (isDead || playerTarget == null) return;

        // 플레이어 방향으로 이동
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        
        // y축 이동 방지 (땅바닥에 붙어다니게)
        direction.y = 0; 
        
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // 바라보는 방향 회전
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        Debug.Log($"{name} Hit! HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{name} Died!");

        // 플레이어에게 경험치 지급
        var playerStat = playerTarget.GetComponent<PlayerStat>();
        if (playerStat != null)
        {
            playerStat.GainExp(expValue);
        }

        // TODO: WaveManager에게 알림 (리스폰 처리를 위해)
        // 지금은 일단 파괴합니다. WaveManager가 생기면 오브젝트 풀링으로 바꿀 수 있습니다.
        Destroy(gameObject); 
    }
}
