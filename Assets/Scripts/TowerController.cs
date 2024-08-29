using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] public BulletPool bulletPool;

    [SerializeField] GameObject[] bulletPrefabs; // 총알 프리팹 배열

    //프리팹: 게임 오브젝트 설계도 => 유니티에서 게임 오브젝트를 생성할 때 복사할 원본
    //[SerializeField] GameObject bulletPrefab; //생성할 총알 프리팹

    //[SerializeField] float bulletTime; //총알 생성 시간
    [SerializeField] float minBulletTime;
    [SerializeField] float maxBulletTime;
    [SerializeField] float remainTime; //다음 총알 생성할 때까지 기다린 시간
    [SerializeField] bool isAttacking; //공격 여부

    private void Start()
    {
        //GameObject.FindGameObjectWithTag: 게임에 있는 태그를 통해서 특정 게임 오브젝트를 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        //===========================아래 두 코드가 같은 내용의 코드임=================================
        //하나만 사용해도 무관
        //GetComponent: 게임 오브젝트에 있는 컴포넌트 가져오기
        target = playerObj.GetComponent<Transform>();

        //transform: 게임 오브젝트의 위치, 회전, 크기를 관리하는 기능담당자
        //transform 컴포넌트는 몯든 게임 오브젝트에 반드시 있음 => transform 컴포넌트만 특별하게 프로퍼티로 바로 사용 가능
        target = playerObj.transform;
        //===========================================================================================

        RandomFireTime();
    }

    private void Update()
    {
        //공격 중이 아닐 때는 총알을 생성하지 않도록 함
        if (isAttacking == false)
            return;

        //다음 총알 생성까지 남은 시간을 계속 차감
        remainTime -= Time.deltaTime;

        //다음 총알을 생성할 떄까지 남은 시간이 없는 경우 == 총알을 생성할 타이밍
        if (remainTime <= 0)
        {
            //Debug.Log("총알 생성!");
            //bulletPrefab 설계도를 토대로 총알을 생성
            //Instantiate: 프리팹을 토대로 게임 오브젝트 생성하기
            //Instantiate(프리팹, 위치, 회전); 으로 사용함
            //GameObject bulletGameObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            //bullet.SetTarget(target);

            //GameObject bulletGameObj = bulletPool.GetBullet();
            //Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            //bullet.SetTarget(target, bulletPool);

            //bulletGameObj.transform.position = transform.position;
            //bulletGameObj.transform.rotation = transform.rotation;

            // 총알 프리팹을 랜덤으로 선택
            GameObject bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];

            GameObject bulletGameObj = bulletPool.GetBullet(bulletPrefab);
            if (bulletGameObj != null)
            {
                Bullet bullet = bulletGameObj.GetComponent<Bullet>();
                bullet.SetTarget(target, bulletPool, bulletPrefab);

                bulletGameObj.transform.position = transform.position;
                bulletGameObj.transform.rotation = transform.rotation;

                //다음 총알을 생성할 때까지 남은 시간을 다시 설정
                //remainTime = bulletTime;

                RandomFireTime();
            }
        }
    }

    //총알 발사 시간 랜덤으로 설정하기
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
