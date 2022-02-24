using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvents
{
    public class AI
    {
        public static Action<Vector3> OnSetDestination;
    }

    public class Options
    {
        public static Action<bool> OnToggleQuitMenu;
    }

    public class Player
    {
        public static Action<Vector3> OnSetCheckpoint;
        public static Func<Vector3> OnGetPlayerPos;
        public static Action OnRespawnPlayer;
        public static Func<ProblemType> OnGetProblemType;
        public static Action<ProblemType> OnSetProblemType;
    }

    public class Problems
    {
        public static Action<bool> OnAllowPrompts;
        public static Func<bool> OnGetPrompt;
        public class Jumping
        {
            public static Action OnAddJumpFail;
            public static Action<bool> OnToggleUI;
        }

        public class Platforms
        {
            public static Action OnAddPlatformFail;
            public static Action<float> OnChangeWaveStrength;
            public static Action<bool> OnToggleUI;
        }
    }
    public class Scripts
    {
        public static Action<bool> OnDisableMovement;
        public static Action<bool> OnDisableCamera;
    }
}
