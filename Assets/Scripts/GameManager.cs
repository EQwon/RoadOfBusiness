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
    public Sprite face;
    public int salary;
    public string info;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Business> department = new List<Business> { };
    public List<Expert> experts;
    public List<string> news = new List<string> { };

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
    private List<string> simpleNews = new List<string> {
        "신흥 종교 퓨디파이교 유행", "IRA가 다시 활동 시작", "스코틀랜드 독립",
        "브렉시트 투표 가결", "도쿄 올림픽 연기", "대한민국 통일",
        "다음 올림픽 개최지가 화성으로 확정\n-IOC","게임 캐릭터의 인권 주장",
        "나무위키 폐쇄","미국 마피아 완전히 소멸","프랑스에서 youtube 접근 금지",
        "3차 세계대전 1일만에 종전","1조ZS가 새로운 세계 최대 게임회사로 등장",
        "RAM 1TB 시대 도달","새로운 스마트폰에서 치명적인 결함 발견",
        "사실 교환 법칙은 성립하지 않아...\n-수학자 나유명",
        "새로운 웃음으로 동작하는 발전소 등장\n-건설사는 근처에서 석유통이 발견된 점을 부인",
        "16K TV등장", "분당 90000타를 쳐주는 기계 등장\n-키배의 전환점",
        "대통령이 자신의 오버워치 모스트 픽이 한조임을 부인함","블랙홀 여행 상품 할인\n-1zs",
        "fjw-!fl 소tlr에 rkx혀있음! 살fu주세요?//fjwo 00", "제2의 장미칼 등장",
        "만우절에 모든 범죄자가 풀려나\n-경찰총장 사임", "타임머신 발명",
        "태양 소멸로 인해 전 세계 평균온도 –150가 됨\n-기상캐스터가 깜작 놀라",
        "세계에서 가장 많이 팔린 게임\n-장사의 길",
        "전기 공급 중단으로 석기 시대가 되어\n-뉴욕 주 맨해튼 시"
    };

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

        news.Add("<color=#FF0000>[속보]</color>\n" + companyName + " 회사 설립!!");
        news.Add("");
        news.Add("");
        news.Add("");
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

        if (Day % 30 == 0) //매달 초에 하는 일
        {
            UseMoney(TotalLoss() - TotalProfit());
            ChangeByExpert();
            AddSimpleNews();
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

        if (money >= 2000000000)
        {
            Clear();
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

    private void ChangeByExpert()
    {
        if (experts[1].isHired == true) ChangeReputation(-25);
        if (experts[2].isHired == true) ChangeSatisfaction(30);
        if (experts[4].isHired == true) ChangeReputation(10);
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

    private void AddSimpleNews()
    {
        for (int i = 2; i >= 0; i--)
        {
            if (news.Count <= i) continue;

            news[i + 1] = news[i];
        }

        news[0] = simpleNews[Random.Range(0, simpleNews.Count)];
    }

    public void AddFlashNews(string flashNews)
    {
        for (int i = 2; i >= 0; i--)
        {
            if (news.Count <= i) continue;

            news[i + 1] = news[i];
        }

        news[0] = "<color=#FF0000>[속보]</color>\n" + flashNews;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.instance.gameOverPanel.SetActive(true);
    }

    private void Clear()
    {
        Time.timeScale = 0f;
        UIManager.instance.clearPanel.SetActive(true);
        UIManager.instance.record.text = "기록 : " + (day / 30) + "개월 " + (day % 30) + "일";
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
}   
