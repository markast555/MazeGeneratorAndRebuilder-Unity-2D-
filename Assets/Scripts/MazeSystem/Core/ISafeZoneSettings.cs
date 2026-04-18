namespace MazeSystem.Core
{
    /// <summary>
    /// Интерфейс для безопасной зоны
    /// </summary>
    public interface ISafeZoneSettings
    {
        SafeZoneMode Mode { get; }
    }
}