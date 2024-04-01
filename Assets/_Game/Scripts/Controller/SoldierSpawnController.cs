using Scripts.Controller.Soldier;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Controller
{
    public class SoldierSpawnController
    {
        private SoldierMelee.Factory _soldierMeleeFactory;
        private SoldierRanged.Factory _soldierRangedFactory;
        private SoldierBomber.Factory _soldierBomberFactory;

        private Vector3 _spawnPosition;
        
        public SoldierSpawnController(SoldierMelee.Factory soldierMeleeFactory
            ,SoldierRanged.Factory soldierRangedFactory
            ,SoldierBomber.Factory soldierBomberFactory)
        {
            _soldierMeleeFactory = soldierMeleeFactory;
            _soldierRangedFactory = soldierRangedFactory;
            _soldierBomberFactory = soldierBomberFactory;

            _spawnPosition = new Vector3(0, 1.5f, 5f);
        }
        
        public void OnSoldierMeleeClick()
        {
            DeploySoldiers(SoldierTypes.SoldierMelee,UnitTeams.TeamDefend);
        }
        public void OnSoldierRangedClick()
        {
            DeploySoldiers(SoldierTypes.SoldierRanged,UnitTeams.TeamDefend);
        }
        public void OnSoldierBomberClick()
        {
            DeploySoldiers(SoldierTypes.SoldierBomber,UnitTeams.TeamDefend);
        }
        
        private void DeploySoldiers(SoldierTypes soldierTypes,UnitTeams unitTeams)
        {
            switch (soldierTypes)
            {
                case SoldierTypes.SoldierMelee:
                    SoldierMelee soldierMelee = _soldierMeleeFactory.Create(1, unitTeams, SoldierTypes.SoldierMelee);
                    soldierMelee.transform.position = 
                        _spawnPosition+Vector3.right*Random.Range(-2f,2f);
                    break;
                case SoldierTypes.SoldierRanged:
                    SoldierRanged soldierRanged = _soldierRangedFactory.Create(1, unitTeams, SoldierTypes.SoldierRanged);
                    soldierRanged.transform.position = 
                        _spawnPosition+Vector3.right*Random.Range(-2f,2f);
                    break;
                case SoldierTypes.SoldierBomber:
                    SoldierBomber soldierBomber = _soldierBomberFactory.Create(1, unitTeams, SoldierTypes.SoldierBomber);
                    soldierBomber.transform.position = 
                        _spawnPosition+Vector3.right*Random.Range(-2f,2f);
                    break;
                default:
                    break;
            }
        }
    }
}