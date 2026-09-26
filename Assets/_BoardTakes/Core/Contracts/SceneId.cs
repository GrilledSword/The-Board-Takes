namespace BoardTakes.Core
{
    /// <summary>
    /// Stable scene tokens. Paths live in <see cref="SceneCatalog"/>.
    /// Never pass raw scene strings through gameplay code.
    /// </summary>
    public enum SceneId
    {
        None = 0,
        Boot = 1,
        Splash = 2,
        Intro = 3,
        MainMenu = 4,
        Settings = 5,
        MultiplayerLobby = 6,
        GameplayCore = 7,
        MapGeometry = 8,
        MapLightingPersistent = 9,
        EndingCutscene = 10,
        Credits = 11
    }
}
