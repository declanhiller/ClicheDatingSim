using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryEvent {
    public string eventName;
    public List<Connection> childEvents = new List<Connection>();
    protected EventManager eventManager;

    internal void Start() {
    }
    
    

}
