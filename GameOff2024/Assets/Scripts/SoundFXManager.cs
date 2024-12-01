using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField]
    private AudioSource _soundFXPrefab;
    [SerializeField]
    private AudioSource _loopingSoundFXPrefab;

    // Dictionary to track if a sound is playing and its AudioSource
    private Dictionary<string, AudioSource> _activeAudioSources = new Dictionary<string, AudioSource>();



    private void Awake()
    {
        try
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Awake: " + ex);
        }
    }

    //public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume, float startTime = 0f, float endTime = -1f, float distance = 500f)
    //{
    //    AudioSource audioSource = null;
    //    float clipLength = 0;
    //    try
    //    {
    //        int random = UnityEngine.Random.Range(0, audioClips.Length);

    //        audioSource = Instantiate(_soundFXPrefab, spawnTransform.position, Quaternion.identity);
    //        audioSource.maxDistance = distance;
    //        audioSource.clip = audioClips[random];
    //        audioSource.volume = volume;

    //        clipLength = audioSource.clip.length;

    //        if (startTime > 0f)
    //            audioSource.time = startTime;  // Start from a specific time if provided

    //        // Play the sound for the duration
    //        if (endTime > 0f && endTime <= audioClips.Length)
    //        {
    //            audioSource.Play();
    //            float playDuration = endTime - startTime;
    //            StartCoroutine(StopSoundAfterDuration(audioSource, playDuration));
    //        }
    //        else
    //        {
    //            // Play the whole sound
    //            audioSource.Play();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".PlayRandomSoundFXClip: " + ex);
    //    }
    //    finally
    //    {
    //        if (audioSource != null)
    //            Destroy(audioSource.gameObject, clipLength);
    //    }
    //}

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume, float startTime = 0f, float endTime = -1f,  float distance = 500f, float cooldown = 0.3f,  /*Cooldown duration in seconds*/ string soundKey = "DefaultSoundKey"  /*Unique key for this sound type*/)
    {
        AudioSource audioSource = null;
        float clipLength = 0;

        try
        {
            if (!soundKey.Contains("DefaultSoundKey"))
            {
                // Check cooldown before playing the sound
                if (!SoundRateLimiter.IsCooldownElapsed(soundKey, cooldown))
                {
                    Debug.Log($"Sound with key '{soundKey}' skipped due to cooldown.");
                    return; // Exit if the cooldown hasn't elapsed
                }

                // Register the sound play time
                SoundRateLimiter.RegisterSoundPlay(soundKey);
            }


            // Pick a random sound
            int random = UnityEngine.Random.Range(0, audioClips.Length);
            AudioClip selectedClip = audioClips[random];



            // Instantiate the sound effect prefab
            audioSource = Instantiate(_soundFXPrefab, spawnTransform.position, Quaternion.identity);
            audioSource.maxDistance = distance;
            audioSource.clip = selectedClip;
            audioSource.volume = volume;

            clipLength = selectedClip.length;

            if (startTime > 0f)
                audioSource.time = startTime; // Start from a specific time if provided

            // Play the sound for a specific duration if endTime is valid
            if (endTime > 0f && endTime <= selectedClip.length)
            {
                audioSource.Play();
                float playDuration = endTime - startTime;
                StartCoroutine(StopSoundAfterDuration(audioSource, playDuration));
            }
            else
            {
                // Play the full clip
                audioSource.Play();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayRandomSoundFXClip: " + ex);
        }
        finally
        {
            if (audioSource != null)
                Destroy(audioSource.gameObject, clipLength); // Ensure cleanup
        }
    }


    // Coroutine to stop the sound
    private IEnumerator StopSoundAfterDuration(AudioSource audioSource, float duration)
    {
        // Wait for the clip to finish
        yield return new WaitForSeconds(duration);

        if (audioSource != null)
        {
            // Destroy the AudioSource object after the clip has finished
            Destroy(audioSource.gameObject);
        }
    }

    // Function to play a sound if it's not already playing
    public void PlayTrimmedSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float startTime = 0f, float endTime = -1f, string soundKey = "default", float distance = 500f)
    {
        // Ensure that the sound is not already playing for the same key
        if (_activeAudioSources.ContainsKey(soundKey) && _activeAudioSources[soundKey] != null)
            return;

        try
        {
            AudioSource audioSource = Instantiate(_soundFXPrefab, spawnTransform.position, Quaternion.identity);

            audioSource.maxDistance = distance;

            audioSource.clip = audioClip;
            audioSource.volume = volume;

            if (startTime > 0f)
                audioSource.time = startTime;  // Start from a specific time if provided

            // Play the sound for the duration
            if (endTime > 0f && endTime <= audioClip.length)
            {
                audioSource.Play();
                float playDuration = endTime - startTime;
                StartCoroutine(StopAndResetSound(audioSource, playDuration, soundKey));
            }
            else
            {
                // Play the whole sound
                audioSource.Play();
                StartCoroutine(StopAndResetSound(audioSource, audioClip.length, soundKey));
            }

            // Store the active AudioSource for this soundKey
            _activeAudioSources[soundKey] = audioSource;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayTrimmedSoundFXClip: " + ex);
        }
    }

    // Coroutine to stop the sound and reset the flag after it finishes
    private IEnumerator StopAndResetSound(AudioSource audioSource, float duration, string soundKey)
    {
        // Wait for the clip to finish
        yield return new WaitForSeconds(duration);

        if (audioSource != null)
        {
            // Destroy the AudioSource object after the clip has finished
            Destroy(audioSource.gameObject);

            // Reset the active AudioSource for the provided key
            if (_activeAudioSources.ContainsKey(soundKey))
            {
                _activeAudioSources[soundKey] = null;
            }
        }
    }

    public void PlayLoopingTrimmedSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float startTime, float endTime = -1f, float distance = 500f)
    {
        try
        {
            if (endTime <= 0f)
            {
                endTime = audioClip.length;
            }

            AudioSource audioSource = Instantiate(_loopingSoundFXPrefab, spawnTransform.position, Quaternion.identity);

            audioSource.maxDistance = distance;

            audioSource.clip = audioClip;
            audioSource.volume = volume;

            audioSource.time = startTime;
            audioSource.loop = true;

            // Start playing the loop
            audioSource.Play();

            // Start a coroutine to manage the loop and reset the start time after reaching the endTime
            StartCoroutine(LoopTrimmedAudioClip(audioSource, startTime, endTime));
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayLoopingTrimmedSoundFXClip: " + ex);
        }
    }

    private IEnumerator LoopTrimmedAudioClip(AudioSource audioSource, float startTime, float endTime)
    {
        while (true)
        {
            if (audioSource.time >= endTime)
            {
                audioSource.time = startTime;  // Reset to the start time of the loop
            }

            yield return null;  // Wait for the next frame
        }
    }

    // Function to stop a specific sound by key
    public void StopSound(string soundKey)
    {
        try
        {
            if (_activeAudioSources.ContainsKey(soundKey) && _activeAudioSources[soundKey] != null)
            {
                // Stop and destroy the active sound
                _activeAudioSources[soundKey].Stop();
                Destroy(_activeAudioSources[soundKey].gameObject);

                // Reset the active AudioSource for the provided key
                _activeAudioSources[soundKey] = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StopSound: " + ex);
        }
    }

}
