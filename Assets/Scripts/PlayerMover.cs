using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

//컴포넌트: 게임 오브젝트의 기능을 담당하는 부품
public class PlayerMover : MonoBehaviour
{
    //인스펙터 창에서 변수를 참조하거나 값을 지정해주어서 쓰는 경우
    //멤버변수: 정보
    //[SerializeField]: private이지만 인스펙터에서 확인하는 것
    [SerializeField] Rigidbody rigid;
    [SerializeField] float moveSpeed;

    //플레이어가 죽었을 때
    public event Action OnDied; 

    //유니티 메시지 함수들을 각각의 역할에 맞게 채워 넣으며 기능에 대해서 구현
    //멤버함수: 동작
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //입력 값을 받아 저장
        //입력매니저: Edit -> ProjectSetting에서 설정한 이름의 입력 방식을 사용
        //GetAxis(): 축 입력 -1 ~ 1 float 값 => 조이 스틱처럼 조금 입력하는 거 가능
        //GetAxisRaw(): 축 입력 -1, 0, 1 float값이지만 소수점 없이 입력 여부 판단 => 키보드처럼 누르고 있는 경우에 적합
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //콘솔: 게임의 정보를 텍스트 형태로 제작자에게 알려주는 창
        //Debug.Log($"{x}, {z}");

        //정규화: 크기가 1이 아닌 백터를 크기를 1로 만들기
        Vector3 moveDir = new Vector3(x, 0, z);
        
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        //리지드바디: 물리엔진 담당의 컴포넌트 => FixedUpdate()에서 돌아가고 있음 (지정된 속도(=moveSpeed) 만큼)
        //AddForce, AddTorque, Velocity, angleVelocity
        rigid.velocity = moveDir * moveSpeed;

        //트랜스폼: 위치, 회전, 크기 담당의 컴포넌트
        //Translate, position, MoveTowardm Lerp/Rotate, rotation(쿼터니언 -> 오일러), RotateAround, LookAt
    }

    public void TakeHit()
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }

}
