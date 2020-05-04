using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EmotionalState
{
    None,
    Angry,
    Confused,
    Dizzy,
    Fallen,
    Happy,
    Sad,
    Scared
}

public class Boy : MonoBehaviour
{
    static public Boy instance = null;

    [SerializeField] private EmotionalState m_initialEmotionalState = EmotionalState.Happy;
    [SerializeField] private float m_atTargetBuffer = 0.5f;
    [SerializeField] private TextMeshProUGUI m_endingText = null;

    [SerializeField] private float m_cameraMoveDistance = 5.0f;
    [SerializeField] private float m_moveSpeed = 3f;
    [SerializeField] private float m_itemThrowSpeed = 5f;

    [Header( "Arrows" )]
    [SerializeField] private GameObject m_playerArrow = null;
    [SerializeField] private GameObject m_boyArrow = null;

    public EmotionalState EmotionalState {
        get { return m_emotionalState; }
        set {
            if ( value == m_emotionalState ) return;

            m_prevEmotionalState = m_emotionalState;
            m_emotionalState = value;

            var stateSpriteKey = m_emotionalState.ToString();
            if ( m_isCroc ) stateSpriteKey += " Croc";
            m_spriteChanger.ChangeSprite( stateSpriteKey );
        }
    }

    public bool FacingRight {
        get { return !m_spriteRenderer.flipX; }
    }

    public bool IsCroc {  get { return m_isCroc; } }

    public float TargetX {
        private get {
            return m_targetX;
        }
        set {
            m_targetX = value;
            m_hasTarget = true;

            if ( TargetX < transform.position.x )
                StartCoroutine( ShowBoyArrow( false, EmotionalState.Happy ) );
            else if ( TargetX > transform.position.x )
                StartCoroutine( ShowBoyArrow( true, EmotionalState.Happy ) );
        }
    }

    public Door TargetDoor { private get; set; }

    private EmotionalState m_emotionalState = EmotionalState.None;

    private Rigidbody2D m_body = null;
    private SpriteChanger m_spriteChanger = null;
    private SpriteRenderer m_spriteRenderer = null;

    private bool m_isCroc = false;
    private bool m_isReacting = false;

    private EmotionalState m_prevEmotionalState = EmotionalState.None;

    private bool m_hasTarget = false;
    private float m_targetX = 0f;

    public void MakeCroc() {
        m_isCroc = true;
        var prevState = m_emotionalState;
        EmotionalState = EmotionalState.None;
        EmotionalState = prevState;
    }

    public void React(EmotionalState a_emotionalState, float a_timeSec ) {
        m_body.velocity = Vector2.zero;
        if( a_emotionalState != EmotionalState.None )
            EmotionalState = a_emotionalState;
        m_isReacting = true;
        if ( a_timeSec < Mathf.Epsilon ) return;

        StartCoroutine( EndReactAfter( a_timeSec ) );
    }

    private IEnumerator EndReactAfter(float a_timeSec ) {
        yield return new WaitForSeconds( a_timeSec );
        m_isReacting = false;
    } 

    public void RefuseSuggestion() {
        StartCoroutine( ShowBoyArrow( FacingRight ) );
    }

    public void StartMoving( bool a_right ) {
        if( a_right ) m_body.velocity = Vector2.right * m_moveSpeed;
        else m_body.velocity = Vector2.left * m_moveSpeed;
    }

    public void TurnAround( EmotionalState m_emotionalState ) {
        StartCoroutine( TurnAroundCoroutine( m_emotionalState ) );
    }

    private IEnumerator TurnAroundCoroutine( EmotionalState m_emotionalState ) {
        StartMoving( !FacingRight );
        yield return null;
        StartCoroutine( ShowBoyArrow( FacingRight, m_emotionalState ) );
    }

    private IEnumerator ShowEnding() {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds( 0.5f );
        CrocBoyManager.instance.FadeOut( 1.5f );
        yield return new WaitForSeconds( 1.5f );
        m_endingText.enabled = true;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        Debug.Log( $"Boy collided with {collision.collider.name}" );

        var bully = collision.gameObject.GetComponent<Bully>();
        if ( bully == null ) return;

        if( m_isCroc) {
            bully.GetComponent<Rigidbody2D>().velocity = Vector2.left * 3f;
            bully.GetComponent<SpriteChanger>().ChangeSprite( "Knocked Out" );
            bully.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            m_body.velocity = Vector2.zero;
            m_body.constraints = RigidbodyConstraints2D.FreezeAll;
            m_emotionalState = EmotionalState.Angry;

            StartCoroutine( ShowEnding() );

            return;
        }

        StartCoroutine( LoseLunchMoney(bully) );
    }

