using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHp : MonoBehaviour
{
    private int addHp = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<RubyController>().AddHp(addHp);
            gameObject.SetActive(false);
        }
    }
}
