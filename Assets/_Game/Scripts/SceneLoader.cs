using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

}
