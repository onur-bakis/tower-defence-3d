using System.Collections.Generic;
using Scripts.Controller.Unit;
using Scripts.Enums;
using Scripts.Keys;
using Scripts.Manager;
using UnityEngine;
using Zenject;

namespace Scripts.Controller.Turret
{
    public class TurretHead : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystemSelect;
        [SerializeField] private ParticleSystem _particleSystemUpgrade;

        private float projectileTimeInterval = 5f;
        private float lastProjectileTime;
        private Queue<UnitActionBase> _stack;
        private UnitActionBase _currentEnemy;
        private float attackInterval = 1f;
        private float lastAttackTime;
        public float upgradeLevel;

        [Inject] 
        private ProjectileManager ProjectileManager;
        
        private void Start()
        {
            _stack = new Queue<UnitActionBase>();
            lastAttackTime = Time.time;
            lastProjectileTime = Time.time;
        }

        private void Update()
        {
            if(_stack.Count == 0)
                return;
            
            if (_currentEnemy == null || !_currentEnemy.active)
            {
                _currentEnemy = _stack.Dequeue();
                return;
            }
            
            if (Time.time-lastAttackTime > attackInterval)
            {
                AttackEnemy();
                lastAttackTime = Time.time;
            }
            
            if(upgradeLevel < 3)
                return;
            
            if (Time.time-lastProjectileTime > projectileTimeInterval)
            {
                SendProjectileToEnemy();
                lastProjectileTime = Time.time;
            }
            
        }

        private void SendProjectileToEnemy()
        {
            ProjectileManager.SendProjectile(ProjectileType.Tower,transform.position,_currentEnemy.transform.position);
        }

        public void AttackEnemy()
        {
            ActionParams actionParams = new ActionParams();
            actionParams.ActionTypes = ActionTypes.Attack;
            actionParams.Impact = 20f*upgradeLevel;
            _currentEnemy.ActionReceived(actionParams);
        }
        public void Attack(UnitActionBase enemy)
        {
            if(!_stack.Contains(enemy))
                _stack.Enqueue(enemy);
        }


        public void DeSelect()
        {
            _particleSystemSelect.Stop();
        }

        public void Select()
        {
            _particleSystemSelect.Play();
        }

        public void SetUpgrade(float level,bool upgrade)
        {
            upgradeLevel = level;
            if(upgrade)
                _particleSystemUpgrade.Play();
        }
    }
}