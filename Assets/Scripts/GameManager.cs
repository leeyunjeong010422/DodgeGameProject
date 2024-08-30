using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //싱글톤패턴 사용
    public static GameManager Instance { get; private set; }
    public enum GameState { Ready, Running, GameOver, Clear }

    [SerializeField] GameState curState;
    [SerializeField] PlayerMover player;
    [SerializeField] TowerController[] towers;
    [SerializeField] BulletPool bulletPool;
    [SerializeField] GameObject clearZone;

    [Header("UI")]
    [SerializeField] Text readyText;
    [SerializeField] Text gameOverText;
    [SerializeField] Text timerText;
    [SerializeField] Text bestScoreText;

    private float bestScore = 0f;

    //싱글톤패턴을 구현하는 메서드
    private void Awake()
    {
        //인스턴스가 존재하지 않으면 현재 인스턴스를 설정
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //이미 인스턴스가 존재하면 삭제
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        curState = GameState.Ready;
        bestScore = PlayerPrefs.GetFloat("BestScore", 0f); //최고기록 가져오기
        UpdateBestScoreUI(); //최고기록 UI 업데이트

        //게임씬에 있는 모든 컴포넌트 찾기
        //단, 시간이 오래 걸리는 함수이기 때문에 로딩 과정에서 사용 권장
        towers = FindObjectsOfType<TowerController>();

        //===========================아래 두 코드가 같은 내용의 코드임=================================
        //GameObject playerGameObj = GameObject.FindGameObjectWithTag("Player");
        //PlayerMover playerMover = playerGameObj.GetComponent<PlayerMover>();

        //게임씬에 있는 특정 컴포넌트 찾기
        //단, 시간이 오래 걸리는 함수이기 때문에 로딩 과정에서 사용 권장
        player = FindObjectOfType<PlayerMover>();
        //===========================================================================================

        player.OnDied += GameOver;
        
        readyText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        bestScoreText.gameObject.SetActive(false);
        clearZone.SetActive(false);

        foreach (var tower in towers)
        {
            tower.bulletPool = bulletPool;
        }

    }

    private void Update()
    {
        //아무키나 누르면 게임 시작
        if(curState == GameState.Ready && Input.anyKeyDown)
        {
            GameStart();
        }

        //어느 키를 눌러야 재시작이 되는지 (현재 D키로 설정함!!)
        else if (curState == GameState.GameOver && Input.GetKeyDown(KeyCode.D))
        {
            //씬을 재로딩하면 됨
            SceneManager.LoadScene("DodgeScene");
        }
    }

    public void GameStart()
    {
        curState = GameState.Running;

        //타워들 공격개시
        foreach (TowerController tower in towers)
        {
            tower.StartAttack();
        }

        readyText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        bestScoreText.gameObject.SetActive(false);

        Timer.Instance.StartTimer();

        //20초 후에 클리어 존 활성화
        //20초를 못버텨서 5초로 테스트했기 때문에 현재 기록되어 있는 최고기록이 20초 미만임...
        Invoke("ClearZoneMaked", 20f);
    }

    public void GameOver()
    {
        curState = GameState.GameOver;
        //타워들 공격중지
        foreach (TowerController tower in towers)
        {
            tower.StopAttack();
        }

        readyText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        bestScoreText.gameObject.SetActive(true);
        //timer.StopTimer();

        //타이머 멈추기 싱글톤 사용
        Timer.Instance.StopTimer(); 
    }

    //클리어 존 활성화 (Invoke로 위에서 호출: Invoke("ClearZoneMaked", 20f);)
    private void ClearZoneMaked()
    {
        if (curState == GameState.Running) //현재 상태가 Running일 때만 활성화함
        {
            clearZone.SetActive(true); // 비활성화 되어 있던 클리어존 다시 활성화
        }
    }

    public void ClearZoneEntered()
    {
        if (curState == GameState.Running)
        {
            //타이머에서 경과시간 가져오기 (싱글톤으로 Timer.Instance 사용)
            float elapsedTime = Timer.Instance.GetElapsedTime();
            
            //만약에 저장되어 있는 최고기록보다 경과된 시간이 크면 최고기록을 갱신함
            if (elapsedTime > bestScore)
            {
                bestScore = elapsedTime; //갱신
                PlayerPrefs.SetFloat("BestScore", bestScore);  //최고 기록 저장
                UpdateBestScoreUI();
                
                //새로운 최고 점수가 나오면 출력하기
                Debug.Log("New! Best Score: " + bestScore);
            }
            curState = GameState.Clear;
        }
    }

    //최고 기록을 UI에 업데이트할 수 있도록함
    private void UpdateBestScoreUI()
    {
        bestScoreText.text = "Best Score: " + string.Format("{0:D2}:{1:D2}", (int)bestScore / 60, (int)bestScore % 60);
    }
}
