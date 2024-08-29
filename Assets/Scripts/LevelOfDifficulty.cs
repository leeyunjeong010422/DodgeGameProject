using UnityEngine;

public class LevelOfDifficulty : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab; // 타워 프리팹
    [SerializeField] float xRange = 7f; // 타워가 생성될 수 있는 x축 범위
    [SerializeField] float zRange = 7f; // 타워가 생성될 수 있는 z축 범위
    [SerializeField] float yPosition = 1f; // 타워의 y축 위치 (고정)

    public void LevelHigh()
    {
        // 랜덤 위치 생성
        Vector3 randomPosition = new Vector3(Random.Range(-xRange, xRange),yPosition,Random.Range(-zRange, zRange));

        // 타워 생성
        GameObject newTower = Instantiate(towerPrefab, randomPosition, Quaternion.identity);

        //타워 공격
        TowerController towerController = newTower.GetComponent<TowerController>();
        if (towerController != null)
        {
            towerController.StartAttack();
        }
    }
}
