using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus status;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    void Start()
    {
        status.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {            
            status.Increase("Attack");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            status.Increase("HP");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
        }
    }

    public void Attack()
    {
        
    }
}
