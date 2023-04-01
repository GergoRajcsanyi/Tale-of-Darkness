using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int sceneIndex;
    public Vector3 playerPosition;
    public int deathCount;
    public int madnessCount;
    public bool joystickVisibility;
    public float audioVolume;
    public SerializableDictionary<string, bool> abilityCollected;

    public GameData()
    {
        this.sceneIndex = 1;
        playerPosition = new Vector3((float)0.23, (float)-4.18, (float)0);
        this.deathCount = 0;
        this.madnessCount = 0;
        abilityCollected = new SerializableDictionary<string, bool>();
        joystickVisibility = true;
        this.audioVolume = 0f;
    }
}
