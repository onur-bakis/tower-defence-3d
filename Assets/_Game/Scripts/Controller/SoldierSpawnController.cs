using Scripts.Enums;
using Scripts.Soldier;
using UnityEngine;

namespace Scripts.Controller
{
    public class SoldierSpawnController
    {
        private SoldierMelee.Factory _soldierMeleeFactory;
        private SoldierRanged.Factory _soldierRangedFactory;
        private SoldierBomber.Factory _soldierBomberFactory;
        
        public SoldierSpawnController(SoldierMelee.Factory soldierMeleeFactory
            ,SoldierRanged.Factory soldierRangedFactory
            ,SoldierBomber.Factory soldierBomberFactory)
        {
            _soldierMeleeFactory = soldierMeleeFactory;
            _soldierRangedFactory = soldierRangedFactory;
            _soldierBomberFactory = soldierBomberFactory;
        }
        
        public void OnSoldierMeleeClick()
        {
            DeploySoldiers(SoldierTypes.SoldierMelee,UnitTeams.TeamDefend,1);
        }
        public void OnSoldierRangedClick()
        {
            DeploySoldiers(SoldierTypes.SoldierRanged,UnitTeams.TeamDefend,1);
        }
        public void OnSoldierBomberClick()
        {
            DeploySoldiers(SoldierTypes.SoldierBomber,UnitTeams.TeamDefend,1);
        }
        
        private void DeploySoldiers(SoldierTypes soldierTypes,UnitTeams unitTeams,float delay)
        {
            switch (soldierTypes)
            {
                case SoldierTypes.SoldierMelee:
                    SoldierMelee soldierMelee = _soldierMeleeFactory.Create(1, unitTeams, SoldierTypes.SoldierMelee);
                    soldierMelee.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*Random.Range(0f,2f);
                    break;
                case SoldierTypes.SoldierRanged:
                    SoldierRanged soldierRanged = _soldierRangedFactory.Create(1, unitTeams, SoldierTypes.SoldierRanged);
                    soldierRanged.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*Random.Range(0f,2f);
                    break;
                case SoldierTypes.SoldierBomber:
                    SoldierBomber soldierBomber = _soldierBomberFactory.Create(1, unitTeams, SoldierTypes.SoldierBomber);
                    soldierBomber.transform.position = 
                        new Vector3(0, 1.5f, 8f)+Vector3.forward*4f*delay+Vector3.right*Random.Range(0f,2f);
                    break;
                default:
                    break;
            }
        }
    }
}