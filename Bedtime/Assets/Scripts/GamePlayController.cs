using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayController : MonoBehaviour {

	public List<PlayerController> m_players;

    // freeze settings
    float m_freezeTime = 3.0f;

    // speed settings
    float m_speedTime = 2.0f;
    float m_speedMultiplyer = 4.0f;
    float m_speedFade = 1.0f;

    // invincible settings
    float m_invincibleTime = 5.0f;

    public GameObject m_obstacle;

    // Powerup Audio
    public AudioSource freezeSound;
    public AudioSource speedSound;
    public AudioSource invincibleSound;
    public AudioSource obstacleSound;

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
            freezeSound.Play();

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
            speedSound.Play();

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
                Debug.Log(elapsedTime + " / " + m_speedTime);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // reset all effects and such
            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            canvas.clearStatusEffectIcon();

            // fade trail and speed
            elapsedTime = 0.0f;
            while (elapsedTime < m_speedFade)
            {
                trail.time = Mathf.SmoothStep(0.5f, 0, (elapsedTime / m_speedFade));
                _targetPlayer.m_speed = Mathf.SmoothStep(_targetPlayer.defaultSpeed * m_speedMultiplyer, _targetPlayer.defaultSpeed, (elapsedTime / m_speedFade));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public IEnumerator usePowerupInvincible(PlayerController _targetPlayer)
    {
        if (_targetPlayer.canTakeStatusEffect())
        {
            invincibleSound.Play();

            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Invincible);

            MeshRenderer forceFieldMesh = _targetPlayer.forceField.GetComponent<MeshRenderer>();

            _targetPlayer.m_isInvincible = true;
            forceFieldMesh.enabled = true;

            // wind the ring down
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

            StairController[] stairs = FindObjectsOfType<StairController>();
            List<StairController> activeStairs = new List<StairController>();
            GameObject correctPreviousStairs = new GameObject();

            List<GameObject> obstacles = new List<GameObject>();

            for (int i = 0; i < stairs.Length; ++i)
            {
                if(!stairs[i].m_old)
                {
                    activeStairs.Add(stairs[i]);
                }
            }

            for (int i = 0; i < activeStairs.Count; ++i)
            {
                if(activeStairs[i].m_player == _targetPlayer)
                {
                    Debug.Log("Setting the correct stairs");
                    correctPreviousStairs = activeStairs[i].m_previous;
                }
            }


            float elapsedTime = 0.0f;
            while (elapsedTime < 10.0f)
            {
                obstacles.Add(Instantiate(m_obstacle, correctPreviousStairs.transform.position + new Vector3(Random.Range(1, 6), 10, 0), Quaternion.identity));


                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(5);

            for(int i = 0; i < obstacles.Count; ++i)
            {
                Destroy(obstacles[i]);
            }
            obstacles.Clear();
        }
    }


}
