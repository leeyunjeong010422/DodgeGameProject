using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //�̱������� ���
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

    //�̱��������� �����ϴ� �޼���
    private void Awake()
    {
        //�ν��Ͻ��� �������� ������ ���� �ν��Ͻ��� ����
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //�̹� �ν��Ͻ��� �����ϸ� ����
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        curState = GameState.Ready;
        bestScore = PlayerPrefs.GetFloat("BestScore", 0f); //�ְ��� ��������
        UpdateBestScoreUI(); //�ְ��� UI ������Ʈ

        //���Ӿ��� �ִ� ��� ������Ʈ ã��
        //��, �ð��� ���� �ɸ��� �Լ��̱� ������ �ε� �������� ��� ����
        towers = FindObjectsOfType<TowerController>();

        //===========================�Ʒ� �� �ڵ尡 ���� ������ �ڵ���=================================
        //GameObject playerGameObj = GameObject.FindGameObjectWithTag("Player");
        //PlayerMover playerMover = playerGameObj.GetComponent<PlayerMover>();

        //���Ӿ��� �ִ� Ư�� ������Ʈ ã��
        //��, �ð��� ���� �ɸ��� �Լ��̱� ������ �ε� �������� ��� ����
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
        //�ƹ�Ű�� ������ ���� ����
        if(curState == GameState.Ready && Input.anyKeyDown)
        {
            GameStart();
        }

        //��� Ű�� ������ ������� �Ǵ��� (���� DŰ�� ������!!)
        else if (curState == GameState.GameOver && Input.GetKeyDown(KeyCode.D))
        {
            //���� ��ε��ϸ� ��
            SceneManager.LoadScene("DodgeScene");
        }
    }

    public void GameStart()
    {
        curState = GameState.Running;

        //Ÿ���� ���ݰ���
        foreach (TowerController tower in towers)
        {
            tower.StartAttack();
        }

        readyText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        bestScoreText.gameObject.SetActive(false);

        Timer.Instance.StartTimer();

        //20�� �Ŀ� Ŭ���� �� Ȱ��ȭ
        //20�ʸ� �����߼� 5�ʷ� �׽�Ʈ�߱� ������ ���� ��ϵǾ� �ִ� �ְ����� 20�� �̸���...
        Invoke("ClearZoneMaked", 20f);
    }

    public void GameOver()
    {
        curState = GameState.GameOver;
        //Ÿ���� ��������
        foreach (TowerController tower in towers)
        {
            tower.StopAttack();
        }

        readyText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        bestScoreText.gameObject.SetActive(true);
        //timer.StopTimer();

        //Ÿ�̸� ���߱� �̱��� ���
        Timer.Instance.StopTimer(); 
    }

    //Ŭ���� �� Ȱ��ȭ (Invoke�� ������ ȣ��: Invoke("ClearZoneMaked", 20f);)
    private void ClearZoneMaked()
    {
        if (curState == GameState.Running) //���� ���°� Running�� ���� Ȱ��ȭ��
        {
            clearZone.SetActive(true); // ��Ȱ��ȭ �Ǿ� �ִ� Ŭ������ �ٽ� Ȱ��ȭ
        }
    }

    public void ClearZoneEntered()
    {
        if (curState == GameState.Running)
        {
            //Ÿ�̸ӿ��� ����ð� �������� (�̱������� Timer.Instance ���)
            float elapsedTime = Timer.Instance.GetElapsedTime();
            
            //���࿡ ����Ǿ� �ִ� �ְ��Ϻ��� ����� �ð��� ũ�� �ְ����� ������
            if (elapsedTime > bestScore)
            {
                bestScore = elapsedTime; //����
                PlayerPrefs.SetFloat("BestScore", bestScore);  //�ְ� ��� ����
                UpdateBestScoreUI();
                
                //���ο� �ְ� ������ ������ ����ϱ�
                Debug.Log("New! Best Score: " + bestScore);
            }
            curState = GameState.Clear;
        }
    }

    //�ְ� ����� UI�� ������Ʈ�� �� �ֵ�����
    private void UpdateBestScoreUI()
    {
        bestScoreText.text = "Best Score: " + string.Format("{0:D2}:{1:D2}", (int)bestScore / 60, (int)bestScore % 60);
    }
}
