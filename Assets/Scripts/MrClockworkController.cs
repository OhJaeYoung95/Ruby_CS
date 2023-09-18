using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MrClockworkController : MonoBehaviour
{
    public Image hpGauageUI;

    public ParticleSystem smogEffect;
    public ParticleSystem attackEffect;

    public int maxHp = 3;
    private int currentHp;

    public float moveSpeed = 5f;
    public Vector2 offset = new Vector2(10f, 0f);

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 direction;

    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 currentTarget;

    private Vector2 targetPosition;
    private Vector2 newPosition;

    private bool isFixed = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        currentHp = maxHp;
        pointA = rigidbody2d.position - offset;
        pointB = rigidbody2d.position + offset;
        currentTarget = pointA;
        smogEffect.Play();
        attackEffect.Stop();

        hpGauageUI.fillAmount = currentHp / maxHp;
    }

    private void FixedUpdate()
    {
        if (isFixed)
            return;

        ////transform.position += direction * speed * Time.deltaTime;
        //Vector2 position = rigidbody2d.position;
        //position += direction * speed * Time.fixedDeltaTime;
        //rigidbody2d.MovePosition(position);
        PatrolPhysics();

    }


    // Update is called once per frame
    void Update()
    {
        if (isFixed)
            return;
        Patrol();
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

    public void PatrolPhysics()
    {
        targetPosition = currentTarget;
        newPosition = Vector2.MoveTowards(rigidbody2d.position, targetPosition, moveSpeed * Time.deltaTime);
        rigidbody2d.MovePosition(newPosition);
    }

    public void Patrol()
    {
        direction = (targetPosition - rigidbody2d.position).normalized;

        if (Vector2.Distance(rigidbody2d.position, targetPosition) < 0.01f)
        {
            if (currentTarget == pointA)
                currentTarget = pointB;
            else
                currentTarget = pointA;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHp = Math.Clamp(currentHp - damage, 0, maxHp);
        attackEffect.Stop();
        attackEffect.Play();
        hpGauageUI.fillAmount = (float)currentHp / (float)maxHp;

        Debug.Log(currentHp);
        if(currentHp <= 0f)
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        isFixed = true;
        animator.SetTrigger("Fix");
        boxCollider2d.enabled = false;
        smogEffect.Stop();
        var mainModule = attackEffect.main;
        mainModule.loop = true;

        attackEffect.Play();

    }

    IEnumerator CoTakeDamage()
    {
        yield return null;
    }
}