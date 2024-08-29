using UnityEngine;

public class LevelOfDifficulty : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab; // Ÿ�� ������
    [SerializeField] float xRange = 7f; // Ÿ���� ������ �� �ִ� x�� ����
    [SerializeField] float zRange = 7f; // Ÿ���� ������ �� �ִ� z�� ����
    [SerializeField] float yPosition = 1f; // Ÿ���� y�� ��ġ (����)

    public void LevelHigh()
    {
        // ���� ��ġ ����
        Vector3 randomPosition = new Vector3(Random.Range(-xRange, xRange),yPosition,Random.Range(-zRange, zRange));

        // Ÿ�� ����
        GameObject newTower = Instantiate(towerPrefab, randomPosition, Quaternion.identity);

        //Ÿ�� ����
        TowerController towerController = newTower.GetComponent<TowerController>();
        if (towerController != null)
        {
            towerController.StartAttack();
        }
    }
}
