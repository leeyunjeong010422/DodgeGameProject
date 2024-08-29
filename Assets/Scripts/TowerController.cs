using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] public BulletPool bulletPool;

    [SerializeField] GameObject[] bulletPrefabs; // �Ѿ� ������ �迭

    //������: ���� ������Ʈ ���赵 => ����Ƽ���� ���� ������Ʈ�� ������ �� ������ ����
    //[SerializeField] GameObject bulletPrefab; //������ �Ѿ� ������

    //[SerializeField] float bulletTime; //�Ѿ� ���� �ð�
    [SerializeField] float minBulletTime;
    [SerializeField] float maxBulletTime;
    [SerializeField] float remainTime; //���� �Ѿ� ������ ������ ��ٸ� �ð�
    [SerializeField] bool isAttacking; //���� ����

    private void Start()
    {
        //GameObject.FindGameObjectWithTag: ���ӿ� �ִ� �±׸� ���ؼ� Ư�� ���� ������Ʈ�� ã��
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        //===========================�Ʒ� �� �ڵ尡 ���� ������ �ڵ���=================================
        //�ϳ��� ����ص� ����
        //GetComponent: ���� ������Ʈ�� �ִ� ������Ʈ ��������
        target = playerObj.GetComponent<Transform>();

        //transform: ���� ������Ʈ�� ��ġ, ȸ��, ũ�⸦ �����ϴ� ��ɴ����
        //transform ������Ʈ�� ���� ���� ������Ʈ�� �ݵ�� ���� => transform ������Ʈ�� Ư���ϰ� ������Ƽ�� �ٷ� ��� ����
        target = playerObj.transform;
        //===========================================================================================

        RandomFireTime();
    }

    private void Update()
    {
        //���� ���� �ƴ� ���� �Ѿ��� �������� �ʵ��� ��
        if (isAttacking == false)
            return;

        //���� �Ѿ� �������� ���� �ð��� ��� ����
        remainTime -= Time.deltaTime;

        //���� �Ѿ��� ������ ������ ���� �ð��� ���� ��� == �Ѿ��� ������ Ÿ�̹�
        if (remainTime <= 0)
        {
            //Debug.Log("�Ѿ� ����!");
            //bulletPrefab ���赵�� ���� �Ѿ��� ����
            //Instantiate: �������� ���� ���� ������Ʈ �����ϱ�
            //Instantiate(������, ��ġ, ȸ��); ���� �����
            //GameObject bulletGameObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            //bullet.SetTarget(target);

            //GameObject bulletGameObj = bulletPool.GetBullet();
            //Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            //bullet.SetTarget(target, bulletPool);

            //bulletGameObj.transform.position = transform.position;
            //bulletGameObj.transform.rotation = transform.rotation;

            // �Ѿ� �������� �������� ����
            GameObject bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

            GameObject bulletGameObj = bulletPool.GetBullet(bulletPrefab);
            if (bulletGameObj != null)
            {
                Bullet bullet = bulletGameObj.GetComponent<Bullet>();
                bullet.SetTarget(target, bulletPool, bulletPrefab);

                bulletGameObj.transform.position = transform.position;
                bulletGameObj.transform.rotation = transform.rotation;

                //���� �Ѿ��� ������ ������ ���� �ð��� �ٽ� ����
                //remainTime = bulletTime;

                RandomFireTime();
            }
        }
    }

    //�Ѿ� �߻� �ð� �������� �����ϱ�
    private void RandomFireTime()
    {
        remainTime = Random.Range(minBulletTime, maxBulletTime);
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void StopAttack()
    {
        isAttacking = false;
    }
}
