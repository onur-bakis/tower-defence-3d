using Scripts.Keys;

namespace Scripts.Signals
{
    public struct OnTowerUpgrade{}
    public struct OnFireBallClick{}
    public struct OnIceBallClick{}
    public struct OnSoldierMeleeClick{}
    public struct OnSoldierRangedClick{}
    public struct OnSoldierBomberClick{}
    public struct OnReloadClick{}
    public struct OnTowerUnlock{}
    public struct OnStartUIUpdate{}

    public struct OnEnemyKilled {}
    public struct OnTowerDestroyed {}


    public struct OnProgressBarUpdated
    {
        public int waveNumber;
        public float value;
    }

    public struct OnCoinUpdated
    {
        public int amount;
    }

    public struct OnLevelStart
    {
        public int levelNumber;
    }

    public struct OnLevelEnd
    {
        public bool Win;
    }
    public struct OnTapSignal
    {
        public InputDataParams InputDataParams;
    }
}