using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> audios = new List<AudioSource>();
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            if (i == 0)
            {
                audio.volume = 0.5f;
            }
            audios.Add(audio);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    public void PlayClip(int Audiosindex,AudioClip clip,bool isLoop)
    {
        var audio = audios[Audiosindex];
        audio.clip = clip;
        audio.loop = isLoop;
        audio.Play();
    }
    public void StopClip()
    {
        audios[0].Stop();
        audios[1].Stop();
        audios[2].Stop();
    }
    public void PauseBackSound()
    {
        audios[0].Pause();
        audios[2].Pause();
    }
    public void ContinueBackSound()
    {
        audios[0].UnPause();
        audios[2].UnPause();
    }
}
