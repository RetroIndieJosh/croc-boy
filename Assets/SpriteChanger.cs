using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> m_spriteList = new List<Sprite>();

    SpriteRenderer m_spriteRenderer = null;

    private void Awake() {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(string a_name ) {
        var name = a_name.ToLower();
        foreach( var sprite in m_spriteList) {
            if ( sprite.name.ToLower() == name ) {
                m_spriteRenderer.sprite = sprite;
                return;
            }
        }

        Debug.LogWarning( $"No sprite by name {a_name} in {name}" );
    }
}
