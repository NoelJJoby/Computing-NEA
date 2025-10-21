using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuUI : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_Dropdown registeringToggle;
    [SerializeField] Button submitButton;


    [SerializeField] GameObject usernameReqs;
    [SerializeField] GameObject passwordReqs;
    [SerializeField] TextMeshProUGUI usernameHailText;

    private void Awake()
    {

        usernameHailText.gameObject.SetActive(false);


        submitButton.onClick.AddListener(() =>
        {
            Error();
            if (!ValidateUsername(usernameInput.text))
            {
                usernameReqs.SetActive(true);
                return;
            }

            if (!ValidatePassword(passwordInput.text))
            {
                passwordReqs.SetActive(true);
                return;
            }

            if (registeringToggle.value == 0)
            {
                Authentication.Register(usernameInput.text, passwordInput.text);

                Correct();
            }
            else if (Authentication.Login(usernameInput.text, passwordInput.text))
            {
                Correct();

            }

        });
    }

    private void Start()
    {
        if (Authentication.CheckAuthenticated() || Authentication.CheckDebug())
        {
            usernameHailText.gameObject.SetActive(true);
            usernameHailText.text = "Hello, " + Authentication.GetCurrentUsername();
        }
    }

    private bool ValidateUsername(string name)
    {
        return name.Length >= 5;
    }

    private bool ValidatePassword(string pass)
    {
        if (pass.Length < 8)
        {
            return false;
        }
        if (pass.ToUpper() == pass)
        {
            return false;
        }
        return pass.Any(char.IsDigit) && pass.Any(char.IsPunctuation);
    }

    private void Error()
    {
        submitButton.GetComponent<Outline>().effectColor = Color.red;
        submitButton.GetComponent<Outline>().effectDistance = 5 * Vector2.one;
    }

    private void Correct()
    {
        submitButton.GetComponent<Outline>().effectColor = Color.green;
        submitButton.GetComponent<Outline>().effectDistance = 2 * Vector2.one;

        usernameHailText.gameObject.SetActive(true);
        usernameHailText.text = "Hello, " + usernameInput.text;
        gameObject.SetActive(false);
    }
}
