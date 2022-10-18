using UnityEngine;

namespace Character
{

    public interface IPlayerController 
    {
        Vector3 Velocity { get; }
        FrameInput Input { get; }
        bool JumpingThisFrame { get; }
        bool LandingThisFrame { get; }
        Vector3 RawMovement { get; }
        bool Grounded { get; }
    }

    public interface IPlayerCombat
    {
        int? CountAttack { get; }
        bool IsAttacking { get; }
        bool IsBlocking { get; }
        bool PressBlockThisFrame { get; }
        bool IsParry { get; }
        float GetReduceDamagePercent();
        void SetIsParry(bool isParry);
        
    }

    public interface IAnimatorController 
    {
        Animator Animator { get; set; }
        void SetIsAttacking(bool isAttacking);
        void TriggerAttack(int? countAttack);
        void TriggerAttacked();
        void SetDeath();
        void SetBlock(bool isBlocking);
        void FilpCharacter();
        void FilpCharacter(Vector2 direction);
        float GetAnimationLength(string animName);
    }

    public struct FrameInput 
    {
        public float X;
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