using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[System.Serializable]
public class Story : MonoBehaviour
{
    public string name;
    public StoryEvent start;
    public List<StoryEvent> allEvents = new List<StoryEvent>();
    [NonSerialized]
    public string fileSavePath;

}