using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    KeyCode Exit = KeyCode.Escape;
    bool ButtonsEnabled;
    bool IsNotOnCreditScreen;
    public Image TitleScreenBackground;
    float TitleScreenMovementSpeed;
    float MoveScreen;
    Vector3 OriginalTitlePosition;
    Vector3 CreditTitlePosition;

    // Use this for initialization
    void Start() {
        ButtonsEnabled = true;
        IsNotOnCreditScreen = true;
        TitleScreenMovementSpeed = .5f;
        OriginalTitlePosition = new Vector3(0f, 540f, 0f);
        CreditTitlePosition = new Vector3(0f, -540f, 0f);
    }

    // Update is called once per frame
    void Update() {
        if (IsNotOnCreditScreen)
        {
            if (Input.GetKey(Exit))
            {
                Application.Quit();
            }
        }
        else
        {
            if(Input.GetKey(Exit))
            {
                StartCoroutine(MoveTitleScreenDown());
            }
        }
        
    }

    public void PlayButton()
    {
        if (ButtonsEnabled)
        {
            SceneManager.LoadScene("FirstFloor");
        }
    }

    public void CreditsButton()
    {
        if (ButtonsEnabled)
        {
            IsNotOnCreditScreen = false;
            StartCoroutine(MoveTitleScreenUp());
        }
    }

    public void ReturnFromCredits()
    {
        if (true)
        {
            
        }
        ButtonsEnabled = true;
        StartCoroutine(MoveTitleScreenDown());
    }

    IEnumerator MoveTitleScreenUp()
    {
        ButtonsEnabled = false;
        while (MoveScreen < 1)
        {
            MoveScreen += TitleScreenMovementSpeed * Time.deltaTime;
            yield return null;
            TitleScreenBackground.transform.localPosition = Vector3.Lerp(OriginalTitlePosition, CreditTitlePosition, MoveScreen);
        }
        ButtonsEnabled = true;
    }

    IEnumerator MoveTitleScreenDown()
    {
        ButtonsEnabled = false;
        while (MoveScreen < 1)
        {
            MoveScreen += TitleScreenMovementSpeed * Time.deltaTime;
            yield return null;
            TitleScreenBackground.transform.position = Vector3.Lerp(CreditTitlePosition, OriginalTitlePosition, MoveScreen);
        }
        ButtonsEnabled = true;
        IsNotOnCreditScreen = true;
    }



    public void QuitGame()
    {
        Application.Quit();
    }

}
