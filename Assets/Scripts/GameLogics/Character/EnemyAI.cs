using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using Constants;

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
        public float TimeBetweenAttack = 1.5f;
        public float TimeBetweenNextMove = 0.5f;
        public float Damage = 20f;
        public Collider2D HitBox;
        public float DetectEnemyRange = 0.5f;
        public Transform DetectEnemy;
        public LayerMask EnemyLayers;
        private int? CountAttack = 0;
        private float TimeSinceAttack = 0f;
        private bool IsAttacking;
        private int DamageFrameCount = 0;

        [Header("Custom Behavior")]
        public bool FollowEnabled = true;
        public bool JumpEnabled = true;
        public bool AttackEnabled = true;
        public bool DirectionLookEnabled = true;

        private Path Path;
        private int CurrentWaypoint = 0;
        private bool IsGrounded = false;
        private Seeker Seeker;
        private Rigidbody2D Rb;
        private KnockBack KnockBack;
        private IAnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private AnimatorClipInfo[] AnimatorClip;

        void Start()
        {
            Seeker = GetComponent<Seeker>();
            Rb = GetComponent<Rigidbody2D>();
            KnockBack = GetComponent<KnockBack>();
            AnimatorController = GetComponent<IAnimatorController>();

            InvokeRepeating(nameof(UpdatePath), 0f, PathUpdateSecond);
        }

        void Update()
        {
            AnimationState = AnimatorController.Animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClip = AnimatorController.Animator.GetCurrentAnimatorClipInfo(0);

            if(TargetInDistance() && FollowEnabled && IsAttacking == false && KnockBack.IsKnockingBack == false)
            {
                PathFollow();
            }

            if(AttackEnabled)
            {
                Attack();
            }

            SetIsAttacking(CountAttack);
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
            if(DirectionLookEnabled && KnockBack.IsKnockingBack == false)
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
            List<Collider2D> enemies = Physics2D.OverlapCircleAll(DetectEnemy.position, DetectEnemyRange, EnemyLayers).ToList();
            bool isEnemyDetected =  enemies.Any();
            TimeSinceAttack += Time.deltaTime;

            if(IsAttacking == false && TimeSinceAttack > TimeBetweenAttack && (isEnemyDetected || CountAttack == 1))
            {
                //look direction
                var target = enemies.FirstOrDefault(x => x.tag == TagName.Player);
                if(target != null)
                {
                    Vector2 direction = ((Vector2)target.transform.position - Rb.position).normalized;
                    AnimatorController.FilpCharacter(direction);
                }

                CountAttack++;
                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 2 || TimeSinceAttack > TimeBetweenAttack + TimeBetweenNextMove)
                {
                    CountAttack = 1;
                }
                
                AnimatorController.TriggerAttack(CountAttack);

                //Reset
                TimeSinceAttack = 0f;
                DamageFrameCount = 0;
            }

            if(IsAttacking && DamageFrameCount == 0)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = new List<Collider2D>();
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(EnemyLayers);
                HitBox.OverlapCollider(contactFilter, hitEnemies);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    hitEnemy.GetComponent<CharacterStatus>().TakeDamage(Damage, HitBox.gameObject);
                    DamageFrameCount++;
                }
            }
            else if(IsAttacking == false)
            {
                DamageFrameCount = 0;
            }
        }

        private void SetIsAttacking(int? countAttack)
        {
            IsAttacking = false;
            if(countAttack != null && countAttack > 0)
            {
                IsAttacking = AnimationState.IsName($"{AnimationName.Attack}{countAttack}");
            }
        }

        private float GetCurrentAttackingTime(int? countAttack)
        {
            float result = 0f;
            var clip = GetCurrentAttackingClip(countAttack);
            if(clip != null)
            {
                result = clip.length * AnimationState.normalizedTime;
            }

            return result;
        }

        private AnimationClip GetCurrentAttackingClip(int? countAttack)
        {
            AnimationClip result = new AnimationClip();
            if(countAttack != null && countAttack > 0)
            {
                var clip = AnimatorClip[0].clip;
                var animName = $"{AnimationName.Attack}{countAttack}";
                if(clip.name == animName)
                {
                    result = clip;
                }
            }
            return result;
        }

        #endregion

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DetectEnemy.position, DetectEnemyRange);
        }
    }
}
