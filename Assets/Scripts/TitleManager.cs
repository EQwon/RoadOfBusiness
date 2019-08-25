using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject coin;

    [Header("UI Holder")]
    public GameObject nameInput;
    public GameObject startPanel;
    public Text companyNameText;

    public List<GameObject> coins;

    private void Start()
    {
        Time.timeScale = 1;
        nameInput.SetActive(true);
        startPanel.SetActive(false);

        StartCoroutine(CreateCoin());
    }

    private IEnumerator CreateCoin()
    {
        float deltaTime = Random.Range(0.001f, 0.051f);
        Vector2 spawnPos = new Vector2(Random.Range(-10f, 10f), 6f);
        Quaternion quaternion = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        coins.Add(Instantiate(coin, spawnPos, quaternion, transform));

        yield return new WaitForSeconds(deltaTime);

        if (coins.Count >= 500)
        {
            Destroy(coins[0]);
            coins.RemoveAt(0);
        }
        StartCoroutine(CreateCoin());
    }

    public void SetCompanyName(string name)
    {
        Debug.Log("회사 이름은 " + name + "입니다.");
        PlayerPrefs.SetString("CompanyName", name);

        nameInput.SetActive(false);
        startPanel.SetActive(true);
        companyNameText.text = "회사 이름 : " + name;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

