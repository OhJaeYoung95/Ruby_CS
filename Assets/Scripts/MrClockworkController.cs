using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MrClockworkController : MonoBehaviour
{
    public int maxHp = 3;
    private int currentHp;

    private float speed = 1f;
    private Vector2 offset = new Vector2(1f, 0f);

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 direction;

    public float duration = 10.0f;
    private float elapsedTime = 0.0f;

    private Vector2 leftPos;
    private Vector2 rightPos;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d=  GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        currentHp = maxHp;
        leftPos = rigidbody2d.position - offset;
        rightPos = rigidbody2d.position + offset;
    }

    private void FixedUpdate()
    {
        ////transform.position += direction * speed * Time.deltaTime;
        //Vector2 position = rigidbody2d.position;
        //position += direction * speed * Time.fixedDeltaTime;
        //rigidbody2d.MovePosition(position);
        Wander();

    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

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

    public void Wander()
    {

        float t = Mathf.Sin(Mathf.Clamp01(elapsedTime / duration) * Mathf.PI * 2);

        if(elapsedTime > duration)
        {
            elapsedTime = 0;
        }

        Vector2 position = rigidbody2d.position;
        position = Vector2.Lerp(leftPos, rightPos, t);
        rigidbody2d.MovePosition(position);
    }

    public void TakeDamage(int damage)
    {
        currentHp = Math.Clamp(currentHp - damage, 0, maxHp);
        Debug.Log(currentHp);
        if(currentHp <= 0f)
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        animator.SetTrigger("Fix");
        boxCollider2d.isTrigger = true;

    }

    IEnumerator CoTakeDamage()
    {
        yield return null;
    }
}