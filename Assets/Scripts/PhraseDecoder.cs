using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phrase
{
    // These variables are case sensitive and must match the strings "enemy" and "player" in the JSON.
    public string enemy;
    public string player;
}

[System.Serializable]
public class Phrases
{
    // The variable name "phrases" is case sensitive and must match the string "phrases" in the JSON.
    public Phrase[] phrases;
}

public class PhraseDecoder : MonoBehaviour
{
    public TextAsset jsonFile;
    public Phrase[] phrases;

    void Start()
    {
        phrases = JsonUtility.FromJson<Phrases>(jsonFile.text).phrases;

        //Debug.Log(phrases[0].player);
    }

    public Phrase GetRandomPhrase() {
        return phrases[Random.Range(0, phrases.Length)];
    }
}
