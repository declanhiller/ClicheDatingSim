using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class StoryEvent{
    public string eventName;
    public List<StoryEvent> childEvents = new List<StoryEvent>();

    internal void Start() {
    }
    
    

}
