using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource playerAudioSource;

    public AudioClip paintStart;    // Holds the clip that plays when the player starts painting
    public AudioClip paintWhile;    // Holds the clip that plays while the player is painting
    public AudioClip paintEnd;      // Holds the clip that plays when the player finish painting

    public void StartPaintingSound()
    {
        //isPainting = true;

        playerAudioSource.loop = false;
        playerAudioSource.clip = paintStart;
        playerAudioSource.Play();
    }

    public void WhilePaintingSound()
    {
        /*
        if (!isPainting)
            return;
            
        */

        // se o clip ainda não está a tocar, muda para o de loop
        if (!playerAudioSource.isPlaying)
        {
            playerAudioSource.clip = paintWhile;
            playerAudioSource.loop = true;
            playerAudioSource.Play();
        }
    }

    public void EndPaintingSound()
    {
        //isPainting = false;

        playerAudioSource.loop = false;
        playerAudioSource.clip = paintEnd;
        playerAudioSource.Play();
    }
}
