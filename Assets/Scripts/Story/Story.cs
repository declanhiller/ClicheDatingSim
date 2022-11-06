using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "Story", order = 141)]
public class Story : ScriptableObject{
    [SerializeField]
    public StoryEvent start;
    public List<StoryEvent> allEvents = new List<StoryEvent>();
}