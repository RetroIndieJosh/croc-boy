using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameState : MonoBehaviour
{
    /*
    [SerializeField] private GameState m_requiredState = GameState.Any;
    [SerializeField] private GameState m_targetState = GameState.Any;
    [SerializeField] private EmotionalState m_requiredEmotionalState = EmotionalState.None;
    [SerializeField] private bool m_requireCroc = false;

    private void OnTriggerEnter2D( Collider2D collision ) {
        var boy = collision.GetComponent<Boy>();
        if ( boy == null ) return;

        if ( m_requiredState != GameState.Any && m_requiredState != CrocBoyManager.instance.State )
            return;

        if ( m_requireCroc && Boy.instance.IsCroc == false )
            return;
        if ( m_requiredEmotionalState != EmotionalState.None && m_requiredEmotionalState != Boy.instance.EmotionalState )
            return;

        if ( m_targetState == GameState.Any ) {
            Debug.LogError( "Cannot change game state to 'Any'" );
            return;
        }

        CrocBoyManager.instance.State = m_targetState;
    }
    */
}

