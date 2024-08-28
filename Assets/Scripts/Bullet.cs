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
    //    //LookAt: ȸ�� �� ��ġ�� �ٶ󺸵��� ȸ��
    //    transform.LookAt(desrination);

    //    //�Ѿ��� �ӵ��� �չ��� speed ��ŭ���� ����
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
        //LookAt: Ÿ�� ������ �ٶ󺸴� ȸ��
        transform.LookAt(target.position);

        rigid.velocity = transform.forward * speed;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    //�浹(Collision): ������ �浹�� Ȯ��
    //����Ƽ �浹 �޽��� �Լ�
    //Ʈ����(Trigger): ��ħ���� Ȯ��
    //����Ƽ Ʈ���� �޽��� �Լ�

    private void OnCollisionEnter(Collision collision)
    {
        //Collision �Ű�����: �浹�� ��Ȳ�� ���� �������� ������ �ִ� (ex: �浹�� �ٸ� �浹ü, ���� ��, �ε��� ���� ��)    
        if(collision.gameObject.tag == "Player")
        {
            PlayerMover playerMover = collision.gameObject.GetComponent<PlayerMover>();
            playerMover.TakeHit();
        }
        //Destroy: ���� ������Ʈ ����
        Destroy(gameObject);
    }

}
