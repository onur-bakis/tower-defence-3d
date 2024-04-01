using Scripts.Enums;
using Scripts.Keys;
using UnityEngine;

namespace Scripts.Controller.Unit
{
    public class UnitActionBase : MonoBehaviour
    {
        public float health;
        public bool active;
        public float range = 5f;
        public float size = 1f;

        public UnitTeams unitTeams;
        private UnitActionBase _cacheUnitActionBaseOther;
        private ActionParams _cacheActionParams;
        
        //When an action is received from other UnitActionBases
        public virtual void ActionReceived(ActionParams actionParams) { }
        
        //When sending actions to other base object
        public virtual void TakeAction(UnitActionBase enemy) { }

        public void Init(UnitTeams unitTeam)
        {
            this.unitTeams = unitTeam;
        }

        public void Reset(float health)
        {
            this.health = health;
        }

        public virtual void Defeat()
        {
            if(!active)
                return;
            
            active = false;
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("UnitActionBase"))
            {
                _cacheUnitActionBaseOther = other.gameObject.GetComponent<UnitActionBase>();
            }
            else
            {
                return;
            }

            if (_cacheUnitActionBaseOther.unitTeams == unitTeams)
                return;
            
            TakeAction(_cacheUnitActionBaseOther);
        }
        

    }
}