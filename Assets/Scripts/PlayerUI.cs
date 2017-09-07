using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {

    GameObject Player;
    GameObject GameManager;
    int PlayerCurrentHealth;
    int PlayerMaxHealth;
    int MaxPossibleHealth;
    float FloatCurrentHealth;
    public Text ItemPickupName;
    public Text ItemPickupText;

    public Image[] HealthImages;
    public Sprite[] HealthSprites;

    public Image PauseMenu;

    public Image BossFightScreen;

    public Text BossNameText;

    public Image BossImage;

    public Image BossHealthBar;

    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        MaxPossibleHealth = 12;
        StartCoroutine(WaitOneFrame());
        DisableThePauseMenu();
        HideTheBossHealthBar();
        HideBossFightScreen();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerCurrentHealth = Player.GetComponent<Player>().CurrentHealth;
        PlayerMaxHealth = Player.GetComponent<Player>().MaxHealth;
        MaxHealthAmount();
        FloatCurrentHealth = PlayerCurrentHealth;
        UpdateRedHearts();

    }

    //Displays the correct number of empty hearts
    void MaxHealthAmount()
    {
        for(int i = 0; i < MaxPossibleHealth; i++)
        {
            if (PlayerMaxHealth / 2 <= i)
            {
                HealthImages[i].enabled = false;
            }
            else
            {
                HealthImages[i].enabled = true;
            }
        }
    }

    public void UpdateRedHearts()
    {
        //Set HP to redhearts depending on the value of PlayerCurrentHealth, and if there's a remainder make the last visable empty heart a half heart
        for (int i = 0; i < PlayerMaxHealth / 2; i++)
        {
            if (PlayerCurrentHealth / 2 > i)
            {
                HealthImages[i].sprite = HealthSprites[HealthSprites.Length - 1];
            }
            else if (FloatCurrentHealth / 2 - i == 0.5)
            {
                HealthImages[i].sprite = HealthSprites[HealthSprites.Length - 2];
            }
            else
            {
                HealthImages[i].sprite = HealthSprites[0];
            }
        }
    }

    //It's handled like this because a coroutine will stop if the original item is deleted, so this ensures it runs as long as it needs too
    public void CallItemTextCoroutine(string NameText, string ItemText)
    {
        StartCoroutine(ShowItemTextOnScreen(NameText, ItemText));
    }

    public IEnumerator ShowItemTextOnScreen(string NameText, string ItemText)
    {
        ItemPickupName.text = NameText;
        ItemPickupText.text = ItemText;
        yield return new WaitForSeconds(2f);
        ItemPickupName.text = "";
        ItemPickupText.text = "";
    }
    //Wait one frame for redhearts to update
    IEnumerator WaitOneFrame()
    {
        yield return null;
        UpdateRedHearts();
    }

    //------------------BOSS HANDLING--------------------

    //Display the boss healthbar
    public void ShowTheBossHealthBar()
    {
        BossHealthBar.transform.parent.gameObject.SetActive(true);
        BossHealthBar.enabled = true;
    }

    //Hides the boss healthbar
    public void HideTheBossHealthBar()
    {
        BossHealthBar.transform.parent.gameObject.SetActive(false);
        BossHealthBar.enabled = false;
    }

    //Controls the boss losing health
    public void BossHealth(float HealthBarFill)
    {
        //Change the current fill amount by a constant that is linear with the boss dying
        BossHealthBar.fillAmount = BossHealthBar.fillAmount - (Player.GetComponent<Player>().BulletDamage / HealthBarFill);
    }

    public void BossFightScreenCoroutine(Sprite BossSprite, string BossName)
    {
        StartCoroutine(RunBossFightScreenCoroutine(BossSprite, BossName));
    }
    //Show the boss screen and then hide it
    IEnumerator RunBossFightScreenCoroutine(Sprite BossSprite, string BossName)
    {
        ShowBossFightScreen(BossSprite, BossName);
        yield return new WaitForSeconds(4f);
        HideBossFightScreen();
    }

    //Hides the screen before the fight
    private void HideBossFightScreen()
    {
        BossFightScreen.enabled = false;
        Text[] BossFightScreenText = BossFightScreen.GetComponentsInChildren<Text>();
        for (int i = 0; i < BossFightScreenText.Length; i++)
        {
            BossFightScreenText[i].enabled = false;
        }
        BossImage.enabled = false;
    }

    //Shows the screen before the fight and adds the sprite and the name of the boss
    private void ShowBossFightScreen(Sprite BossSprite, string BossName)
    {
        BossFightScreen.enabled = true;
        Text[] BossFightScreenText = BossFightScreen.GetComponentsInChildren<Text>();
        for (int i = 0; i < BossFightScreenText.Length; i++)
        {
            BossFightScreenText[i].enabled = true;
        }
        BossNameText.text = BossName;
        BossImage.enabled = true;
        BossImage.GetComponent<Image>().sprite = BossSprite;
    }
    //------------PauseMenu----------------
    //Pause and unpause the game
    public void DisableThePauseMenu()
    {
        Time.timeScale = 1;
        Player.GetComponent<Player>().PlayerCanShoot = true;
        PauseMenu.enabled = false;
        Text[] PauseText = PauseMenu.GetComponentsInChildren<Text>(true);
        Button[] PauseButton = PauseMenu.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < PauseText.Length; i++)
        {
            PauseText[i].enabled = false;
        }
        for (int i = 0; i < PauseButton.Length; i++)
        {
            PauseButton[i].gameObject.SetActive(false);
        }
    }

    public void EnableThePauseMenu()
    {
        Time.timeScale = 0;
        Player.GetComponent<Player>().PlayerCanShoot = false;
        PauseMenu.enabled = true;
        Text[] PauseText = PauseMenu.GetComponentsInChildren<Text>(true);
        Button[] PauseButton = PauseMenu.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < PauseText.Length; i++)
        {
            PauseText[i].enabled = true;
        }
        for (int i = 0; i < PauseButton.Length; i++)
        {
            PauseButton[i].gameObject.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        Destroy(GameManager);
        Destroy(Player);
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
