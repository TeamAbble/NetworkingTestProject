using Eflatun.SceneReference;
using UnityEngine;

public class SceneContainer : MonoBehaviour
{
    public static SceneContainer Instance;

    public SceneReference mainMenu;
    public class SceneTarget
    {
        /// <summary>
        /// Some levels can be composed of multiple scenes. To account for this, we'll ask for a list of game scenes.
        /// </summary>
        public SceneReference[] gameScene;
        public Sprite loadScreenImage;
        public string levelName;
    }
    



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

}
