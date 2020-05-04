using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionGate : MonoBehaviour
{
    [SerializeField] private GameState m_gameState = GameState.Any;
    [SerializeField] private EmotionalState m_requiredEmotionalState = EmotionalState.None;
    [SerializeField] private EmotionalState m_turnAroundEmotion = EmotionalState.Confused;
    [SerializeField] private bool m_requireCroc = false;

    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( m_gameState != GameState.Any && CrocBoyManager.instance.State != m_gameState ) return;

        var canPass = true;
        if ( m_requireCroc && Boy.instance.IsCroc == false )
            canPass = false;
        if( m_requiredEmotionalState != EmotionalState.None && m_requiredEmotionalState != Boy.instance.EmotionalState)
            canPass = false;

        if ( canPass == false )
            Boy.instance.TurnAround( m_turnAroundEmotion );
    }
}
