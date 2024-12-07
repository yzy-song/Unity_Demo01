using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonSoundToScenesFolder : MonoBehaviour
{
    [MenuItem("Tools/Add Button Sound to Scenes in Assets/Scenes")]
    private static void AddSoundToButtonsInScenesFolder()
    {
        // 指定要处理的目录
        string targetFolder = "Assets/Scenes";

        // 获取目标目录中的所有场景路径
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { targetFolder });

        if (sceneGuids.Length == 0)
        {
            Debug.LogWarning($"No scenes found in {targetFolder}");
            return;
        }

        // 保存当前打开的场景路径
        string currentScenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;

        // 遍历目标场景
        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);

            EditorUtility.DisplayProgressBar("Adding Button Sound", $"Processing {scenePath}", 0.5f);

            // 打开场景
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);

            // 查找场景中的所有按钮
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (Button button in buttons)
            {
                // 如果按钮没有挂载 ButtonSound 脚本，则添加
                ButtonSound buttonSound = button.GetComponent<ButtonSound>();
                if (buttonSound == null)
                {
                    buttonSound = button.gameObject.AddComponent<ButtonSound>();
                }

                // 自动加载默认音效
                buttonSound.LoadDefaultSound();
            }

            // 保存场景
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        // 恢复到最初的场景
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(currentScenePath);

        EditorUtility.ClearProgressBar();
        Debug.Log($"Button Sound script has been added to all buttons in scenes under {targetFolder}.");
    }
}
