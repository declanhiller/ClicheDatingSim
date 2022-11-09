using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[System.Serializable]
public class Story
{
    public string name;
    public StoryEvent start;
    public List<StoryEvent> allEvents = new List<StoryEvent>();



}