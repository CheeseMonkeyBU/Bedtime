using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {

    public GameObject camera;
    public GameObject player;

    [SerializeField]
    GameObject m_button;
    [SerializeField]
    GameObject m_powerup;
    [SerializeField]
    GameObject m_statusEffect;
    [SerializeField]
    GameObject m_statusEffectRing;
    [SerializeField]
    GameObject m_battery;

    [SerializeField]
    Sprite m_spriteFreeze;
    [SerializeField]
    Sprite m_spriteInvincible;
    [SerializeField]
    Sprite m_spriteSpeed;
    [SerializeField]
    Sprite m_spriteObstacle;

    bool m_isInitialised = false;

    TextMeshProUGUI m_batteryText;
    PlayerController m_playerController;

    Image m_powerupImage;
    Image m_statusEffectImage;

    public void initialise()
    {
        if(camera == null || player == null)
        {
            Debug.LogError("Unable to init canvas, no player or camera set");
            return;
        }

        Button button = m_button.GetComponent<Button>();
        TextMeshProUGUI buttonText = m_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ColorBlock colourBlock = button.colors;

        m_batteryText = m_battery.GetComponent<TextMeshProUGUI>();

        m_playerController = player.GetComponent<PlayerController>();

        m_powerupImage = m_powerup.GetComponent<Image>();
        m_statusEffectImage = m_statusEffect.GetComponent<Image>();

        // set button depending on player
        if (m_playerController.m_playerNumber == 0)
        {
            buttonText.text = "A";
            // default is green
        }
        else if (m_playerController.m_playerNumber == 1)
        {
            buttonText.text = "B";
            colourBlock.normalColor = new Color(0.96f, 0.36f, 0.23f, 0.51f);
        }
        else if (m_playerController.m_playerNumber == 2)
        {
            buttonText.text = "X";
            colourBlock.normalColor = new Color(0.23f, 0.47f, 20.96f, 0.51f);
        }
        else if (m_playerController.m_playerNumber == 3)
        {
            buttonText.text = "Y";
            colourBlock.normalColor = new Color(0.93f, 0.76f, 0.19f, 0.51f);
        }

        button.colors = colourBlock;

        m_isInitialised = true;
    }

    public void changePowerupIcon(Powerup.PowerupType _type)
    {
        m_powerupImage.color = new Color(1, 1, 1, 1);
        if (_type == Powerup.PowerupType.Freeze)
        {
            m_powerupImage.sprite = m_spriteFreeze;
        }
        else if(_type == Powerup.PowerupType.Invincible)
        {
            m_powerupImage.sprite = m_spriteInvincible;
        }
        else if (_type == Powerup.PowerupType.Speed)
        {
            m_powerupImage.sprite = m_spriteSpeed;
        }
        else if (_type == Powerup.PowerupType.Obstacles)
        {
            m_powerupImage.sprite = m_spriteObstacle;
        }
    }

    public void changeStatusEffectIcon(Powerup.PowerupType _type)
    {
        m_statusEffectImage.color = new Color(1, 1, 1, 1);
        if (_type == Powerup.PowerupType.Freeze)
        {
            m_statusEffectImage.sprite = m_spriteFreeze;
        }
        else if (_type == Powerup.PowerupType.Invincible)
        {
            m_statusEffectImage.sprite = m_spriteInvincible;
        }
        else if (_type == Powerup.PowerupType.Speed)
        {
            m_statusEffectImage.sprite = m_spriteSpeed;
        }
        else if (_type == Powerup.PowerupType.Obstacles)
        {
            m_statusEffectImage.sprite = m_spriteObstacle;
        }
    }

    public void clearPowerupIcon()
    {
        m_powerupImage.color = new Color(1, 1, 1, 0);
    }

    public void clearStatusEffectIcon()
    {
        m_statusEffectImage.color = new Color(1, 1, 1, 0);
    }

    public void setStatusEffectRingPerc(float _perc)
    {
        m_statusEffectRing.GetComponent<Image>().fillAmount = _perc;
    }

    void Update()
    {
        if(!m_isInitialised)
        {
            return;
        }

        m_batteryText.text = (int)((m_playerController.m_battery / m_playerController.m_maxBattery) * 100) + "%";
    }
}
