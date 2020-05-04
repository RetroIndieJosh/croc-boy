using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTrigger : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private GameState m_requiredState = GameState.Any;
    [SerializeField] private GameState m_targetState = GameState.Any;
    
    [Header("Reaction")]
    [SerializeField] private EmotionalState m_targetEmotion = EmotionalState.None;
    [SerializeField, Tooltip("0 for permanent")] private float m_timeSec = 0f;

    [Header("Requirements")]
    [SerializeField] private bool m_requireCroc = false;
    [SerializeField] private EmotionalState m_requiredEmotionalState = EmotionalState.None;

    private void OnTriggerEnter2D( Collider2D collision ) {
        var boy = collision.GetComponent<Boy>();
        if ( boy == null ) return;

        if ( m_requiredState != GameState.Any && m_requiredState != CrocBoyManager.instance.State )
            return;

        if ( m_requireCroc && Boy.instance.IsCroc == false )
            return;
        if ( m_requiredEmotionalState != EmotionalState.None && m_requiredEmotionalState != Boy.instance.EmotionalState )
            return;

        boy.React( m_targetEmotion, m_timeSec );

        if ( m_targetState == GameState.Any ) {
            Debug.LogError( "Cannot change game state to 'Any'" );
            return;
        }

        CrocBoyManager.instance.State = m_targetState;
    }
}
