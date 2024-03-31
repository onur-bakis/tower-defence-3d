using Scripts.Enums;
using Scripts.Unit;

namespace Scripts.Projectile
{
    public class FireBallProjectile : ProjectileBase
    {
        public override void Init()
        {
            base.Init();
            actionTypes = ActionTypes.Damage;
            projectileType = ProjectileType.Fire;
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