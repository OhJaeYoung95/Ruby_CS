using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MrClockworkController : MonoBehaviour
{
    public int maxHp = 5;
    private int currentHp;

    private float speed = 4f;
    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 direction;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHp = maxHp;
    }

    private void FixedUpdate()
    {
        //transform.position += direction * speed * Time.deltaTime;
        Vector2 position = rigidbody2d.position;
        position += direction * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }


    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        direction = new Vector3(h, v);

        var directionMag = direction.magnitude;
        if (directionMag > 1f)
        {
            direction.Normalize();
        }

        if (directionMag > 0f)
        {
            lookDirection = direction;
            lookDirection.Normalize();
        }

        animator.SetFloat("Speed", directionMag);
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
    }

    public void TakeDamage(int damage)
    {
        currentHp = Math.Clamp(currentHp - damage, 0, maxHp);
        Debug.Log(currentHp);
    }

    IEnumerator CoTakeDamage()
    {
        yield return null;
    }
}