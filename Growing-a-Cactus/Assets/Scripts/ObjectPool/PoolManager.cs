using System.Collections;
using TMPro;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PoolManager>();
            return instance;
        }
    }

    public ObjectPool damageTextPool; // 데미지 텍스트 풀
    public ObjectPool thornPool; // 가시 오브젝트 풀
    public ObjectPool enemyPool; // 적 오브젝트 풀

    // 데미지 텍스트 활성화
    public void CreateDamageText(Vector3 hitPoint, float damage, bool isCritical)
    {
        GameObject damageTextObj = damageTextPool.GetObject();
        damageTextObj.transform.position = hitPoint;
        damageTextObj.transform.rotation = Quaternion.identity;

        TextMeshPro damageText = damageTextObj.GetComponentInChildren<TextMeshPro>();
        damageText.text = TextFormatter.FormatText(damage);
        damageText.color = isCritical ? Color.red : Color.white;

        StartCoroutine(DeactivateObject(damageTextObj, 1.0f));
    }

    // 데미지 텍스트 비활성화
    private IEnumerator DeactivateObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        damageTextPool.ReturnObject(obj);
    }

    // 가시 활성화
    public void GetThorn(Vector3 position, Vector2 direction, float damage, bool isCritical)
    {
        GameObject thornObj = thornPool.GetObject();
        thornObj.transform.position = position;
        thornObj.transform.rotation = Quaternion.identity;

        Thorn thorn = thornObj.GetComponent<Thorn>();

        if (isCritical)
        {
            thorn.SetCriticalDamage(damage);
        }
        else
        {
            thorn.SetDamage(damage);
        }

        thorn.SetDirection(direction);
    }

    // 가시 비활성화
    public void ReturnToThornPool(GameObject obj)
    {
        thornPool.ReturnObject(obj);
    }

    // 적 활성화
    public GameObject GetEnemy(Vector3 position)
    {
        GameObject enemyObj = enemyPool.GetObject();
        enemyObj.transform.position = position;
        enemyObj.transform.rotation = Quaternion.identity;

        EnemyScript enemy = enemyObj.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.SetEnemyManager(FindObjectOfType<EnemyManager>());
        }
        return enemyObj;
    }

    // 적 비활성화
    public void ReturnToEnemyPool(GameObject obj)
    {
        enemyPool.ReturnObject(obj);
    }
}