using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryHolder : MonoBehaviour {
    
    public TextAsset Prologue;
    public TextAsset BadBoyChapter1;
    public TextAsset BadBoyChapter2;
    public TextAsset BadBoyChapter3;
    public TextAsset MilfChapter;

    private void Awake() {
        // Prologue = Resources.Load<TextAsset>("Prologue");
        // BadBoyChapter1 = Resources.Load<TextAsset>("BadBoyChapter");
        // BadBoyChapter2 = Resources.Load<TextAsset>("BadBoyChapter2");
        // BadBoyChapter3 = Resources.Load<TextAsset>("BadBoyChapter3");
        // MilfChapter = Resources.Load<TextAsset>("MilfChapter");
        DontDestroyOnLoad(gameObject);
    }

    public TextAsset GetStory(string managerName) {
        switch (managerName) {
            case "Prologue":
                return Prologue;
            case "BadBoyChapter":
                return BadBoyChapter1;
            case "BadBoyChapter2":
                return BadBoyChapter2;
            case "BadBoyRomance":
                return BadBoyChapter3;
            case "MilfStory":
                return MilfChapter;
        }

        return null;
    }
}
