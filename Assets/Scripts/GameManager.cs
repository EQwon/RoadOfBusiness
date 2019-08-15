using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepartmentName { 식품, 운송, 전자, 통신, 화학, 스포츠, 의류, 디스플레이 };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };

    private int day;
    public int Day { get { return day; } }
    private float time;

    private int money;
    public int Money { get { return money; } }

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

        if (Day % 30 == 0) //매달 초에 하는 일
        {
            PaySalary();
            day += 1; //다음 달 1일부터 시작하게 하기 위해
        }
    }

    public void UseMoney(int amount)
    {
        money -= amount;
    }

    public void CreateNewDepartment(int num)
    {
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
}
