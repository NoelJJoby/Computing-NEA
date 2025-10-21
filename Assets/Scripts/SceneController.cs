using UnityEngine.SceneManagement;

public static class SceneController
{

	public enum SceneName
	{
		MainMenuScene,
		LevelSelectorScene,
		GameScene,
		LoadingScene
	}

	private static SceneName currentScene = SceneName.MainMenuScene;

	public static void MoveToScene(SceneName sceneToLoad)
	{
        currentScene = sceneToLoad;
        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }

	public static void LoaderCallback()
	{
        SceneManager.LoadScene(currentScene.ToString());
    }

}
