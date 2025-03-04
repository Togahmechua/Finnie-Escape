using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<Collider2D, Coral> coral = new Dictionary<Collider2D, Coral>();

    public static Coral GetCoral(Collider2D collider)
    {
        if (!coral.ContainsKey(collider))
        {
            coral.Add(collider, collider.GetComponent<Coral>());
        }

        return coral[collider];
    }

    private static Dictionary<Collider2D, PlayerMovement> player = new Dictionary<Collider2D, PlayerMovement>();

    public static PlayerMovement GetPlayerMovement(Collider2D collider)
    {
        if (!player.ContainsKey(collider))
        {
            player.Add(collider, collider.GetComponent<PlayerMovement>());
        }

        return player[collider];
    }
}
