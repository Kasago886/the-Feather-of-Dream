using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    Character character;
    public static Hashtable CharacterUI;
    private void OnEnable()
    {
        character = target as Character;
    }
    public override void OnInspectorGUI()
    {
        
    }
}
