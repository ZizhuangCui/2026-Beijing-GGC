using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DogBarkOnKey : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip barkClip;

    [Header("Input")]
    [SerializeField] private KeyCode barkKey = KeyCode.G;

    [Header("Behavior")]
    [SerializeField] private bool allowRetriggerWhilePlaying = true;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.loop = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(barkKey))
        {
            Bark();
        }
    }

    public void Bark()
    {
        if (barkClip == null)
        {
            Debug.LogWarning($"{nameof(DogBarkOnKey)}: barkClip is not assigned.", this);
            return;
        }

        if (!allowRetriggerWhilePlaying && _audio.isPlaying)
            return;

        // One-shot 不会打断别的音效播放，更适合短音效
        _audio.PlayOneShot(barkClip);
    }
}
