using Scripts.Enums;
using Scripts.Keys;
using Scripts.Unit;
using UnityEngine;
using Zenject;

namespace Scripts.Soldier
{
    public class SoldierRanged : SoldierBase, IPoolable<int,UnitTeams,SoldierTypes,IMemoryPool>
    {
        private ActionParams _actionParams;
        private IMemoryPool _pool;


        public override void Attack(UnitActionBase enemy)
        {
            base.Attack(enemy);
            _actionParams.ActionTypes = actionType;
            _actionParams.Impact = actionImpact*Time.deltaTime;
            _actionParams.Radius = actionRadius;
            enemy.ActionReceived(_actionParams);
        }

        public override void Defeat()
        {
            base.Defeat();
            _pool.Despawn(this);
        }

        public void OnDespawned()
        {
        }

        public void OnSpawned(int p1, UnitTeams p2, SoldierTypes p3, IMemoryPool p4)
        {
            active = true;
            health = p1*100f;
            unitTeams = p2;
            soldierType = p3;
            _pool = p4;
            Reset();
        }
        
        public class Factory : PlaceholderFactory<int, UnitTeams, SoldierTypes, SoldierRanged>
        {
            
        }
    }
}