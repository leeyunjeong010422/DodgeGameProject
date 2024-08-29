using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Running, GameOver }

    [SerializeField] GameState curState;
    [SerializeField] PlayerMover player;
    [SerializeField] TowerController[] towers;
    [SerializeField] BulletPool bulletPool;

    [Header("UI")]
    [SerializeField] Text readyText;
    [SerializeField] Text gameOverText;
    [SerializeField] Text timerText;
    [SerializeField] Timer timer;


    private void Start()
    {
        curState = GameState.Ready;

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

        foreach (var tower in towers)
        {
            tower.bulletPool = bulletPool;
        }

    }

    private void Update()
    {
        if(curState == GameState.Ready && Input.anyKeyDown)
        {
            GameStart();
        }

        //어느 키를 눌러야 재시작이 되는지
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
        timer.StopTimer();
    }
}
