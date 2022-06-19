using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health;
    public int time;

    private void Start()
    {
        RecieveHealing();
    }

    private void Update()
    {
        if (time == 3 || health == 100)
        {
            StopCoroutine(HealingCoroutine());
        }
    }

    public void RecieveHealing()
    {
        StartCoroutine(HealingCoroutine());
    }

    IEnumerator HealingCoroutine()
    {
        for (int i = 0; i < 100; i++)
        {
            if (time < 3 && health < 100)
            {                
                health += 5;
                Debug.Log("plus 5 hp");
                yield return new WaitForSeconds(0.5f);
                time++;
            }            
        }
    }

}
