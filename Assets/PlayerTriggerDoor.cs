using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDoor : MonoBehaviour
{
    [SerializeField] private GameState m_requiredGameState = GameState.Any;
    [SerializeField] private Door m_targetDoor = null;
    [SerializeField] private EmotionalState m_requiredEmotionalState = EmotionalState.None;

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( transform.position, 0.5f );
        Gizmos.DrawLine( transform.position, m_targetDoor.transform.position );
        Gizmos.DrawWireSphere( m_targetDoor.transform.position, 0.5f );
    }

    private void OnMouseDown() {
        if ( m_requiredGameState != GameState.Any && m_requiredGameState != CrocBoyManager.instance.State )
            return;

        if ( m_requiredEmotionalState == EmotionalState.None || m_requiredEmotionalState == Boy.instance.EmotionalState )
            Boy.instance.TargetX = m_targetDoor.transform.position.x;
        else
            Boy.instance.RefuseSuggestion();
    }
}
