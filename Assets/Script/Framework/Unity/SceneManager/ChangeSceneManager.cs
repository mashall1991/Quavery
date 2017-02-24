using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///场景切换时的信息保存
/// </summary>
public enum SceneChangeInfoSaveType
{
    None,
    EasyLobbyGameExit,
    SNGGameExit,
}

namespace Framework.Unity
{
    /// <summary>
    /// 切换场景的管理类
    /// </summary>
    class ChangeSceneManager : Framework.Singleton<ChangeSceneManager>
    {
        /// <summary>
        /// 记录进入Loading场景后下一个要跳转到的场景
        /// </summary>
        public string NextSceneName { get; set; }

        /// <summary>
        /// 切换时的信息保存，当信息使用之后必须置空
        /// </summary>
        public SceneChangeInfoSaveType SaveType = SceneChangeInfoSaveType.None; 

        /// <summary>
        /// 通过Loading场景跳转到新的场景
        /// </summary>
        public void ChangeToScene(string sceneName, string LodingSceneName = "LodingScene")
        {
            // 保存要跳转到的场景
            NextSceneName = sceneName;
            SceneManager.LoadScene(LodingSceneName);
        }
    }
}
