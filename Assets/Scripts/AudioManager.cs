using UnityEngine;

public class AudioManager : MonoBehaviour
{

    internal static AudioManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void PlaySoundAt(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
