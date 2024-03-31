using UnityEngine;

namespace Scripts.Models
{
    public class GameModel
    {
        private const string LevelNumberString = "LevelNumber";
        private const string TowerNumberString = "TowerNumber";
        private const string TowerUpgradeString = "TowerUpgrade";
        private const string CoinNumberString = "CoinNumber";
        public int CoinNumber
        {
            get
            {
                return PlayerPrefs.GetInt(CoinNumberString, 100);
            }
            set
            {
                PlayerPrefs.SetInt(CoinNumberString,value);
                PlayerPrefs.Save();
            }
        }
        public int LevelNumber
        {
            get
            {
                return PlayerPrefs.GetInt(LevelNumberString, 0);
            }
            set
            {
                PlayerPrefs.SetInt(LevelNumberString,value);
                PlayerPrefs.Save();
            }
        }
        public int TowerNumber
        {
            get
            {
                return PlayerPrefs.GetInt(TowerNumberString, 1);
            }
            set
            {
                PlayerPrefs.SetInt(TowerNumberString,value);
                PlayerPrefs.Save();
            }
        }

        public int TowerUpgrade(int id,int levelSave = -1)
        {
            if (levelSave == -1)
            {
                return PlayerPrefs.GetInt(TowerUpgradeString+id, 1);
            }
            else
            {
                PlayerPrefs.SetInt(TowerUpgradeString+id,levelSave);
                PlayerPrefs.Save();
            }

            return levelSave;
        }
        
    }
}