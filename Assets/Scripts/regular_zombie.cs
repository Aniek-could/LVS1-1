using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Animations;

enum ZombieState//��ʬ״̬
{
    Move,
    Eat,//����
    Die
}

public class Regular_zombie : Zombie
{

    private Plant currentEatPlant;//��ǰ�Ե�ֲ��

    ZombieState zombieState = ZombieState.Move;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (zombieState)
        {
            case ZombieState.Move:
                MoveUpdate();
                break;
            case ZombieState.Eat:
                break;
            case ZombieState.Die:
                break;
            default:
                break;
        }
    }

    void MoveUpdate()
    {
        rb.MovePosition(rb.position + Vector2.left * speed * Time.deltaTime * direction);//Ĭ������
    }

    void EatUpdate()
    {
        atkTimer += Time.deltaTime;
        if (atkTimer > atkDuration && currentEatPlant != null)
        {
            currentEatPlant.TakeDamage(atkValue);
            atkTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "plant")
        {
            animator.SetBool("IsAttacking", true);
            TransitionToEat();
            currentEatPlant = other.GetComponent<Plant>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "plant")
        {
            animator.SetBool("IsAttacking", false);
            zombieState = ZombieState.Move;
            currentEatPlant = null;
        }
    }

    void TransitionToEat()//ת��״̬-����
    {
        zombieState = ZombieState.Eat;
        atkTimer = 0;
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;

        this.currentHP -= damage;
        if (currentHP <= 0)//����
        {
            currentHP = -1;
            TransitionToDie();
        }
        float hpPercent = currentHP * 1f / HP;//Ѫ��ֵ
        animator.SetFloat("HPPercent", hpPercent);
    }
    
    void TransitionToDie()
    {
        zombieState = ZombieState.Die;
        GetComponent<Collider2D>().enabled = false;

        Destroy(this.gameObject, 2);//�������������
    }
}
