using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocMaker : MonoBehaviour
{
    private void OnCollisionEnter2D( Collision2D collision ) {
        var boy = collision.gameObject.GetComponent<Boy>();
        if ( boy == null ) return;

        Destroy( gameObject );
        boy.MakeCroc();
        boy.StartMoving( true );
    }
}
