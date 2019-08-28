using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DepartmentName { 식품, 운송, 전자, 통신, 화학, 스포츠, 의류, IT };

[System.Serializable]
public class Expert
{
    public bool isHired;
    public string name;
    public int salary;
    public string info;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };
    public List<Expert> experts;

    public string companyName;
    private int day; //게임 시작 후 현재까지의 날짜
    public int Day { get { return day; } }
    private int money; //자본
    public int Money { get { return money; } }
    private int reputation; //평판
    public int Reputation { get { return reputation; } }
    private int satisfaction; //직원 만족도
    public int Satisfaction { get { return satisfaction; } }

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
        money = 430000;
        satisfaction = 500;
        reputation = 50;

        for (int i = 0; i < 8; i++)
        {
            Business business = new Business();
            business.Initialize(((DepartmentName)i).ToString());
            department.Add(business);
        }
        CreateNewDepartment(0);
    }

    private void Update()
    {
        //매 Update 마다 체크해야 하는 요소
        CheckGameOver();

        time += Time.deltaTime;

        if (time >= 1f)
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
            UseMoney(TotalLoss() - TotalProfit());
            day += 1; //다음 달 1일부터 시작하게 하기 위해
        }
    }

    public void UseMoney(int amount)
    {
        money -= amount;
    }

    public void ChangeSatisfaction(int amount)
    {
        satisfaction += amount;
        if (satisfaction > 1000) satisfaction = 1000;
    }

    public void ChangeReputation(int amount)
    {
        reputation += amount;
        if (reputation > 100) reputation = 100;
    }

    public void CreateNewDepartment(int num)
    {
        int enableDepartment = 0;
        foreach (Business business in department)
        {
            if (business.enabled == true) enableDepartment += 1;
        }

        money -= 100000 + (enableDepartment - 1) * 70000;
        department[num].Create();
    }

    public void StartNewDevelop(int num, int investMoney)
    {
        //신제품 개발에 필요한 금액 차감
        UseMoney(investMoney);

        //해당 부서 개발 시작
        department[num].StartDevelop(investMoney);
    }

    public int TotalProfit()
    {
        int profit = 0;

        for (int i = 0; i < department.Count; i++)
        {
            profit += department[i].profit;
        }

        return profit;
    }

    public int TotalLoss()
    {
        int loss = 0;

        for (int i = 0; i < department.Count; i++)
        {
            loss += department[i].loss;
        }

        loss += TotalExpertSalary();

        return loss;
    }

    private void CheckGameOver()
    {
        if (money <= 0 || satisfaction <= 0 || reputation <= 0)
        {
            GameOver();
        }
    }

    private int TotalExpertSalary()
    {
        int total = 0;

        for (int i = 0; i < experts.Count; i++)
        {
            if (experts[i].isHired == true) total += experts[i].salary;
        }

        return total;
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
        UseMoney(20000);
        int reputationIncreasedAmount = Random.Range(1, 6);
        StartCoroutine(UIManager.instance.ShowReputationIncrease(reputationIncreasedAmount));
        ChangeReputation(reputationIncreasedAmount);
    }

    public void Welfare()
    {
        UseMoney(20000);
        int satisfactionIncreasedAmount = Random.Range(1, 21);
        StartCoroutine(UIManager.instance.ShowSatisfactionIncrease(satisfactionIncreasedAmount));
        ChangeSatisfaction(satisfactionIncreasedAmount);
    }

    public void ChangeTimeScale(float amount)
    {
        if (Time.timeScale * amount <= 99f && Time.timeScale * amount >= 0.1f)
            Time.timeScale *= amount;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.instance.gameOverPanel.SetActive(true);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
}   
