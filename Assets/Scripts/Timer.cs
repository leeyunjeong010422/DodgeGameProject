using UnityEngine;
using UnityEngine.UI;

//Timer ��ũ��Ʈ ���ͳ� ����: https://gyong0.tistory.com/13
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

        //1�и��� Ÿ�� ����
        //if (!towersSpawned && Min > 0)
        //{
        //    difficulty.LevelHigh();
        //    towersSpawned = true;
        //}


        //���߿� �ڷ�ƾ ����ؼ� 10�ʸ��� 1���� �����ϰ� ����
        //Ÿ�̸Ӱ� 10�ʰ� �Ǹ� ���� ��ġ�� Ÿ�� ����
        if ((int)Sec == 10 && !towersSpawned)
        {
            difficulty.LevelHigh();
            difficulty.LevelHigh();
            difficulty.LevelHigh();
            towersSpawned = true;
        }

        //10�ʰ� �Ǹ� Ÿ���� �������� �����Ǵ� �� ����
        //10�� �Ŀ� �ٽ� Ÿ���� ������ �� �ְ� �ٲپ� �����ν� ���?�ϰ� ��
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