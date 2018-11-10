using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayController : MonoBehaviour {

	public List<PlayerController> m_players;

    // freeze settings
    public float m_freezeTime = 3.0f;

    // speed settings
    public float m_speedTime = 4.0f;
    public float m_speedMultiplyer = 4.0f;

    // invincible settings
    public float m_invincibleTime = 5.0f;

    public GameObject m_obstacle;

    // Use this for initialization
    void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        if (players.Length == 0)
        {
            RectTransform panel = FindObjectOfType<UIController>().getWinPanel();
            panel.GetComponentInChildren<Image>().enabled = true;
            panel.GetComponentInChildren<Image>().color = new Color(0, 0, 0, 1);
            panel.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            panel.GetComponentInChildren<TextMeshProUGUI>().text = "Darkness envelops you";
            StartCoroutine(waitForMenu());
        }
    }

    public void registerPlayer(PlayerController _player)
    {
        _player.m_playerNumber = m_players.Count;
        _player.m_gpController = this;
        m_players.Add(_player);
    }

    public bool applyPowerupToPlayer(int _playerID, Powerup.PowerupType _type)
    {
        if (!m_players[_playerID].canTakeStatusEffect())
        {
            Debug.Log("Failed to apply pickup, player cannot accept");
            return false;
        }
        if (_playerID >= m_players.Count)
        {
            Debug.Log("Failed to apply pickup, player id out of range");
            return false;
        }

        if(_type == Powerup.PowerupType.Freeze)
        {
            Debug.Log("Starting Freeze");
            StartCoroutine("usePowerupFreeze", m_players[_playerID]);
        }
        else if (_type == Powerup.PowerupType.Speed)
        {
            Debug.Log("Starting Speed");
            StartCoroutine("usePowerupSpeed", m_players[_playerID]);
        }
        else if(_type == Powerup.PowerupType.Invincible)
        {
            Debug.Log("Starting invincible");
            StartCoroutine("usePowerupInvincible", m_players[_playerID]);
        }
        else if (_type == Powerup.PowerupType.Obstacles)
        {
            StartCoroutine("usePowerupObstacle", m_players[_playerID]);
        }

        return true;
    }

    private IEnumerator waitForMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    public IEnumerator usePowerupFreeze(PlayerController _targetPlayer)
    {
        if(_targetPlayer.canTakeStatusEffect())
        {
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Freeze);

            // turn on particles
            ParticleSystem freezeParticles = _targetPlayer.iceBlock.GetComponent<ParticleSystem>();
            MeshRenderer iceBlockMesh = _targetPlayer.iceBlock.GetComponent<MeshRenderer>();
            ParticleSystem.EmissionModule em = freezeParticles.emission;
            em.rateOverTime = 20.0f;
            iceBlockMesh.enabled = true;

            _targetPlayer.m_speed = 0.0f;

            float elapsedTime = 0.0f; 
            while(elapsedTime < m_freezeTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_freezeTime));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            em.rateOverTime = 0.0f;
            iceBlockMesh.enabled = false;

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;

            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            _targetPlayer.m_canvas.GetComponent<CanvasController>().clearStatusEffectIcon();
        }

    }

    public IEnumerator usePowerupSpeed(PlayerController _targetPlayer)
    {
        if (_targetPlayer.canTakeStatusEffect())
        {
            // set the status effect on the canvas
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Speed);

            _targetPlayer.m_speed *= m_speedMultiplyer;

            TrailRenderer trail = _targetPlayer.gameObject.GetComponent<TrailRenderer>();

            trail.time = 0.5f;

            float elapsedTime = 0.0f;
            while (elapsedTime < m_speedTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_speedTime));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;
            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            canvas.clearStatusEffectIcon();

            // fade trail
            elapsedTime = 0.0f;
            while (elapsedTime < 1.0f)
            {
                trail.time = Mathf.SmoothStep(0.5f, 0, (elapsedTime / 1.0f));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }


        }

        //_targetPlayer.m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
    }

    public IEnumerator usePowerupInvincible(PlayerController _targetPlayer)
    {
        if (_targetPlayer.canTakeStatusEffect())
        {
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Invincible);

            MeshRenderer forceFieldMesh = _targetPlayer.forceField.GetComponent<MeshRenderer>();

            _targetPlayer.m_isInvincible = true;
            forceFieldMesh.enabled = true;

            float elapsedTime = 0.0f;
            while (elapsedTime < m_invincibleTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_invincibleTime));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _targetPlayer.m_isInvincible = false;
            forceFieldMesh.enabled = false;

            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            _targetPlayer.m_canvas.GetComponent<CanvasController>().clearStatusEffectIcon();
        }

        //_targetPlayer.m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
    }

    public IEnumerator usePowerupObstacle(PlayerController _targetPlayer)
    {
        if (!_targetPlayer.m_isInvincible)
        {
            yield return new WaitForSeconds(1);

            if (_targetPlayer.m_recentStairController != null)
            {
                GameObject o = Instantiate(m_obstacle);
                o.transform.position = _targetPlayer.m_recentStairController.GetComponent("EndOfStairs").transform.position;
            }
        }


        //_targetPlayer.m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
    }


}
