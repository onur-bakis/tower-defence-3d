using Scripts.Enums;
using Scripts.Keys;
using Scripts.Unit;
using UnityEngine;
using Zenject;

namespace Scripts.Soldier
{
    public class SoldierBomber : SoldierBase, IPoolable<int,UnitTeams,SoldierTypes,IMemoryPool>
    {
        private ActionParams _actionParams;
        private IMemoryPool _pool;

        public override void Attack(UnitActionBase enemy)
        {
            base.Attack(enemy);
            
            _actionParams.ActionTypes = actionType;
            _actionParams.Impact = 175f;
            _actionParams.Radius = 5f;


            if (active)
            {
                Explosion(_actionParams.Radius);
                Defeat();
            }
        }

        public override void Defeat()
        {
            base.Defeat();
            _pool.Despawn(this);
        }
        
        public void Explosion(float range)
        {
            Vector3 p1 = transform.position;
            RaycastHit[] hit = new RaycastHit[]{};
            hit = Physics.SphereCastAll(p1, range,Vector3.down);
            foreach (var objects in hit)
            {
                GameObject objectsGO = objects.collider.gameObject;
        
                if (objectsGO.CompareTag("UnitActionBase"))
                {
                    UnitActionBase unitActionBase = gameObject.GetComponent<UnitActionBase>();
                    unitActionBase.ActionReceived(_actionParams);
                }
            }
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
        public class Factory : PlaceholderFactory<int, UnitTeams, SoldierTypes, SoldierBomber>
        {
            
        }
    }
}