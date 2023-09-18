using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    public Image hpGauageUI;

    private ParticleSystem hitParticle;

    public Projectile projectilePrefab;
    public float projectileForce = 10f;

    public int maxHp = 10;
    private int preHp;
    [SerializeField]
    private int currentHp;

    private float speed = 4f;
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip pickupItem;


    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 direction;

    private bool isInvincible = false;
    public float timeInvincible = 1f;
    private float invincibleTimer;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        hitParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        currentHp = maxHp;
        hpGauageUI.fillAmount = currentHp / maxHp;
        //hitParticle.Stop();
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

        if (isInvincible)
        {
            LerpHpUI();
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
            {
                isInvincible = false;
                spriteRenderer.color = Color.white;
            }
        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        direction = new Vector3(h, v);

        var directionMag = direction.magnitude;
        if (directionMag > 1f)
        {
            direction.Normalize();
        }

        if (directionMag > 0f )
        {
            lookDirection = direction;
            lookDirection.Normalize();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            var lookNormalized = lookDirection.normalized;

            var pos = rigidbody2d.position;
            pos.y += 0.5f;
            pos += lookNormalized * 0.5f;


            var projectile = Instantiate(projectilePrefab, pos, Quaternion.identity);
            projectile.Launch(lookNormalized, projectileForce);
            animator.SetTrigger("Launch");
        }

        animator.SetFloat("Speed", directionMag);
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        spriteRenderer.color = Color.red;
        preHp = currentHp;
        currentHp = Math.Clamp(currentHp - damage, 0, maxHp);

        isInvincible = true;
        invincibleTimer = timeInvincible;

        animator.SetTrigger("Hit");
        SetClipNPlay(hitSound);
        hitParticle.Stop();
        hitParticle.Play();
    }

    public void AddHp(int addHp)
    {
        Debug.Log($"Heal Before: {currentHp} + {addHp}");
        currentHp = Math.Clamp(currentHp + addHp, 0, maxHp);
        SetClipNPlay(pickupItem);
        Debug.Log($"Heal After : {currentHp}");
    }

    public void LerpHpUI()
    {
        hpGauageUI.fillAmount = (Mathf.Lerp(currentHp, preHp, invincibleTimer / timeInvincible) / (float)maxHp);
    }

    public void SetClipNPlay(AudioClip setClip)
    {
        audioSource.clip = setClip;
        audioSource.PlayOneShot(setClip);
    }

    IEnumerator CoTakeDamage()
    {
        yield return null;
    }
}
