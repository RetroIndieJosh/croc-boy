using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeCollider : MonoBehaviour
{
    [SerializeField] private string TargetSpriteName = "";

    private void OnCollisionEnter2D( Collision2D collision ) {
        var spriteChanger = collision.gameObject.GetComponent<SpriteChanger>();
        if ( spriteChanger == null ) return;

        spriteChanger.ChangeSprite( TargetSpriteName );
    }
}
