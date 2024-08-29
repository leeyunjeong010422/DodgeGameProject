using UnityEngine;

public class LevelOfDifficulty : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab; // Ÿ�� ������
    
    //Ÿ���� �������� ������ ��ġ
    [SerializeField] float xRange = 7f;
    [SerializeField] float zRange = 7f;

    //Ÿ���� y������ �����صα� (���� ������ �� ����)
    [SerializeField] float yPosition = 1f;


    public void LevelHigh()
    {

        //Ÿ���� ���� ��ġ ����
        Vector3 randomPosition = new Vector3(Random.Range(-xRange, xRange), yPosition,Random.Range(-zRange, zRange));

        //Ÿ�� ����
        GameObject newTower = Instantiate(towerPrefab, randomPosition, Quaternion.identity);

        //Ÿ�� ���� ����
        TowerController towerController = newTower.GetComponent<TowerController>();
        if (towerController != null)
        {
            towerController.StartAttack();
        }
    }
}
