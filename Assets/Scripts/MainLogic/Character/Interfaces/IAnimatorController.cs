using UnityEngine;

namespace Character.Interfaces
{
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
}