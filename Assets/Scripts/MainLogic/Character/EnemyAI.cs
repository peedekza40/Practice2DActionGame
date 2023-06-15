using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using Core.Constants;
using Character.Behaviours;
using Character.Interfaces;
using Constants;
using Character.Combat.States;
using Character.Behaviours.States;
using Character.Animators;
using Character.Status;
using Character.Combat.States.BossKnight;

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
        public float JumpSpeed = 8f;
        public float JumpCheckOffset = 0.1f;

        [Header("Combat")]
        public float AttackDuration = 1.5f;
        public float TimeBetweenCombo = 0.25f;
        public float MaxDamage = 20f;
        public float MinDamage = 10f;
        public float BufferTimeAttack = 0.5f;
        public List<Collider2D> HitBoxes;
        public float DetectEnemyRange = 0.5f;
        public Transform DetectEnemyTransform;
        public LayerMask EnemyLayers;
        public List<Collider2D> DetectedEnemies { get; private set; } = new List<Collider2D>();
        public StateMachine CombatStateMachine { get; private set; }
        public StateMachine BehaviourStateMachine { get; private set; }
        private float TimeSinceAttack = 0f;

        [Header("Custom Behavior")]
        public bool FollowEnabled = true;
        public bool JumpEnabled = true;
        public bool AttackEnabled = true;
        public bool DirectionLookEnabled = true;

        private Path Path;
        private int CurrentWaypoint = 0;
        public bool IsJumping { get; private set ;}
        public bool IsGrounded { get; private set ;}
        private bool IsKnockingBack = false;
        private Seeker Seeker;
        private Rigidbody2D Rb;
        private KnockBack KnockBack;
        private AnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private AnimatorClipInfo[] AnimatorClip;

        private EnemyStatus EnemyStatus;

        private void Awake() 
        {
            Seeker = GetComponent<Seeker>();
            Rb = GetComponent<Rigidbody2D>();
            KnockBack = GetComponent<KnockBack>();
            AnimatorController = GetComponent<AnimatorController>();
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
            BehaviourStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Behaviour);
            EnemyStatus = GetComponent<EnemyStatus>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdatePath), 0f, PathUpdateSecond);
        }

        private void Update()
        {
            AnimationState = AnimatorController.MainAnimator.GetCurrentAnimatorStateInfo(0);
            AnimatorClip = AnimatorController.MainAnimator.GetCurrentAnimatorClipInfo(0);

            var isIdleCombat = CombatStateMachine.IsCurrentState(typeof(IdleCombatState));
            var isMovableCombat = CombatStateMachine.IsCurrentState(typeof(MovableCombatState));
            var isAttackFromAir = CombatStateMachine.IsCurrentState(typeof(AttackFromAirState));//special case
            var isDisabledMove = BehaviourStateMachine.IsCurrentState(typeof(DisabledMoveState));
            IsKnockingBack = BehaviourStateMachine.IsCurrentState(typeof(KnockBackState));
            if(TargetInDistance() && FollowEnabled 
                && (isIdleCombat || isMovableCombat || isAttackFromAir)
                && isDisabledMove == false
                && IsKnockingBack == false)
            {
                PathFollow();
            }

            if(AttackEnabled)
            {
                Attack();
            }
        }

        #region Pathfinding

        public float DistanceFromTarget()
        {
            return Vector2.Distance(transform.position, Target.transform.position);
        }

        public Vector2 DirectionToTarget()
        {
            return ((Vector2)Target.transform.position - Rb.position).normalized;
        }

        public void SetIsJumping(bool isJumping)
        {
            IsJumping = isJumping;
        }

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
            IsJumping = IsGrounded == false;
            
            //Direction Calculation
            Vector2 direction = ((Vector2)Path.vectorPath[CurrentWaypoint] - Rb.position).normalized;

            //Jump
            if(JumpEnabled && IsGrounded)
            {
                if(direction.y > JumpNodeHeightRequirement)
                {
                    Rb.velocity = Vector2.up * JumpSpeed;
                    IsJumping = true;
                }
            }

            //Movement
            Rb.AddForce(direction * Speed * Time.deltaTime);

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
            return DistanceFromTarget() < ActivateDistance;
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
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) && isEnemyDetected && TimeSinceAttack > BufferTimeAttack)
            {
                TimeSinceAttack = 0f;
                State attackState = null;
                switch(EnemyStatus.Type)
                {
                    case EnemyId.Skeleton : 
                        attackState = new Character.Combat.States.Skeleton.MeleeEntryState(); 
                        break;
                    case EnemyId.Goblin : 
                        attackState = new Character.Combat.States.Goblin.MeleeEntryState(); 
                        break;
                    case EnemyId.Mushroom :
                        attackState = new Character.Combat.States.Mushroom.MeleeEntryState(); 
                        break;
                    case EnemyId.BossKnight :
                        attackState = new Character.Combat.States.BossKnight.MeleeEntryState();
                        break;
                }

                CombatStateMachine.SetNextState(attackState);
            }

            TimeSinceAttack += Time.deltaTime;
        }

        #endregion

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DetectEnemyTransform.position, DetectEnemyRange);
        }
    }
}
