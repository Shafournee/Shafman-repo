using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    KeyCode Exit = KeyCode.Escape;
    public Image TitleScreenBackground;
    float TitleScreenMovementSpeed;
    float MoveScreen;
    //Tells you when buttons can be pressed and when escape does what
    bool CanPressButtons;
    bool IsOnCredits;
    bool ScreenIsNotScrolling;
    Vector3 OriginalTitlePosition;
    Vector3 CreditTitlePosition;

    // Use this for initialization
    void Start() {
        CanPressButtons = true;
        IsOnCredits = false;
        ScreenIsNotScrolling = false;
        TitleScreenMovementSpeed = .5f;
        OriginalTitlePosition = new Vector3(0f, 540f, 0f);
        CreditTitlePosition = new Vector3(0f, -540f, 0f);
    }

    // Update is called once per frame
    void Update() {
        //While the screen is scrolling escape does nothing, while on credits it takes you back to main menu and while on menu it quits the game
        if(ScreenIsNotScrolling)
        {
            if (IsOnCredits)
            {
                if (Input.GetKeyDown(Exit))
                {
                    ReturnFromCredits();
                }
            }
            else
            {
                if (Input.GetKeyDown(Exit))
                {
                    Application.Quit();
                }
            }
        }
    }

    public void PlayButton()
    {
        if(CanPressButtons)
        {
            SceneManager.LoadScene("FirstFloor");
        }
    }

    public void CreditsButton()
    {
        if(CanPressButtons)
        {
            CanPressButtons = false;
            ScreenIsNotScrolling = false;
            StartCoroutine(MoveTitleScreenUp());
        }
    }

    public void ReturnFromCredits()
    {
        if(CanPressButtons)
        {
            CanPressButtons = false;
            ScreenIsNotScrolling = false;
            StartCoroutine(MoveTitleScreenDown());
        }
    }

    IEnumerator MoveTitleScreenUp()
    {
        while (MoveScreen < 1)
        {
            MoveScreen += TitleScreenMovementSpeed * Time.deltaTime;
            yield return null;
            TitleScreenBackground.transform.localPosition = Vector3.Lerp(OriginalTitlePosition, CreditTitlePosition, MoveScreen);
        }
        MoveScreen = 0;
        CanPressButtons = true;
        ScreenIsNotScrolling = true;
        IsOnCredits = true;
    }

    IEnumerator MoveTitleScreenDown()
    {
        while (MoveScreen < 1)
        {
            MoveScreen += TitleScreenMovementSpeed * Time.deltaTime;
            yield return null;
            TitleScreenBackground.transform.localPosition = Vector3.Lerp(CreditTitlePosition, OriginalTitlePosition, MoveScreen);
        }
        MoveScreen = 0;
        CanPressButtons = true;
        ScreenIsNotScrolling = true;
        IsOnCredits = false;
    }



    public void QuitGame()
    {
        if(CanPressButtons)
        {
            Application.Quit();
        }
    }

}
