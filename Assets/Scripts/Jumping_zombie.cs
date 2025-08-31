using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

enum Jumping_ZombieState
{
    Move,
    Jumping,
    Eat,//����
    Die
}

public class Jumping_zombie : Zombie
{
    Jumping_ZombieState Jumping_zombieState = Jumping_ZombieState.Move;

    private Plant currentEatPlant;//��ǰ�Ե�ֲ��

    //跳跃
    private float jumpDistance = 1.5f;        // 跳跃跨越的距离（跳过植物）
    private float jumpHeight = 1.5f;            // 跳跃高度
    private bool hasJumped = false;

    // Update is called once per frame
    void Update()
    {
        switch (Jumping_zombieState)
        {
            case Jumping_ZombieState.Move:
                MoveUpdate();
                break;
            case Jumping_ZombieState.Jumping:
                break;
            case Jumping_ZombieState.Eat:
                EatUpdate();
                break;
            case Jumping_ZombieState.Die:
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
            if (!hasJumped)
            {
                TransitionToJumping();
                StartCoroutine(PerformJump());
            }
            else
            {
                animator.SetBool("IsAttacking", true);
                TransitionToEat();
                currentEatPlant = other.GetComponent<Plant>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "plant" && hasJumped)
        {
            animator.SetBool("IsAttacking", false);
            Jumping_zombieState = Jumping_ZombieState.Move;
            currentEatPlant = null;
        }
    }

    void TransitionToEat()//ת��״̬-����
    {
        Jumping_zombieState = Jumping_ZombieState.Eat;
        atkTimer = 0;
    }


    void TransitionToDie()
    {
        Jumping_zombieState = Jumping_ZombieState.Die;
        GetComponent<Collider2D>().enabled = false;

        Destroy(this.gameObject, 2);//�������������
    }

    void TransitionToJumping()
    {
        Jumping_zombieState = Jumping_ZombieState.Jumping;
        hasJumped = true;
    }

    IEnumerator PerformJump()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;

        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + Vector2.left * jumpDistance * direction;
        float jumpDuration = 1f; // 跳跃持续时间
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = 4 * jumpHeight * t * (1 - t); // 抛物线公式
            rb.position = Vector2.Lerp(startPos, endPos, t) + Vector2.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = endPos; // 确保最终位置准确

        gameObject.GetComponent<Collider2D>().enabled = true;
        Jumping_zombieState = Jumping_ZombieState.Move;
    }
    
}
