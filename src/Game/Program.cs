using OpenTK.Windowing.Desktop;

var gameWindowSettings = new GameWindowSettings()
{
    RenderFrequency = 144,
    UpdateFrequency = 144
};

var nativeWindowSettings = new NativeWindowSettings()
{
    Size = new OpenTK.Mathematics.Vector2i(1920, 1080),
    Title = "Test",
    APIVersion = new Version(4, 5)
};

using var game = new Game.Game(gameWindowSettings, nativeWindowSettings);

game.Run();