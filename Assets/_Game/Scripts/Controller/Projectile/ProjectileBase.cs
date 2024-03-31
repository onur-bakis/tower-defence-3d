using Scripts.Enums;
using Scripts.Keys;
using Scripts.Unit;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Projectile
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _colliderCollision;
        [SerializeField] private Rigidbody _rigidbody;
        
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

        public void OnCollisionEnter(Collision other)
        {
            Impact();
            Invoke(nameof(EffectFinished),_effectTime);
        }

        public void OnTriggerStay(Collider other)
        {
            if(!impacted)
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
            //_meshRenderer.enabled = false;
            _colliderCollision.enabled = false;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;

            impacted = true;
        }
        public virtual void Damage(UnitActionBase unitActionBase)
        {
        }
        
        public virtual void Move(Vector3 transformPosition)
        {
            //transform.DOMove(transformPosition, 1f);
            transform.DOJump(transformPosition, 5, 1, 1f);
        }




    }
}