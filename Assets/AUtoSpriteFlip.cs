using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpriteFlip : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer = null;
    private Rigidbody2D m_body = null;

    private void Awake() {
        m_body = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if ( m_body.velocity.x > Mathf.Epsilon )
            m_spriteRenderer.flipX = false;
        else if ( m_body.velocity.x < -Mathf.Epsilon )
            m_spriteRenderer.flipX = true;
    }
}
