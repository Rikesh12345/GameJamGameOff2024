using System.Collections.Generic;
using UnityEngine;

public static class SoundRateLimiter
{
    private static Dictionary<string, float> _lastPlayedTime = new Dictionary<string, float>();

    /// <summary>
    /// Checks if the cooldown has elapsed for the given sound key.
    /// </summary>
    public static bool IsCooldownElapsed(string soundKey, float cooldown)
    {
        float currentTime = Time.time;

        if (_lastPlayedTime.TryGetValue(soundKey, out float lastPlayed))
        {
            return currentTime - lastPlayed >= cooldown;
        }

        return true; // No record exists; cooldown is elapsed
    }

    /// <summary>
    /// Registers the current time for the given sound key.
    /// </summary>
    public static void RegisterSoundPlay(string soundKey)
    {
        _lastPlayedTime[soundKey] = Time.time;
        _lastPlayedTime.TryGetValue(soundKey, out float time);
        Debug.Log("Dictionary: " + _lastPlayedTime);
        Debug.Log("SoundKey: " + soundKey + " Time: " + time);
    }
}

