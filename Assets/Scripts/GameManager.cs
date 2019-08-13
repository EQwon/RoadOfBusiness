using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DepartmentName { 식품, 운송, 전자, 통신, 화학, 스포츠, 의류, 디스플레이 };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };

    private int day;
    private float time;

    private int money;
    public int Money { get { return money; } }

    [Header("UI Holder")]
    public Text DateText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        day = 1;
        time = 0;
        money = 45000;

        CreateNewDepartment(DepartmentName.식품);
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 5f)
        {
            day += 1;
            time = 0;
        }
    }

    public void UseMoney(int amount)
    {
        money -= amount;
    }

    public void CreateNewDepartment(DepartmentName name)
    {
        Business business = new Business();
        business.Initialize(name);
        department.Add(business);
    }
}
