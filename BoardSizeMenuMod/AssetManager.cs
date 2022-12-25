using System.IO;
using System.Linq;
using UnityEngine;

namespace BoardSizeAndStanceMod
{
    class AssetManager
    {
        private GameObject _menuGameObject;
        public GameObject instantiatedMenu;
        public MenuBuilder menuBuilder;

        public void Init()
        {
            AssetBundle modDataBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetFullPath("."), "UserData/boardsizeandstancemod_bundle"));
            if (modDataBundle)
            {
                //MelonLogger.Msg("Asset bundle loaded.");
                var menuAsset = modDataBundle.LoadAsset("BoardSizeMod_Canvas");
                _menuGameObject = menuAsset.Cast<GameObject>();
                ModLogger.Log("MenuObject loaded.");
            }
        }

        public void CreateMenu()
        {
            instantiatedMenu = GameObject.Instantiate(_menuGameObject);
            menuBuilder = instantiatedMenu.AddComponent<MenuBuilder>();
            UnityEngine.Object.DontDestroyOnLoad(instantiatedMenu);
            instantiatedMenu.SetActive(false);
        }
    }
}
