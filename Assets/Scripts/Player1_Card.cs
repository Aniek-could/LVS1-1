using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Player1_Card : MonoBehaviour
{
    #region  卡牌区域
    [SerializeField]
    private List<GameObject> Plant_cardObject;//��ҵ�ֲ�����ſ���

    [SerializeField]
    private List<GameObject> Zombie_cardOject;//��ҵĽ�ʬ���ſ���

    private List<GameObject> curent_cardObject;
    private static bool isPlant_card = true;
    #endregion

    #region  放置卡牌期间
    [SerializeField]
    private List<GameObject> Plant_cardPrefab;//���Ԥ����(�޶���)

    [SerializeField]
    private List<GameObject> Zombie_cardPrefab;

    private List<GameObject> curent_cardPerfab;
    #endregion

    #region  放置到格子中
    [SerializeField]
    private List<GameObject> really_PlantcardPerab;//���Ԥ����(�ж���+�ű�)

    [SerializeField]
    private List<GameObject> really_ZombiecardPerab;

    private List<GameObject> really_currentcardPerab;
    #endregion

    //��ƺ�ĳ���
    private static int Grid_X = 4;
    private static int Grid_Y = 4;

    [SerializeField]
    private Transform[] gridPointsFlat = new Transform[Grid_X * Grid_Y]; // Inspector�ɼ�

    private Transform[,] gridPoints = new Transform[Grid_X, Grid_Y];// 4*4���ӵĲ�ƺ���ӣ���ά��

    private GameObject[,] plantObjects = new GameObject[Grid_X, Grid_Y];//��¼�ø�����ֲ��ֲ��

    private bool[,] hasPlant = new bool[Grid_X, Grid_Y];// ���ÿ�������Ƿ���ֲ��

    //ѡ��
    private bool isSelectCard = true;
    private int selecrIndex = 0;
    private const float selectCardScale = 1.5f;
    private const float unSelectCardScale = 1.0f;

    //�ſ���
    private bool isSelectPlace = false;
    private GameObject isSelectObject;//ѡ�е����
    private int selectGridIndex_X = 0;//ѡ�еĸ���X��
    private int selectGridIndex_Y = 0;//ѡ�еĸ���Y��

    private int shovel_index;//���ӿ��Ƶ�index
    private bool shovel_choice = false;//�Ƿ�ѡ���˲���

    private void Awake()
    {
        for (int x = 0; x < Grid_X; x++)
        {
            for (int y = 0; y < Grid_Y; y++)
            {
                gridPoints[x, y] = gridPointsFlat[y * Grid_X + x];
            }
        }

        curent_cardObject = Plant_cardObject;
        really_currentcardPerab = really_PlantcardPerab;
        curent_cardPerfab = Plant_cardPrefab;
    }

    void Start()
    {
        SelectCard(selecrIndex);
        isSelectObject = null;

        shovel_index = Plant_cardObject.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))//换卡槽
        {
            isPlant_card = !isPlant_card;
            if (isPlant_card)
            {
                curent_cardObject = Plant_cardObject;
                curent_cardPerfab= Plant_cardPrefab;
                really_currentcardPerab = really_PlantcardPerab;
                Disappear_Card(Zombie_cardOject);
                Appear_Card(Plant_cardObject);
            }
            else
            {
                curent_cardObject = Zombie_cardOject;
                curent_cardPerfab = Zombie_cardPrefab;
                really_currentcardPerab = really_ZombiecardPerab;
                shovel_choice = false;
                Disappear_Card(Plant_cardObject);
                Appear_Card(Zombie_cardOject);
            }

            SelectCard(selecrIndex);
        }

        if (isSelectCard)//ѡ���ڼ�
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                selecrIndex = (selecrIndex + 1) % Plant_cardObject.Count;
                SelectCard(selecrIndex);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SelectCard(-1);

                if (shovel_index == selecrIndex) shovel_choice = true;
                Debug.Log($"����+{shovel_choice}");

                selectGridIndex_X = 0;
                selectGridIndex_Y = 0;
                isSelectObject = Instantiate(curent_cardPerfab[selecrIndex], gridPoints[selectGridIndex_X, selectGridIndex_Y].position, Quaternion.identity);//ʵ����ѡ�е�����

                isSelectCard = false;
                isSelectPlace = true;
            }
        }

        if (isSelectPlace)
        {
            bool move = false;
            if (Input.GetKeyDown(KeyCode.W) && selectGridIndex_Y > 0)
            {
                selectGridIndex_Y--;
                move = true;
            }

            if (Input.GetKeyDown(KeyCode.S) && selectGridIndex_Y < Grid_Y - 1)
            {
                selectGridIndex_Y++;
                move = true;
            }

            if (Input.GetKeyDown(KeyCode.A) && selectGridIndex_X > 0)
            {
                selectGridIndex_X--;
                move = true;
            }

            if (Input.GetKeyDown(KeyCode.D) && selectGridIndex_X < Grid_X - 1)
            {
                selectGridIndex_X++;
                move = true;
            }

            if (move)
            {
                isSelectObject.transform.position = gridPoints[selectGridIndex_X, selectGridIndex_Y].position;
                move = false;
            }

            //ȷ��λ��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hasPlant[selectGridIndex_X, selectGridIndex_Y] && shovel_choice)
                {
                    Destroy(plantObjects[selectGridIndex_X, selectGridIndex_Y]);//�ݻ�ֲ��
                    hasPlant[selectGridIndex_X, selectGridIndex_Y] = false;
                }
                else if (hasPlant[selectGridIndex_X, selectGridIndex_Y] && !shovel_choice && isPlant_card)
                {
                    return;
                }
                else if (!hasPlant[selectGridIndex_X, selectGridIndex_Y] && shovel_choice)
                {

                }
                else
                {
                    if (!isPlant_card)//��ʬ����Ҫռ�ø���
                    {
                        Instantiate(really_currentcardPerab[selecrIndex], gridPoints[selectGridIndex_X, selectGridIndex_Y].position, Quaternion.Euler(0, -180, 0));//��¼��λ�õ�ֲ��
                    }
                    else
                    {
                        plantObjects[selectGridIndex_X, selectGridIndex_Y] = Instantiate(really_currentcardPerab[selecrIndex], gridPoints[selectGridIndex_X, selectGridIndex_Y].position, Quaternion.identity);//��¼��λ�õ�ֲ��
                        hasPlant[selectGridIndex_X, selectGridIndex_Y] = true;
                    }

                }

                Destroy(isSelectObject);
                isSelectObject = null;

                shovel_choice = false;
                isSelectPlace = false;
                isSelectCard = true;

                selecrIndex = 0;
                SelectCard(selecrIndex);
            }
        }
    }

    private void SelectCard(int index)
    {
        Debug.Log($"植物: {(isPlant_card ? "是" : "否")}");
        Debug.Log(index);
        for (int i = 0; i < curent_cardObject.Count; i++)
        {
            if (i == index)
                curent_cardObject[i].transform.localScale = Vector3.one * selectCardScale;
            else
                curent_cardObject[i].transform.localScale = Vector3.one * unSelectCardScale;
        }
    }

    private void Disappear_Card(List<GameObject> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].SetActive(false);
        }
    }
    
    private void Appear_Card(List<GameObject> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].SetActive(true);
        }
    }
}
