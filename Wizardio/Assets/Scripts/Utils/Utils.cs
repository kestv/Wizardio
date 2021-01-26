using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Dictionary<int, string> SpellsMapping = new Dictionary<int, string>()
    {
        {(int)KeyCode.Mouse0, "spell1" },
        {(int)KeyCode.Mouse1, "spell2" }
    };

    private static Dictionary<string, Cooldown> Cooldowns = new Dictionary<string, Cooldown>();

    public static void StartCooldown(string key, float length)
    {
        if (IsCooldownFinished(key))
        {
            Cooldown cooldown = new Cooldown()
            {
                Key = key,
                Length = length,
                Start = Time.time
            };
            Cooldowns.Add(key, cooldown);
        }
    }

    public static bool IsCooldownFinished(string key)
    {
        if (!Cooldowns.ContainsKey(key))
        {
            return true;
        }

        var cd = Cooldowns[key];
        return Time.time > cd.Start + cd.Length;
    }
    public static bool IsInRange(Vector3 _objectPosition, Vector3 _targetPosition, double _error)
    {
        float objectX = _objectPosition.x;
        float objectY = _objectPosition.y;
        float objectZ = _objectPosition.z;

        float targetX = _targetPosition.x;
        float targetY = _targetPosition.y;
        float targetZ = _targetPosition.z;

        if (objectX <= targetX + _error && objectX >= targetX - _error
         && objectY <= targetY + _error && objectY >= targetY - _error
         && objectZ <= targetZ + _error && objectZ >= targetZ - _error)
        {
            return true;
        }
        return false;
    }
}
