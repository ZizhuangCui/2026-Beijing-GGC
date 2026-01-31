using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RightClickZhenBang : MonoBehaviour
{
    [SerializeField] private AudioClip zhenBangClip;
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
        // Êó±êÓÒ¼ü°´ÏÂ
        if (Input.GetMouseButtonDown(1))
        {
            PlayZhenBang();
        }
    }

    public void PlayZhenBang()
    {
        if (zhenBangClip == null)
        {
            Debug.LogWarning($"{nameof(RightClickZhenBang)}: zhenBangClip is not assigned.", this);
            return;
        }

        if (!allowRetriggerWhilePlaying && _audio.isPlaying)
            return;

        _audio.PlayOneShot(zhenBangClip);
    }
}
