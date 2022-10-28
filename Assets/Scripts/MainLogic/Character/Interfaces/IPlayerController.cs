using UnityEngine;

namespace Character.Interfaces
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
}