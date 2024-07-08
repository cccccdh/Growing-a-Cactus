using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float Attack;
    [SerializeField] private float HP;
    [SerializeField] private float HP_Recovery;
    [SerializeField] private float Attack_Speed;
    [SerializeField] private float Critical;
    [SerializeField] private float Critical_Damage;

    public void Init()
    {
        Attack = 10;
        HP = 120;
        HP_Recovery = 7;
        Attack_Speed = 1;
        Critical = 0;
        Critical_Damage = 120;
    }

    public void Increase(string status)
    {
        switch (status)
        {
            case "Attack":
                Attack += 10; 
                break;
            case "HP":
                HP += 10; 
                break;
            case "HP_Recovery":
                HP_Recovery += 0.7f; 
                break;
            case "Attack_Speed":
                Attack_Speed += 0.01f; 
                break;
            case "Critical":
                Critical += 2; 
                break;
            case "Critical_Damage":
                Critical_Damage += 10; 
                break;
        }
    }

    public void Increase_Attack()
    {

    }
}
