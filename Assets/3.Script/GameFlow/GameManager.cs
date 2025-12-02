using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Score { get; private set; } // 점수 변수
    
    protected override void Awake()
    {
        base.Awake(); // ★ 중요: 부모의 Awake를 먼저 실행!
        
        // 내 초기화 코드 작성
        Debug.Log("게임 매니저 초기화 완료");
    }
    // 부모 초기화 먼저해야 안깨짐
   
    public void GameOver()
    {
        Debug.Log("게임 오버!");
        // 게임 오버 UI 띄우기 등 로직
    }
    
     public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("현재 점수: " + Score);
    }

}
