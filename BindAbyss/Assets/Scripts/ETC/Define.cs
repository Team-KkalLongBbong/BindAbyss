using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

public enum Layer
    {
        Ground = 6,
        Block = 7,
        Monster = 8
    }
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }
    public enum CameraMode
    {
        QuarterView
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Test
    }

    public enum UIEvent
    {
        Click,
        Drag
    }

}