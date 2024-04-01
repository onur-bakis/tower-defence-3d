using Scripts.Controller.Unit;
using Scripts.Enums;
using Scripts.Keys;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Controller.Projectile
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _colliderCollision;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _particleSystemImpact;
        [SerializeField] private ParticleSystem _particleSystemEffect;
        
        
        public ActionTypes actionTypes = ActionTypes.Damage;
        public ProjectileType projectileType = ProjectileType.Fire;
        
        private float _effectRange = 5f;
        public float _effectDamage = 5f;
        private float _effectTime = 5f;
        private UnitTeams _unitTeams;
        public ActionParams actionParams;

        private bool impacted;

        
        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            _unitTeams = UnitTeams.TeamDefend;
            impacted = false;
            actionParams = new ActionParams();
            actionParams.Impact = _effectDamage;
            actionParams.Radius = _effectRange;
        }

        public void OnTriggerStay(Collider other)
        {
            if(!impacted || other.isTrigger)
                return;
            
            if (other.CompareTag("UnitActionBase"))
            {
                UnitActionBase unitActionBase = other.GetComponent<UnitActionBase>();
                if (unitActionBase.unitTeams == _unitTeams)
                {
                    return;
                }
                Damage(unitActionBase);
            }
        }

        public virtual void EffectFinished()
        {
            Destroy(gameObject);
        }

        public virtual void Impact()
        {
            transform.rotation = Quaternion.identity;
            _colliderCollision.enabled = false;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;

            _particleSystemImpact.Play();
            _particleSystemEffect.Play();
            impacted = true;
            
            Invoke(nameof(EffectFinished),_effectTime);
        }
        public virtual void Damage(UnitActionBase unitActionBase)
        {
        }
        
        public virtual void Move(Vector3 transformPosition)
        {
            transformPosition.y = 0f;
            transform.DOJump(transformPosition, 10, 1, 1f).OnComplete(Impact);
        }




    }
}