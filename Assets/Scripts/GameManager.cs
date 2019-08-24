using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepartmentName { 식품, 운송, 전자, 통신, 화학, 스포츠, 의류, 디스플레이 };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };

    public string companyName;
    private int day; //게임 시작 후 현재까지의 날짜
    public int Day { get { return day; } }
    private int money; //자본
    public int Money { get { return money; } }
    public int repuation; //평판
    private float satisfaction; //직원 만족도
    public float Satisfaction { get { return satisfaction; } }

    private float time;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (PlayerPrefs.HasKey("CompanyName")) companyName = PlayerPrefs.GetString("CompanyName");
        else companyName = "1조zs";
    }

    private void Start()
    {
        day = 1;
        time = 0;
        money = 530000;
        satisfaction = 500;
        repuation = 0;

        CreateNewDepartment(0);
    }

    private void Update()
    {
        //매 Update 마다 체크해야 하는 요소
        CheckGameOver();

        time += Time.deltaTime;

        if (time >= 0.1f)
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

    public void ChangeSatisfaction(float amount)
    {
        satisfaction += amount;
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

    private void CheckGameOver()
    {
        if (money <= 0 || satisfaction <= 0)
        {
            Debug.LogError("Game Over");
        }
    }

    public int TotalNeRevenue()
    {
        int amount = 0;

        foreach (Business business in department)
        {
            amount += business.earn;
        }

        return amount;
    }

    public int TotalWorkerAmount()
    {
        int amount = 0;

        foreach(Business business in department)
        {
            amount += business.worker[0] + business.worker[1] + business.worker[2];
        }

        return amount;
    }

    public void Donate()
    {
        UseMoney(10000);
        int reputationIncreasedAmount = Random.Range(1, 16);
        StartCoroutine(UIManager.instance.ShowReputationIncrease(reputationIncreasedAmount));
        repuation += reputationIncreasedAmount;
    }

    public void Welfare()
    {
        UseMoney(10000);
        int satisfactionIncreasedAmount = Random.Range(1, 21);
        StartCoroutine(UIManager.instance.ShowSatisfactionIncrease(satisfactionIncreasedAmount));
        satisfaction += satisfactionIncreasedAmount;
    }
}   
