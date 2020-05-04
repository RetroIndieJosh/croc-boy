using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Any,
    GoingToLunch,
    VisitingCrocodile,
    SearchingForSuit
}

public class CrocBoyManager : MonoBehaviour
{
    public static CrocBoyManager instance = null;

    [SerializeField] private string m_mainSceneName = "main";
    [SerializeField] private string m_startLevelSceneName = "level";
    [SerializeField] private SpriteRenderer m_fadeImageRenderer = null;

    public GameState State = GameState.GoingToLunch;

    private string m_activeLevelSceneName = "";

    public void ChangeLevel( string a_levelSceneName, bool fade = true ) {
        StartCoroutine( ChangeLevelCoroutine( a_levelSceneName, fade ) );
    }

    private IEnumerator ChangeLevelCoroutine( string a_levelSceneName, bool fade ) {
        if ( fade ) {
            Time.timeScale = 0f;
            FadeOut( 1f );
            yield return new WaitForSecondsRealtime( 1f );
            Time.timeScale = 1f;
        }

        for ( var i = 0; i < SceneManager.sceneCount; ++i ) {
            if ( SceneManager.GetSceneAt( i ).name == a_levelSceneName ) {
                m_activeLevelSceneName = a_levelSceneName;
                FadeIn( 1f / 60f );
                yield break;
            }
        }

        if ( string.IsNullOrEmpty( m_activeLevelSceneName ) == false )
            SceneManager.UnloadSceneAsync( m_activeLevelSceneName );

        SceneManager.LoadScene( a_levelSceneName, LoadSceneMode.Additive );
        //SceneManager.LoadSceneAsync( a_levelSceneName, LoadSceneMode.Additive );
        m_activeLevelSceneName = a_levelSceneName;

        if ( fade ) {
            Time.timeScale = 0f;
            FadeIn( 1f );
            yield return new WaitForSecondsRealtime( 1f );
            Time.timeScale = 1f;
        }
    }

    public void FadeOut( float a_seconds ) {
        StartCoroutine( FadeCoroutine( a_seconds, true ) );
    }

    public void FadeIn( float a_seconds ) {
        StartCoroutine( FadeCoroutine( a_seconds, false ) );
    }

    private IEnumerator FadeCoroutine( float a_seconds, bool a_fadeOut ) {
        {
            var color = m_fadeImageRenderer.color;
            color.a = a_fadeOut ? 0f : 1f;
            m_fadeImageRenderer.color = color;
        }

        var timeElapsed = 0f;
        while ( timeElapsed < a_seconds ) {
            timeElapsed += Time.unscaledDeltaTime;
            var t = timeElapsed / a_seconds;
            var color = m_fadeImageRenderer.color;
            color.a = a_fadeOut ? t : 1f - t;
            m_fadeImageRenderer.color = color;
            yield return null;
        }
    }

    private void SceneManager_sceneLoaded( Scene arg0, LoadSceneMode arg1 ) {
        Boy.instance.StartMoving( false );
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        var color = m_fadeImageRenderer.color;
        color.a = 0f;
        m_fadeImageRenderer.color = color;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        for ( var i = 0; i < SceneManager.sceneCount; ++i ) {
            var name = SceneManager.GetSceneAt( i ).name;
            if ( name == m_mainSceneName || name == m_startLevelSceneName )
                continue;
            SceneManager.UnloadSceneAsync( name );
        }

        ChangeLevel( m_startLevelSceneName, false );
    }
}
