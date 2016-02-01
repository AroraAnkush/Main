using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Play_Movie : MonoBehaviour
{

    public RawImage targetImage;
#if UNITY_EDITOR
    public Texture splash2;
    public MovieTexture movie;
#endif
    public string PathToMovie1;
    public string PathToMovie2;


    void nextScene()
    {
        CancelInvoke("nextScene");
        StartCoroutine(UIManager.Instance.Splash_To_Menu());

    }

    void aotherVideo()
    {

        CancelInvoke("aotherVideo");
        Handheld.PlayFullScreenMovie(PathToMovie2, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFill);
        Invoke("nextScene", 0.1f);
    }

    void Start()
    {

        Handheld.PlayFullScreenMovie(PathToMovie1, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFill);

        Invoke("nextScene", 0.1f);

#if UNITY_EDITOR
        // movie = (MovieTexture)targetImage.texture;
        // movie.Play();
        // InvokeRepeating("CheckForMovieChange", 0f, .5f);
#else
       // StartCoroutine(PlayMovie());
#endif


    }

    void CheckForMovieChange()
    {

#if UNITY_EDITOR
        if (targetImage.texture.name == "Splash1" && !movie.isPlaying)
        {
            targetImage.texture = splash2;
            movie = (MovieTexture)targetImage.texture;
            movie.Play();
        }
        else if (targetImage.texture.name == "Splash2" && !movie.isPlaying)
        {
            StartCoroutine(UIManager.Instance.Splash_To_Menu());
        }
#endif

    }

#if !UNITY_EDITOR
    IEnumerator PlayMovie()
    {
        Handheld.PlayFullScreenMovie(PathToMovie1, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFill);

        yield return new WaitForEndOfFrame();

        Handheld.PlayFullScreenMovie(PathToMovie2, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
        yield return new WaitForEndOfFrame();

            StartCoroutine(UIManager.Instance.Splash_To_Menu());

    }
#endif
}