using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
