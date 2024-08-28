using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //[SerializeField] Rigidbody rigid;
    //[SerializeField] Vector3 desrination;
    //[SerializeField] float speed;

    //private void Start()
    //{
    //    //LookAt: 회전 중 위치를 바라보도록 회전
    //    transform.LookAt(desrination);

    //    //총알의 속도를 앞방향 speed 만큼으로 설정
    //    rigid.velocity = transform.forward * speed;
    //}
    //public void SetDestination(Vector3 destination)
    //{
    //    this.desrination = destination;
    //}

    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    [SerializeField] Transform target;

    private void Start()
    {
        //LookAt: 타겟 방향을 바라보는 회전
        transform.LookAt(target.position);

        rigid.velocity = transform.forward * speed;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    //충돌(Collision): 물리적 충돌을 확인
    //유니티 충돌 메시지 함수
    //트리거(Trigger): 겹침여부 확인
    //유니티 트리거 메시지 함수

    private void OnCollisionEnter(Collision collision)
    {
        //Collision 매개변수: 충돌한 상황에 대한 정보들을 가지고 있다 (ex: 충돌한 다른 충돌체, 받은 힘, 부딪힌 지점 등)    
        if(collision.gameObject.tag == "Player")
        {
            PlayerMover playerMover = collision.gameObject.GetComponent<PlayerMover>();
            playerMover.TakeHit();
        }
        //Destroy: 게임 오브젝트 삭제
        Destroy(gameObject);
    }

}
