using UnityEngine;

public class LevelOfDifficulty : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab; // 타워 프리팹
    
    //타워가 랜덤으로 생성될 위치
    [SerializeField] float xRange = 7f;
    [SerializeField] float zRange = 7f;

    //타워의 y포지션 고정해두기 (땅에 박히는 거 방지)
    [SerializeField] float yPosition = 1f;


    public void LevelHigh()
    {

        //타워의 랜덤 위치 생성
        Vector3 randomPosition = new Vector3(Random.Range(-xRange, xRange), yPosition,Random.Range(-zRange, zRange));

        //타워 생성
        GameObject newTower = Instantiate(towerPrefab, randomPosition, Quaternion.identity);

        //타워 공격 시작
        TowerController towerController = newTower.GetComponent<TowerController>();
        if (towerController != null)
        {
            towerController.StartAttack();
        }
    }
}
