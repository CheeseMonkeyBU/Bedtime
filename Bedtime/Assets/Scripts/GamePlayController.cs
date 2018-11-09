using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour {

	public List<PlayerController> m_players;

    // freeze settings
    public float m_freezeTime = 3.0f;

    // speed settings
    public float m_speedTime = 4.0f;
    public float m_speedMultiplyer = 4.0f;

    // invincible settings
    public float m_invincibleTime = 5.0f;

    // Use this for initialization
    void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {

    }

    public void registerPlayer(PlayerController _player)
    {
        _player.m_playerNumber = m_players.Count;
        _player.m_gpController = this;
        m_players.Add(_player);
    }

    public void applyPowerupToPlayer(int _playerID, Powerup.PowerupType _type)
    {
        if(_type == Powerup.PowerupType.Freeze)
        {
            StartCoroutine("usePowerupFreeze", m_players[_playerID]);
        }
        else if (_type == Powerup.PowerupType.Speed)
        {
            StartCoroutine("usePowerupSpeed", m_players[_playerID]);
        }
        else if(_type == Powerup.PowerupType.Invincible)
        {
            StartCoroutine("usePowerupInvincible", m_players[_playerID]);
        }
        else if (_type == Powerup.PowerupType.Obstacles)
        {
            StartCoroutine("usePowerupObstacle", m_players[_playerID]);
        }
    }

    public IEnumerator usePowerupFreeze(PlayerController _targetPlayer)
    {
        if(!_targetPlayer.m_isInvincible || !_targetPlayer.m_hasStatusEffect)
        {
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Freeze);

            _targetPlayer.m_speed = 0.0f;

            float elapsedTime = 0.0f; 
            while(elapsedTime < m_freezeTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_freezeTime));
                Debug.Log(elapsedTime);
                Debug.Log(m_freezeTime);
                Debug.Log(elapsedTime / m_freezeTime);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;

            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            _targetPlayer.m_canvas.GetComponent<CanvasController>().clearStatusEffectIcon();
        }

    }

    public IEnumerator usePowerupSpeed(PlayerController _targetPlayer)
    {
        if (!_targetPlayer.m_isInvincible || !_targetPlayer.m_hasStatusEffect)
        {
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Speed);

            _targetPlayer.m_speed *= m_speedMultiplyer;

            float elapsedTime = 0.0f;
            while (elapsedTime < m_speedTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_speedTime));

                Debug.Log(elapsedTime);
                Debug.Log(m_speedTime);
                Debug.Log(elapsedTime / m_speedTime);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;

            _targetPlayer.m_hasStatusEffect = false;
            canvas.setStatusEffectRingPerc(1.0f);
            _targetPlayer.m_canvas.GetComponent<CanvasController>().clearStatusEffectIcon();
        }

        //_targetPlayer.m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
    }

    public IEnumerator usePowerupInvincible(PlayerController _targetPlayer)
    {
        if (!_targetPlayer.m_isInvincible || !_targetPlayer.m_hasStatusEffect)
        {
            _targetPlayer.m_hasStatusEffect = true;
            CanvasController canvas = _targetPlayer.m_canvas.GetComponent<CanvasController>();
            canvas.changeStatusEffectIcon(Powerup.PowerupType.Invincible);

            _targetPlayer.m_isInvincible = true;

            float elapsedTime = 0.0f;
            while (elapsedTime < m_invincibleTime)
            {
                canvas.setStatusEffectRingPerc(1.0f - (elapsedTime / m_invincibleTime));

                Debug.Log(elapsedTime);
                Debug.Log(m_invincibleTime);
                Debug.Log(elapsedTime / m_invincibleTime);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _targetPlayer.m_isInvincible = false;

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
        }

        //_targetPlayer.m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
    }


}
