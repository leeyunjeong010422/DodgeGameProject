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

        //��� Ű�� ������ ������� �Ǵ���
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
        timer.StopTimer();
    }
}
