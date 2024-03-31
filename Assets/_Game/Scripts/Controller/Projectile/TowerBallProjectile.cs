using Scripts.Enums;
using Scripts.Unit;

namespace Scripts.Projectile
{
    public class TowerBallProjectile : ProjectileBase
    {
        public override void Init()
        {
            base.Init();
            actionTypes = ActionTypes.Damage;
            projectileType = ProjectileType.Tower;
            actionParams.ActionTypes = actionTypes;
        }
        public override void Damage(UnitActionBase unitActionBase)
        {
            base.Damage(unitActionBase);
            unitActionBase.ActionReceived(actionParams);
        }
        public override void EffectFinished()
        {
            base.EffectFinished();
        }
        public override void Impact()
        {
            base.Impact();
        }
    }
}