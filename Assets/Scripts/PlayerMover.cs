using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

//������Ʈ: ���� ������Ʈ�� ����� ����ϴ� ��ǰ
public class PlayerMover : MonoBehaviour
{
    //�ν����� â���� ������ �����ϰų� ���� �������־ ���� ���
    //�������: ����
    //[SerializeField]: private������ �ν����Ϳ��� Ȯ���ϴ� ��
    [SerializeField] Rigidbody rigid;
    [SerializeField] float moveSpeed;

    //�÷��̾ �׾��� ��
    public event Action OnDied; 

    //����Ƽ �޽��� �Լ����� ������ ���ҿ� �°� ä�� ������ ��ɿ� ���ؼ� ����
    //����Լ�: ����
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //�Է� ���� �޾� ����
        //�Է¸Ŵ���: Edit -> ProjectSetting���� ������ �̸��� �Է� ����� ���
        //GetAxis(): �� �Է� -1 ~ 1 float �� => ���� ��ƽó�� ���� �Է��ϴ� �� ����
        //GetAxisRaw(): �� �Է� -1, 0, 1 float�������� �Ҽ��� ���� �Է� ���� �Ǵ� => Ű����ó�� ������ �ִ� ��쿡 ����
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //�ܼ�: ������ ������ �ؽ�Ʈ ���·� �����ڿ��� �˷��ִ� â
        //Debug.Log($"{x}, {z}");

        //����ȭ: ũ�Ⱑ 1�� �ƴ� ���͸� ũ�⸦ 1�� �����
        Vector3 moveDir = new Vector3(x, 0, z);
        
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        //������ٵ�: �������� ����� ������Ʈ => FixedUpdate()���� ���ư��� ���� (������ �ӵ�(=moveSpeed) ��ŭ)
        //AddForce, AddTorque, Velocity, angleVelocity
        rigid.velocity = moveDir * moveSpeed;

        //Ʈ������: ��ġ, ȸ��, ũ�� ����� ������Ʈ
        //Translate, position, MoveTowardm Lerp/Rotate, rotation(���ʹϾ� -> ���Ϸ�), RotateAround, LookAt
    }

    public void TakeHit()
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }

}
