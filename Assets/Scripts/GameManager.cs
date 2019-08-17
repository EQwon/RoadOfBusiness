using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepartmentName { 식품, 운송, 전자, 통신, 화학, 스포츠, 의류, 디스플레이 };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };

    private int day; //게임 시작 후 현재까지의 날짜
    public int Day { get { return day; } }
    private int money; //자본
    public int Money { get { return money; } }
    public int repuation; //평판
    private int creditRate; //신용도
    public int CreditRate { get { return creditRate; } }
    private int satisfaction; //직원 만족도
    public int Satisfaction { get { return satisfaction; } }

    private float time;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        day = 1;
        time = 0;
        money = 530000;

        CreateNewDepartment(0);
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.5f)
        {
            day += 1;
            time = 0;
        }

        foreach (Business business in department)
        {
            if (business.Period == 0)
            {
                Debug.Log(business.name + " 부서 신제품 개발 완료!");
            }
        }

        if (Day % 30 == 0) //매달 초에 하는 일
        {
            Earn();
            PaySalary();
            day += 1; //다음 달 1일부터 시작하게 하기 위해
        }
    }

    public void UseMoney(int amount)
    {
        money -= amount;
    }

    public void GainMoney(int a)
    {
        money += a;
    }

    public void CreateNewDepartment(int num)
    {
        money -= 100000 + (department.Count - 1) * 70000;
        Business business = new Business();
        business.Initialize(((DepartmentName)num).ToString());
        department.Add(business);
    }

    public void StartNewDevelop(int num)
    {
        //'현재 자본'을 저장해둔다.
        int nowMoney = money;

        //1. 신제품 개발에 필요한 금액 차감 = 현재 자본 / 12
        UseMoney(nowMoney / 12);

        //2. 기본 출시기간 설정 = 현재 자본 / 9000
        department[num].SetPeriod(nowMoney / 9000);
    }

    private void PaySalary()
    {
        for (int i = 0; i < department.Count; i++)
        {
            UseMoney(department[i].MonthlyPay());
        }
    }

    private void Earn()
    {
        for (int i = 0; i < department.Count; i++)
        {
            GainMoney(department[i].earn);
        }
    }
}   