    private void Awake() {
        instance = this;

        m_body = GetComponent<Rigidbody2D>();
        m_spriteChanger = GetComponent<SpriteChanger>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        m_endingText.enabled = false;

        var cameraPos = Camera.main.transform.position;
        cameraPos.x = transform.position.x;
        Camera.main.transform.position = cameraPos;

        m_playerArrow.SetActive( false );
        m_boyArrow.SetActive( false );

        EmotionalState = m_initialEmotionalState;
    }

    public Vector2 MouseWorldPos {
        get { return Camera.main.ScreenToWorldPoint( Input.mousePosition ); }
    }

    private void Update() {

        // update camera
        var camDistanceDiffX = transform.position.x - Camera.main.transform.position.x;
        if ( camDistanceDiffX < -m_cameraMoveDistance )
            Camera.main.transform.Translate( Vector3.left * Time.deltaTime * m_moveSpeed );
        if ( camDistanceDiffX > m_cameraMoveDistance )
            Camera.main.transform.Translate( Vector3.right * Time.deltaTime * m_moveSpeed );

        if ( m_hasTarget == false && m_isReacting == false && m_body.velocity.magnitude < float.Epsilon )
            //m_body.velocity = Vector2.left * m_moveSpeed;
            TurnAround( m_emotionalState );

        if ( m_hasTarget && m_isReacting == false ) {
            if ( TargetX < transform.position.x )
                m_body.velocity = Vector2.left * m_moveSpeed;
            else if ( TargetX > transform.position.x )
                m_body.velocity = Vector2.right * m_moveSpeed;

            // arrive at target
            if ( Mathf.Abs( transform.position.x - TargetX ) < m_atTargetBuffer ) {
                m_hasTarget = false;
                m_body.velocity = Vector2.zero;
                if ( TargetDoor != null ) {
                    TargetDoor.Enter();
                    TargetDoor = null;
                }
            }
        }

        /*
        if( Input.GetMouseButtonDown(0) && m_isTurning == false ) {
            var diffX = MouseWorldPos.x - transform.position.x;
            StartCoroutine( ShowPlayerArrow( diffX > 0f ) );
        }
        */
    }

    private IEnumerator ShowBoyArrow( bool a_facingRight, EmotionalState m_emotionalState = EmotionalState.Confused ) {
        m_isReacting = true;

        EmotionalState = m_emotionalState;
        m_body.velocity = Vector2.zero;

        m_boyArrow.SetActive( true );
        m_boyArrow.GetComponent<SpriteRenderer>().flipX = !a_facingRight;
        yield return new WaitForSeconds( 0.5f );
        m_boyArrow.SetActive( false );

        if( a_facingRight ) m_body.velocity = Vector2.right * m_moveSpeed;
        else m_body.velocity = Vector2.left * m_moveSpeed;

        EmotionalState = m_prevEmotionalState;

        m_isReacting = false;
    }

    private IEnumerator ShowPlayerArrow( bool a_facingRight ) {
        EmotionalState = EmotionalState.Confused;
        m_body.velocity = Vector2.zero;

        m_playerArrow.transform.position = MouseWorldPos;
        m_playerArrow.GetComponent<SpriteRenderer>().flipX = !a_facingRight;
        m_playerArrow.SetActive( true );
        yield return new WaitForSeconds( 0.5f );
        m_playerArrow.SetActive( false );

        if ( m_isCroc ) StartCoroutine( ShowBoyArrow( a_facingRight ) );
        else StartCoroutine( ShowBoyArrow( !m_spriteRenderer.flipX ) );
    }

    private IEnumerator LoseLunchMoney( Bully a_bully ) {
        EmotionalState = EmotionalState.Dizzy;

        foreach( Transform child in transform ) {
            child.transform.parent = null;
            var body = child.GetComponent<Rigidbody2D>();
            if ( body == null ) continue;
            body.simulated = true;
            body.velocity = Vector2.up + Random.Range( -1f, 1f ) * Vector2.right;
            body.velocity *= m_itemThrowSpeed;

            Destroy( child.gameObject, 3f );
        }

        var timeElapsed = 0f;
        while( timeElapsed < 1f ) {
            m_body.velocity = Vector2.zero;
            a_bully.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        EmotionalState = EmotionalState.Scared;
        m_body.velocity = Vector2.right * m_moveSpeed;

        CrocBoyManager.instance.State = GameState.VisitingCrocodile;
    }
}
