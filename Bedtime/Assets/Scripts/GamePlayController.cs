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
    void Start () {
		//for (int i = 0; i < m_players.Count; i++) {
		//	if (m_players [i] != null) {
		//		m_players [i].m_playerNumber = i + 1;
		//		m_players [i].m_gpController = this;
		//	}
		//}
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
    }

    public IEnumerator usePowerupFreeze(PlayerController _targetPlayer)
    {
        if(!_targetPlayer.m_isInvincible)
        {
            _targetPlayer.m_speed = 0.0f;

            yield return new WaitForSeconds(m_freezeTime);

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;
        }
    }

    public IEnumerator usePowerupSpeed(PlayerController _targetPlayer)
    {
        if (!_targetPlayer.m_isInvincible)
        {
            _targetPlayer.m_speed *= m_speedMultiplyer;

            yield return new WaitForSeconds(m_speedTime);

            _targetPlayer.m_speed = _targetPlayer.defaultSpeed;
        }
    }

    public IEnumerator usePowerupInvincible(PlayerController _targetPlayer)
    {
        if (!_targetPlayer.m_isInvincible)
        {
            _targetPlayer.m_isInvincible = true;

            yield return new WaitForSeconds(m_invincibleTime);

            _targetPlayer.m_isInvincible = false;
        }
    }


}
