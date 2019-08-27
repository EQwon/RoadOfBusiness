using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("기본 UI Holder")]
    public Text companyNameText;
    public Text dateText;
    public Text moneyText;
    public Text satisfactionText;
    public Text reputationText;

    [Header("사업부 현황판 UI Holder")]
    public Text departmentName;
    public Text monthlyProfit;
    public Text monthlyLoss;
    public Text monthlyNetProfit;
    public List<Text> workerAmountText;
    public Text remainPeriodText;
    public Text devInvestMoneyText;
    public Text estimatedDevPeriodText;
    public GameObject beforeDev;
    public GameObject duringDev;

    [Header("회사 현황판 UI Holder")]
    public Text companyName;
    public Text totalLossText;
    public Text totalProfitText;
    public Text totalNetProfit;
    public Text totalWorkers;

    [Header("식단표 UI Holder")]
    public GameObject reputationIncreasedText;
    public GameObject satisfactionIncreasedText;

    /// <summary>
    /// 현재 살펴보고 있는 사업부의 현황판이 몇 번째 사업부인지를 저장해둡니다.
    /// </summary>
    private int nowDepartment;
    /// <summary>
    /// 개발에 얼마나 투자할지를 임시로 저장해둡니다.
    /// </summary>
    private int devInvestMoney = 9000;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        companyNameText.text = GameManager.instance.companyName;
    }

    private void Update()
    {
        //기본 정보
        ShowMoney();
        ShowDate();
        ShowRepuation();
        ShowSatisfaction();

        //사업부 현황판 정보
        ShowDepartmentStatus();

        //회사 전체 정보
        ShowCompanyStatus();
    }

    public void WhichDepartment(int num)
    {
        nowDepartment = num;
    }

    /// <summary>
    /// 현재 사업부에 해당하는 내용으로 텍스트를 교체합니다.
    /// </summary>
    private void ShowDepartmentStatus()
    {
        departmentName.text = GameManager.instance.department[nowDepartment].name;
        monthlyProfit.text = GameManager.instance.department[nowDepartment].profit.ToString() + "ZS";
        monthlyLoss.text = GameManager.instance.department[nowDepartment].loss.ToString() + "ZS";
        monthlyNetProfit.text = (GameManager.instance.department[nowDepartment].profit - GameManager.instance.department[nowDepartment].loss).ToString() + "ZS";

        //직원 수 불러오기
        for (int i = 0; i < 3; i++)
        {
            workerAmountText[i].text = GameManager.instance.department[nowDepartment].worker[i].ToString() + " 명";
        }

        //남은 개발 기간 표시
        if (GameManager.instance.department[nowDepartment].Period != -1)
        {
            int remainPeriod = GameManager.instance.department[nowDepartment].Period;
            remainPeriodText.text = remainPeriod / 30 + "개월 " + remainPeriod % 30 + "일";
        }
        else remainPeriodText.text = "";

        //개발 시작 전, 투자 금액 설정하기
        devInvestMoneyText.text = devInvestMoney.ToString() + "ZS";

        //투자 금액에 따른 예상 기간 설정하기
        int day = devInvestMoney / 300;
        estimatedDevPeriodText.text = day / 30 + "개월 " + day % 30 + "일";
    }

    private void ShowMoney()
    {
        moneyText.text = GameManager.instance.Money.ToString() + "ZS";
    }

    private void ShowDate()
    {
        int day = GameManager.instance.Day;
        dateText.text = (day / 30) + "개월 " + (day % 30) + "일 째";
    }

    private void ShowRepuation()
    {
        reputationText.text = GameManager.instance.repuation.ToString();
    }

    private void ShowSatisfaction()
    {
        satisfactionText.text = GameManager.instance.Satisfaction.ToString("#.0");
    }
    public void HireWorker(int type)
    {
        GameManager.instance.department[nowDepartment].worker[type] += 1;
    }

    public void FireWorker(int type)
    {
        if (GameManager.instance.department[nowDepartment].worker[type] > 0)
        {
            GameManager.instance.department[nowDepartment].worker[type] -= 1;
            GameManager.instance.ChangeSatisfaction(-0.3f);
        }
    }

    public void ChangeDevInvestMoney(int amount)
    {
        if(devInvestMoney + amount >= 0)
            devInvestMoney += amount;
    }

    public void StartDevelop()
    {
        //신제품 개발에 착수
        GameManager.instance.StartNewDevelop(nowDepartment,devInvestMoney);
    }

    public void EndDevelop()
    {
        duringDev.SetActive(false);
        beforeDev.SetActive(true);
    }

    public void ShowCompanyStatus()
    {
        companyName.text = GameManager.instance.companyName;
        totalLossText.text = GameManager.instance.TotalLoss() + "ZS";
        totalProfitText.text = GameManager.instance.TotalProfit() + "ZS";
        totalNetProfit.text = (GameManager.instance.TotalProfit() - GameManager.instance.TotalLoss()) + "ZS";
        totalWorkers.text = GameManager.instance.TotalWorkerAmount() + "명";
    }

    public IEnumerator ShowReputationIncrease(int amount)
    {
        reputationIncreasedText.GetComponent<Text>().text = "인식 +" + amount;
        reputationIncreasedText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        reputationIncreasedText.SetActive(false);
    }

    public IEnumerator ShowSatisfactionIncrease(int amount)
    {
        satisfactionIncreasedText.GetComponent<Text>().text = "직원만족도 +" + amount;
        satisfactionIncreasedText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        satisfactionIncreasedText.SetActive(false);
    }
}
