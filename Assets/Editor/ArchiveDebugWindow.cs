using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArchiveDebugWindow : EditorWindow
{
    int archiveIndex;

    ArchiveDebugWindow()
    {
        this.titleContent = new GUIContent("Archive Debugger");
    }

    [MenuItem("Tool/Archive Debugger")]
    static void showWindow()
    {
        EditorWindow.GetWindow(typeof(ArchiveDebugWindow));
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Archive Debugger");

        GUILayout.Space(10);
        archiveIndex = EditorGUILayout.IntField("´æµµ±àºÅ",archiveIndex);


        if (GUILayout.Button("Print Archive"))
        {
            ArchiveManager.DebugArchiveRead(archiveIndex);
        }
        if (GUILayout.Button("Delete Archive"))
        {
            ArchiveManager.DeleteArchive(archiveIndex);
        }

        GUILayout.EndVertical();
    }
}
