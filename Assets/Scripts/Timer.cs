using UnityEngine;
using UnityEngine.UI;

//Timer 스크립트 인터넷 참고: https://gyong0.tistory.com/13
public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    float Sec;
    int Min;
    bool towersSpawned = false;

    [SerializeField] Text TimerText;
    [SerializeField] LevelOfDifficulty difficulty;

    public bool isRunning = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            _Timer();
        }
    }

    void _Timer()
    {
        Sec += Time.deltaTime;

        TimerText.text = string.Format("{0:D2}:{1:D2}", Min, (int)Sec);

        if ((int)Sec > 59)
        {
            Sec = 0;
            Min++;
            //towerspawmed = false ;
        }

        //1분마다 타워 생성
        //if (!towersSpawned && Min > 0)
        //{
        //    difficulty.LevelHigh();
        //    towersSpawned = true;
        //}


        //나중에 코루틴 사용해서 10초마다 1개씩 생성하게 수정
        //타이머가 10초가 되면 랜덤 위치에 타워 생성
        if ((int)Sec == 10 && !towersSpawned)
        {
            difficulty.LevelHigh();
            difficulty.LevelHigh();
            difficulty.LevelHigh();
            towersSpawned = true;
        }

        //10초가 되면 타워가 무한으로 생성되는 걸 막음
        //10초 후에 다시 타워를 생성할 수 있게 바꾸어 줌으로써 대기?하게 함
        if ((int)Sec > 10)
        {
            towersSpawned = false;
        }

    }

    public void StopTimer()
    {
        isRunning = false;
    }
}