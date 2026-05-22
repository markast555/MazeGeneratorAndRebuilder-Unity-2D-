using MazeSystem.Core;

namespace MazeSystem.Generation
{
    /// <summary>
    /// Определяет функциональность генератора безопасной зоны
    /// </summary>
    public interface ISafeZoneGenerator
    {
        /// <summary>
        /// Формирует безопасную зону
        /// </summary>
        /// <param name="context">Контекст формирования безопасной зоны</param>
        /// <param name="settingsSafeZone">Настройки безопасной зоны</param>
        /// <param name="listener">Слушатель событий безопасной зоны</param>
        /// <returns>Сформированная безопасная зона</returns>
        SafeZone Generate(
            SafeZoneContext context, 
            ISafeZoneSettings settingsSafeZone,
            ISafeZoneListener listener = null
            );
    }
}