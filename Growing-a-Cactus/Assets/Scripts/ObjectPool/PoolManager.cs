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
    public ObjectPool damageTextPool; // 데미지 텍스트 풀
    public ObjectPool thornPool; // 가시 오브젝트 풀
    public ObjectPool enemyPool; // 적 오브젝트 풀
    public ObjectPool bossPool; // 적 오브젝트 풀
    public ObjectPool goldeffectPool; // 골드 이펙트 오브젝트 풀

    [Header("참조")]
    public PlayerStatus status;

    // 데미지 텍스트 활성화
    public void CreateDamageText(Vector3 hitPoint, double damage, bool isCritical)
    {
        GameObject damageTextObj = damageTextPool.GetObject();

        // Z값을 150으로 설정
        hitPoint.z = 150f;
        damageTextObj.transform.position = hitPoint;
        damageTextObj.transform.rotation = Quaternion.identity;

        TextMeshPro damageText = damageTextObj.GetComponentInChildren<TextMeshPro>();
        damageText.text = TextFormatter.FormatText(damage);
        damageText.color = isCritical ? Color.red : Color.black;

        StartCoroutine(DeactivateObject(damageTextObj, 0.5f));
    }

    // 데미지 텍스트 비활성화
    private IEnumerator DeactivateObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        damageTextPool.ReturnObject(obj);
    }

    // 가시 활성화
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
            enemy.Init();
        }
        return enemyObj;
    }

    // 적 비활성화
    public void ReturnToEnemyPool(GameObject obj)
    {
        enemyPool.ReturnObject(obj);
    }

    // 보스 활성화
    public GameObject GetBoss(Vector3 position)
    {
        GameObject bossObj = bossPool.GetObject();
        bossObj.transform.position = position;
        bossObj.transform.rotation = Quaternion.identity;

        BossScript boss = bossObj.GetComponent<BossScript>();

        boss?.SetEnemyManager(FindObjectOfType<EnemyManager>());
        return bossObj;
    }

    // 보스 비활성화
    public void ReturnToBossPool(GameObject obj)
    {
        bossPool.ReturnObject(obj);
    }

    // 골드 이펙트 비활성화
    public GameObject GetGold(Vector3 position)
    {
        GameObject goldObj = goldeffectPool.GetObject();
        goldObj.transform.position = position;
        goldObj.transform.rotation = Quaternion.identity;
        
        return goldObj;
    }

    // 골드 이펙트 비활성화
    public void ReturnToGoldPool(GameObject obj)
    {
        goldeffectPool.ReturnObject(obj);
    }
}