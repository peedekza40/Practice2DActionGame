using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {

    public interface IPlayerController 
    {
        Vector3 Velocity { get; }
        FrameInput Input { get; }
        bool JumpingThisFrame { get; }
        bool LandingThisFrame { get; }
        Vector3 RawMovement { get; }
        bool Grounded { get; }
    }

    public interface IPlayerAttack 
    {
        int? CountAttack { get; }
    }

    public interface IAnimatorController 
    {
        Animator Animator { get; set; }
        void TriggerAttack(int? countAttack);
        void TriggerAttacked();
        void SetDeath();
        void FilpCharacter();
        float GetAnimationLength(string animName);
    }

    public struct FrameInput 
    {
        public float X;
        public bool JumpDown;
        public bool JumpUp;
        public bool AttackDown;
    }

    public struct RayRange 
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir) {
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
            Dir = dir;
        }

        public readonly Vector2 Start, End, Dir;
    }
}