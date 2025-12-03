using Godot;
using ServiceSystem;

public class SceneManager : IService {
    private SceneRepository _sceneRepository;

    public SceneManager(SceneRepository sceneRepository) {
        _sceneRepository = sceneRepository;
    }

    public void ChangeScene(SceneId sceneId) {
        (Engine.GetMainLoop() as SceneTree)?.ChangeSceneToPacked(_sceneRepository.GetPackedScene(sceneId));
    }
}