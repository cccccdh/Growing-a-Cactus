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

    public ObjectPool damageTextPool; // ������ �ؽ�Ʈ Ǯ
    public ObjectPool thornPool; // ���� ������Ʈ Ǯ
    public ObjectPool enemyPool; // �� ������Ʈ Ǯ

    // ������ �ؽ�Ʈ Ȱ��ȭ
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

    // ������ �ؽ�Ʈ ��Ȱ��ȭ
    private IEnumerator DeactivateObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        damageTextPool.ReturnObject(obj);
    }

    // ���� Ȱ��ȭ
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

    // ���� ��Ȱ��ȭ
    public void ReturnToThornPool(GameObject obj)
    {
        thornPool.ReturnObject(obj);
    }

    // �� Ȱ��ȭ
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

    // �� ��Ȱ��ȭ
    public void ReturnToEnemyPool(GameObject obj)
    {
        enemyPool.ReturnObject(obj);
    }
}