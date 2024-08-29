using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //[SerializeField] GameObject bulletPrefab;
    //[SerializeField] int initialPoolSize = 10;

    //직렬화: https://wlsdn629.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-SystemSerializable%EC%97%90-%EB%8C%80%ED%95%B4-%EC%95%8C%EC%95%84%EB%B3%B4%EC%9E%90
    [System.Serializable]
    public class BulletType
    {
        public GameObject bulletPrefab;
        public int initialPoolSize;
        public float speed;
    }

    [SerializeField] BulletType[] bulletTypes;

    //private Queue<GameObject> pool = new Queue<GameObject>();
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Start()
    {
        foreach (var bulletType in bulletTypes)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < bulletType.initialPoolSize; i++)
            {
                GameObject bullet = Instantiate(bulletType.bulletPrefab);
                bullet.transform.parent = transform;
                bullet.SetActive(false);
                pool.Enqueue(bullet);
            }

            pools[bulletType.bulletPrefab] = pool;
        }
    }

    public GameObject GetBullet(GameObject bulletPrefab, Transform target, float speed)
    {
        if (pools.ContainsKey(bulletPrefab) && pools[bulletPrefab].Count > 0)
        {
            GameObject bullet = pools[bulletPrefab].Dequeue();
            bullet.SetActive(true);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(target, this, bulletPrefab, speed);
            return bullet;
        }
        else
        {
            // Pool이 비어있는 경우 새로운 총알을 생성
            GameObject bullet = Instantiate(bulletPrefab);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(target, this, bulletPrefab, speed);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bulletPrefab, GameObject bullet)
    {
        if (pools.ContainsKey(bulletPrefab))
        {
            bullet.SetActive(false);
            pools[bulletPrefab].Enqueue(bullet);
        }
        else
        {
            Destroy(bullet);
        }
    }
}
