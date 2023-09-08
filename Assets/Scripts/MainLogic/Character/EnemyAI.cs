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
using LDtkUnity;
using Core.Repositories;
using Zenject;
using Infrastructure.Entities;

namespace Character
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Pathfinding")]
        public Transform Target;
        public float ActivateDistance = 50f;
        public float PathUpdateSecond = 1f;

        [Header("Patrol")]
        public Transform PointA;
        public Transform PointB;
        private Transform PatrolPoint;
        public float TurnDuration = 2f;
        private float TimeSinceReachedPoint = 0f;
        private LDtkFields Fields;

        [Header("Physics")]
        public float Speed = 200f;
        public float MaxSpeed = 3f;
        public float NextWaypointDistance = 2f;
        public float JumpNodeHeightRequirement = 0.8f;
        public float JumpSpeed = 8f;
        public float JumpCheckOffset = 0.1f;
        public LayerMask GroundLayer;

        [Header("Combat")]
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
        public bool AlwaysFollowWhenDetectedEnabled = false;
        public bool JumpEnabled = true;
        public bool AttackEnabled = true;
        public bool DirectionLookEnabled = true;

        [Header("Events")]
        public GameEvent OnDetectedPlayer;

        private Path Path;
        private int CurrentWaypoint = 0;
        public bool IsJumping { get; private set; }
        public bool IsGrounded { get; private set; }
        private bool IsKnockingBack = false;
        private bool IsDetectedEnemy = false;
        private Seeker Seeker;
        private Rigidbody2D Rb;
        private KnockBack KnockBack;
        private EnemyAnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private AnimatorClipInfo[] AnimatorClip;

        private EnemyStatus EnemyStatus;
        public EnemyConfig Config { get; private set; }

        #region Dependencies
        [Inject]
        private IEnemyConfigRepository enemyConfigRepository;
        #endregion
        private void Awake() 
        {
            Seeker = GetComponent<Seeker>();
            Rb = GetComponent<Rigidbody2D>();
            KnockBack = GetComponent<KnockBack>();
            AnimatorController = GetComponent<EnemyAnimatorController>();
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
            BehaviourStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Behaviour);
            EnemyStatus = GetComponent<EnemyStatus>();
            Fields = GetComponent<LDtkFields>();
            PointA = Fields?.GetEntityReference("PointA").FindEntity()?.transform;
            PointB = Fields?.GetEntityReference("PointB").FindEntity()?.transform;
            Config = enemyConfigRepository.GetById(EnemyStatus.Type);
        }

        private void Start()
        {
            Target = GameObject.FindGameObjectWithTag(TagName.Player).transform.Find(GameObjectName.TargetPoint);
            PatrolPoint = PointA;
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

            if((isIdleCombat || isMovableCombat || isAttackFromAir)
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

        public void ResetStateMachine()
        {
            CombatStateMachine.SetNextStateToMain();
            BehaviourStateMachine.SetNextStateToMain();
        }

        #region Pathfinding

        public float DistanceFromTarget()
        {
            return Vector2.Distance(transform.position, Target.position);
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
            if(Seeker.IsDone())
            {
                if(FollowEnabled && TargetInDistance())
                {
                    IsDetectedEnemy = true;
                    PatrolPoint = PointA;
                    Seeker.StartPath(Rb.position, Target.position, OnPathComplete);
                }
                else if (PointA != null && PointB != null)
                {
                    //Reached to point
                    if(CurrentWaypoint >= Path?.vectorPath.Count)
                    {
                        TimeSinceReachedPoint += PathUpdateSecond;
                        if(TimeSinceReachedPoint >= TurnDuration)
                        {
                            if(PatrolPoint.gameObject == PointA.gameObject)
                            {
                                PatrolPoint = PointB;
                            }
                            else
                            {
                                PatrolPoint = PointA;
                            }
                        }
                    }
                    else
                    {
                        TimeSinceReachedPoint = 0f;
                    }

                    Seeker.StartPath(Rb.position, PatrolPoint.position, OnPathComplete);
                }
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
            var collider = GetComponent<Collider2D>();
            IsGrounded = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
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
            if(Rb.velocity.magnitude <= MaxSpeed)
            {
                Rb.AddForce(direction * Speed * Time.deltaTime);
            }

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
            var isTargetInDistance = DistanceFromTarget() < ActivateDistance;

            //fire event first time on detected
            if(isTargetInDistance && IsDetectedEnemy == false)
            {
                OnDetectedPlayer?.Raise(this, Config);
            }

            return isTargetInDistance || (IsDetectedEnemy && AlwaysFollowWhenDetectedEnabled);
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

        public void TriggerDeflected()
        {
            BehaviourStateMachine.SetNextState(new DisabledMoveState(1f));
            CombatStateMachine.SetNextStateToMain();
            EnemyStatus.IsImmortal = false;
            AnimatorController.TriggerDeflected();
        }

        private void Attack()
        {
            DetectedEnemies = Physics2D.OverlapCircleAll(DetectEnemyTransform.position, DetectEnemyRange, EnemyLayers).ToList();
            bool isEnemyDetected =  DetectedEnemies.Any();
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) 
                && AnimatorController.IsDeflected == false
                && isEnemyDetected 
                && TimeSinceAttack > EnemyStatus.Attribute.BufferTimeAttack)
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

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(DetectEnemyTransform.position, ActivateDistance);
        }
    }
}
