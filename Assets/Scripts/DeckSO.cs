using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Deck")]
public class DeckSO : ScriptableObject
{
    [Serializable]
    public class Cards{
        public List<Sprite> values;
    }
    public List<Cards> faces;
}
