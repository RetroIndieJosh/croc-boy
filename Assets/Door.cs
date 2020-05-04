using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string TargetLevelSceneName = "";

    public void Enter() {
        CrocBoyManager.instance.ChangeLevel( TargetLevelSceneName );
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        var boy = collision.gameObject.GetComponent<Boy>();
        if ( boy == null ) return;

        boy.TargetDoor = this;
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        var boy = collision.gameObject.GetComponent<Boy>();
        if ( boy == null ) return;

        boy.TargetDoor = null;
    }
}
