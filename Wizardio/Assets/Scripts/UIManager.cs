using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // Login
    public GameObject StartMenu;
    public InputField UsernameField;
    public GameObject ConnectButton;

    // Player
    public GameObject GameCanvas;
    public GameObject ScoreField;
    public GameObject Healthbar;
    public GameObject HealthValue;
    private void Awake()
    {
        GameCanvas.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exsists, destroying...");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        SetLoading(true);
        Client.instance.ConnectToServer();
        UsernameField.GetComponent<InputField>().interactable = false;
        ConnectButton.GetComponent<Button>().interactable = false;
    }

    public void ReloadScore(float _score)
    {
        ScoreField.GetComponent<Text>().text = _score.ToString();
    }

    public void SetHealthbar(float _health, float _maxHealth)
    {
        Healthbar.GetComponent<Image>().fillAmount = _health / _maxHealth;
        HealthValue.GetComponent<Text>().text = $"{_health} / {_maxHealth}";
    }

    public void ConnectedSuccessfully()
    {
        SetLoading(false);
        StartMenu.SetActive(false);
        UsernameField.interactable = false;
        GameCanvas.SetActive(true);
    }

    public void ConnectionHadErrors(string error)
    {
        SetLoading(false);
        UsernameField.GetComponent<InputField>().interactable = true;
        ConnectButton.GetComponent<Button>().interactable = true;
        UIUtils.instance.DisplayError(error);
    }

    private void SetLoading(bool flag)
    {
        UIUtils.instance.SetLoading(flag);
    }
}
