using System.Collections;
using TMPro;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("ObjectPool")]
    public ObjectPool damageTextPool; // ������ �ؽ�Ʈ Ǯ
    public ObjectPool thornPool; // ���� ������Ʈ Ǯ
    public ObjectPool enemyPool; // �� ������Ʈ Ǯ
    public ObjectPool bossPool; // �� ������Ʈ Ǯ
    public ObjectPool goldeffectPool; // ��� ����Ʈ ������Ʈ Ǯ

    [Header("����")]
    public PlayerStatus status;

    // ������ �ؽ�Ʈ Ȱ��ȭ
    public void CreateDamageText(Vector3 hitPoint, double damage, bool isCritical)
    {
        GameObject damageTextObj = damageTextPool.GetObject();

        // Z���� 150���� ����
        hitPoint.z = 150f;
        damageTextObj.transform.position = hitPoint;
        damageTextObj.transform.rotation = Quaternion.identity;

        TextMeshPro damageText = damageTextObj.GetComponentInChildren<TextMeshPro>();
        damageText.text = TextFormatter.FormatText(damage);
        damageText.color = isCritical ? Color.red : Color.black;

        StartCoroutine(DeactivateObject(damageTextObj, 0.5f));
    }

    // ������ �ؽ�Ʈ ��Ȱ��ȭ
    private IEnumerator DeactivateObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        damageTextPool.ReturnObject(obj);
    }

    // ���� Ȱ��ȭ
    public void GetThorn(Vector3 position, Vector2 direction, double damage, bool isCritical)
    {
        GameObject thornObj = thornPool.GetObject();
        thornObj.transform.position = position;
        thornObj.transform.rotation = Quaternion.identity;

        Thorn thorn = thornObj.GetComponent<Thorn>();

        if (isCritical)
        {
            thorn.SetCriticalDamage((status.Critical_Damage / 100) * damage);
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
            enemy.Init();
        }
        return enemyObj;
    }

    // �� ��Ȱ��ȭ
    public void ReturnToEnemyPool(GameObject obj)
    {
        enemyPool.ReturnObject(obj);
    }

    // ���� Ȱ��ȭ
    public GameObject GetBoss(Vector3 position)
    {
        GameObject bossObj = bossPool.GetObject();
        bossObj.transform.position = position;
        bossObj.transform.rotation = Quaternion.identity;

        BossScript boss = bossObj.GetComponent<BossScript>();

        boss?.SetEnemyManager(FindObjectOfType<EnemyManager>());
        return bossObj;
    }

    // ���� ��Ȱ��ȭ
    public void ReturnToBossPool(GameObject obj)
    {
        bossPool.ReturnObject(obj);
    }

    // ��� ����Ʈ ��Ȱ��ȭ
    public GameObject GetGold(Vector3 position)
    {
        GameObject goldObj = goldeffectPool.GetObject();
        goldObj.transform.position = position;
        goldObj.transform.rotation = Quaternion.identity;
        
        return goldObj;
    }

    // ��� ����Ʈ ��Ȱ��ȭ
    public void ReturnToGoldPool(GameObject obj)
    {
        goldeffectPool.ReturnObject(obj);
    }
}