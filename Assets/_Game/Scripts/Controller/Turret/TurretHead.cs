using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Keys;
using Scripts.Unit;
using UnityEngine;

namespace Scripts.Turret
{
    public class TurretHead : MonoBehaviour
    {
        private Queue<UnitActionBase> _stack;
        private UnitActionBase _currentEnemy;
        private float attackInterval = 1f;
        private float lastAttackTime;
        public float upgradeLevel;

        private void Start()
        {
            _stack = new Queue<UnitActionBase>();
            lastAttackTime = Time.time;
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
            
        }

        public void AttackEnemy()
        {
            ActionParams actionParams = new ActionParams();
            actionParams.ActionTypes = ActionTypes.Attack;
            actionParams.Impact = 150f+upgradeLevel;
            _currentEnemy.ActionReceived(actionParams);
        }
        public void Attack(UnitActionBase enemy)
        {
            _stack.Enqueue(enemy);
        }
        
        
    }
}