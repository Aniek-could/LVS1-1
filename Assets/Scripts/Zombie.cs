using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Zombie : MonoBehaviour
{
    public float speed;//�ƶ��ٶ�

    public int direction;//����or����

    public Animator animator;
    public Rigidbody2D rb;

    //����
    public int atkValue;//����ֵ
    public float atkDuration;//����Ƶ��
    public float atkTimer = 0;

    public int HP = 100;
    public int currentHP;

    // Start is called before the first frame update
    void Start()
    {

    }

}
