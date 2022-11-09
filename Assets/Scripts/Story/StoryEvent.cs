using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class StoryEvent{
    public string eventName;
    public List<StoryEvent> childEvents = new List<StoryEvent>();
    public int id;
    public bool start = false;
    public List<int> childrenID = new List<int>();

    public struct Args {
        private int posX;
        private int posY;
    }

}
