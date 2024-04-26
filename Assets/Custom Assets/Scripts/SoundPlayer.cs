using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundPlayer
{
    public static AudioSource PlayClip3d(AudioClip clip, float volume, Vector3 position)
    {
        GameObject audioObject = new GameObject("SFX");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioObject.transform.position = position;
        audioSource.spatialBlend = 1;

        audioSource.Play();

        Object.Destroy(audioObject, clip.length);

        return audioSource;
    }
}