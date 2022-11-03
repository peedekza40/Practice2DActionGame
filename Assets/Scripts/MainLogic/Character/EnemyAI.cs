using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using Core.Constants;
using Character.Behaviours;
using Character.Interfaces;
using Constants;
using Character.Combat.States;
using Character.Combat.States.Skeleton;
using Character.Behaviours.States;
using Character.Animators;

namespace Character
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Pathfinding")]
        public Transform Target;
        public float ActivateDistance = 50f;
        public float PathUpdateSecond = 1f;

        [Header("Physics")]
        public float Speed = 200f;
        public float NextWaypointDistance = 2f;
        public float JumpNodeHeightRequirement = 0.8f;
        public float JumpModifier = 0.3f;
        public float JumpCheckOffset = 0.1f;

        [Header("Combat")]
        public float AttackDuration = 1.5f;
        public float TimeBetweenCombo = 0.25f;
        public float MaxDamage = 20f;
        public Collider2D HitBox;
        public float DetectEnemyRange = 0.5f;
        public Transform DetectEnemyTransform;
        public LayerMask EnemyLayers;
        public List<Collider2D> DetectedEnemies { get; private set; } = new List<Collider2D>();
        private StateMachine CombatStateMachine;
        private StateMachine BehaviourStateMachine;

        [Header("Custom Behavior")]
        public bool FollowEnabled = true;
        public bool JumpEnabled = true;
        public bool AttackEnabled = true;
        public bool DirectionLookEnabled = true;

        private Path Path;
        private int CurrentWaypoint = 0;
        private bool IsGrounded = false;
        private bool IsKnockingBack = false;
        private Seeker Seeker;
        private Rigidbody2D Rb;
        private KnockBack KnockBack;
        private AnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private AnimatorClipInfo[] AnimatorClip;

        private void Awake() 
        {
            Seeker = GetComponent<Seeker>();
            Rb = GetComponent<Rigidbody2D>();
            KnockBack = GetComponent<KnockBack>();
            AnimatorController = GetComponent<AnimatorController>();
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
            BehaviourStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Behaviour);
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdatePath), 0f, PathUpdateSecond);
        }

        private void Update()
        {
            AnimationState = AnimatorController.MainAnimator.GetCurrentAnimatorStateInfo(0);
            AnimatorClip = AnimatorController.MainAnimator.GetCurrentAnimatorClipInfo(0);

            bool isIdleCombat = CombatStateMachine.IsCurrentState(typeof(IdleCombatState));
            IsKnockingBack = BehaviourStateMachine.IsCurrentState(typeof(KnockBackState));
            if(TargetInDistance() && FollowEnabled && isIdleCombat && IsKnockingBack == false)
            {
                PathFollow();
            }

            if(AttackEnabled)
            {
                Attack();
            }
        }

        #region Pathfinding

        private void UpdatePath()
        {
            if(FollowEnabled && TargetInDistance() && Seeker.IsDone())
            {
                Seeker.StartPath(Rb.position, Target.position, OnPathComplete);
            }
        }

        private void PathFollow()
        {
            if(Path == null)
            {
                return;
            }

            //Reached end of path
            if(CurrentWaypoint >= Path.vectorPath.Count)
            {
                return;
            }

            //See if colliding with anything
            Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + JumpCheckOffset);
            IsGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);
            
            //Direction Calculation
            Vector2 direction = ((Vector2)Path.vectorPath[CurrentWaypoint] - Rb.position).normalized;
            Vector2 force = direction * Speed * Time.deltaTime;

            //Jump
            if(JumpEnabled && IsGrounded)
            {
                if(direction.y > JumpNodeHeightRequirement)
                {
                    Rb.AddForce(Vector2.up * Speed * JumpModifier);
                }
            }

            //Movement
            Rb.AddForce(force);


            //Next Waypoint
            float distance = Vector2.Distance(Rb.position, Path.vectorPath[CurrentWaypoint]);
            if(distance < NextWaypointDistance)
            {
                CurrentWaypoint++;
            }

            //Direction Graphics Handling
            if(DirectionLookEnabled && IsKnockingBack == false)
            {
                AnimatorController.FilpCharacter();
            }
        }

        private bool TargetInDistance()
        {
            return Vector2.Distance(transform.position, Target.transform.position) < ActivateDistance;
        }

        private void OnPathComplete(Path path)
        {
            if(!path.error)
            {
                Path = path;
                CurrentWaypoint = 0;
            }
        }

        #endregion

        #region Combat

        private void Attack()
        {
            DetectedEnemies = Physics2D.OverlapCircleAll(DetectEnemyTransform.position, DetectEnemyRange, EnemyLayers).ToList();
            bool isEnemyDetected =  DetectedEnemies.Any();
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) && isEnemyDetected)
            {
                CombatStateMachine.SetNextState(new MeleeEntryState());
            }
        }

        #endregion

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DetectEnemyTransform.position, DetectEnemyRange);
        }
    }
}
