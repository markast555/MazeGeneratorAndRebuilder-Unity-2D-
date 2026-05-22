namespace MazeSystem.Core
{
    /// <summary>
    /// Определяет настройки безопасной зоны
    /// </summary>
    public interface ISafeZoneSettings
    {
        SafeZoneMode Mode { get; }
    }
}