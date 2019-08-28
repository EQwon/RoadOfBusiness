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

    [Header("전문가 UI Holder")]
    public Text expertNameText;
    public Text expertSalaryText;
    public Text expertInfoText;
    public GameObject hireExpertButton;
    public GameObject fireExpertButton;

    [Header("이벤트 UI Holder")]
    public GameObject eventPanel;
    public Text eventNameText;
    public Text eventContentsText;

    [Header("GameOver UI Holder")]
    public GameObject gameOverPanel;

    /// <summary>
    /// 현재 살펴보고 있는 사업부의 현황판이 몇 번째 사업부인지를 저장해둡니다.
    /// </summary>
    private int nowDepartment;
    /// <summary>
    /// 개발에 얼마나 투자할지를 임시로 저장해둡니다.
    /// </summary>
    private int devInvestMoney = 9000;
    /// <summary>
    /// 현재 살펴보고 있는 전문가가 몇 번째 전문가인지를 저장.
    /// </summary>
    private int nowExpert;

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
        if (GameManager.instance.department[nowDepartment].Period != -1)
        {
            duringDev.SetActive(true);
            beforeDev.SetActive(false);
        }
        else
        {
            duringDev.SetActive(false);
            beforeDev.SetActive(true);
        }
    }

    /// <summary>
    /// 현재 사업부에 해당하는 내용으로 텍스트를 교체합니다.
    /// </summary>
    private void ShowDepartmentStatus()
    {
        departmentName.text = GameManager.instance.department[nowDepartment].name;
        monthlyProfit.text = string.Format("{0:#,### ZS}", GameManager.instance.department[nowDepartment].profit);
        monthlyLoss.text = string.Format("{0:#,### ZS}", GameManager.instance.department[nowDepartment].loss);
        monthlyNetProfit.text = string.Format("{0:#,### ZS}", GameManager.instance.department[nowDepartment].profit - GameManager.instance.department[nowDepartment].loss);

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
        devInvestMoneyText.text = string.Format("{0:#,### ZS}", devInvestMoney);

        //투자 금액에 따른 예상 기간 설정하기
        int day = devInvestMoney / 300;
        estimatedDevPeriodText.text = day / 30 + "개월 " + day % 30 + "일";
    }

    private void ShowMoney()
    {
        moneyText.text = string.Format("{0:#,### ZS}", GameManager.instance.Money);
    }

    private void ShowDate()
    {
        int day = GameManager.instance.Day;
        dateText.text = (day / 30) + "개월 " + (day % 30) + "일 째";
    }

    private void ShowRepuation()
    {
        reputationText.text = GameManager.instance.Reputation.ToString();
    }

    private void ShowSatisfaction()
    {
        satisfactionText.text = GameManager.instance.Satisfaction.ToString();
    }
    public void HireWorker(int type)
    {
        GameManager.instance.department[nowDepartment].HireWorker(type);
    }

    public void FireWorker(int type)
    {
        if (GameManager.instance.department[nowDepartment].worker[type] > 0)
        {
            GameManager.instance.department[nowDepartment].FireWorker(type);            
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
        totalLossText.text = string.Format("{0:#,### ZS}", GameManager.instance.TotalLoss());
        totalProfitText.text = string.Format("{0:#,### ZS}", GameManager.instance.TotalProfit());
        totalNetProfit.text = string.Format("{0:#,### ZS}", GameManager.instance.TotalProfit() - GameManager.instance.TotalLoss());
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

    public void DevEndEvent(string departmentName, float response, int profit, int repu)
    {
        eventPanel.SetActive(true);
        eventNameText.text = "[" + departmentName + "] 부서 신제품 개발 성공!";

        if (response <= 3.3f) eventContentsText.text = "<color=#0000a0ff>반응이 좋지 않습니다...</color>\n";
        else if (response <= 6.7f) eventContentsText.text = "괜찮은 반응입니다.\n";
        else eventContentsText.text = "<color=#FF0000>획기적입니다!</color>\n";

        eventContentsText.text += "월 이익이 <color=#FF0000>" + string.Format("{0:#,### ZS}", profit) + "</color> 만큼 증가합니다.\n 인식이 <color=#008000ff>" + repu + "</color>만큼 변화합니다.";
    }

    public void ShowExpertStat(int num)
    {
        nowExpert = num;
        Expert expert = GameManager.instance.experts[num];

        if (expert.isHired == true)
        {
            fireExpertButton.SetActive(true);
            hireExpertButton.SetActive(false);
        }
        else
        {
            hireExpertButton.SetActive(true);
            fireExpertButton.SetActive(false);
        }

        expertNameText.text = expert.name;
        expertSalaryText.text = string.Format("{0:#,### ZS}", expert.salary);
        expertInfoText.text = expert.info;
    }

    public void HireExpert()
    {
        if (GameManager.instance.experts[nowExpert].isHired == true) return;
        GameManager.instance.experts[nowExpert].isHired = true;
        HireExpertEvent();
    }

    public void FireExpert()
    {
        if (GameManager.instance.experts[nowExpert].isHired == false) return;
        GameManager.instance.experts[nowExpert].isHired = false;
        FireExpertEvent();
    }

    private void HireExpertEvent()
    {
        eventPanel.SetActive(true);

        Expert expert = GameManager.instance.experts[nowExpert];
        eventNameText.text = "[" + expert.name + "] 전문가 <color=#FF0000>고용</color>!";
        eventContentsText.text = expert.info + "\n";
        eventContentsText.text += "월 지출이 " + string.Format("{0:#,### ZS}", expert.salary) + "만큼 <color=#0000a0ff>증가</color>합니다.";
    }

    private void FireExpertEvent()
    {
        eventPanel.SetActive(true);

        Expert expert = GameManager.instance.experts[nowExpert];
        eventNameText.text = "[" + expert.name + "] 전문가 <color=#0000a0ff>해고</color>!";
        eventContentsText.text = "";
        eventContentsText.text += "월 지출이 " + string.Format("{0:#,### ZS}", expert.salary) + "만큼 <color=#FF0000>감소</color>합니다.";
    }
}
