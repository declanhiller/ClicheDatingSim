using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryEvent {
    public string eventName;
    public List<StoryEvent> childEvents = new List<StoryEvent>();
    protected EventManager eventManager;

    internal void Start() {
    }
    
    

}
