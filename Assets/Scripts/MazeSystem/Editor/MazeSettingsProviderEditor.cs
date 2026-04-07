using UnityEditor;
using UnityEngine;
using MazeSystem.Unity;
using MazeSystem.Core;

namespace MazeSystem.Editor
{
    /// <summary>
    /// Кастомный Inspector для <see cref="MazeSettingsProvider"/>.
    /// Позволяет:
    /// - задавать значения с учётом ограничений (min/max)
    /// - автоматически ограничивать ввод (Clamp)
    /// - сбрасывать отдельные параметры к значениям по умолчанию
    /// - сбрасывать все параметры одной кнопкой (Reset All)
    /// - визуально отключать кнопки, если значения уже равны дефолтным
    /// </summary>
    [CustomEditor(typeof(MazeSettingsProvider))]
    [CanEditMultipleObjects]
    public class MazeSettingsProviderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Синхронизация serializedObject с текущими данными объекта
            serializedObject.Update();
            
            // ===== Tilemap =====
            EditorGUILayout.LabelField("Tilemap", EditorStyles.boldLabel);
            DrawIntField("tilemapRows", "Rows",
                MazeSettings.MinTilemapRows,
                MazeSettings.MaxTilemapRows,
                MazeSettings.DefaultTilemapRows);

            DrawIntField("tilemapCols", "Cols",
                MazeSettings.MinTilemapCols,
                MazeSettings.MaxTilemapCols,
                MazeSettings.DefaultTilemapCols);

            EditorGUILayout.Space();

            // Получение текущих значений для зависимых ограничений
            int tilemapRows = GetInt("tilemapRows");
            int tilemapCols = GetInt("tilemapCols");

            // ===== Maze Start Position =====
            EditorGUILayout.LabelField("Maze Start Position", EditorStyles.boldLabel);
            DrawIntField("mazeStartRow", "Start Row",
                MazeSettings.MinMazeStartRow,
                tilemapRows - MazeSettings.MinMazeRows,
                MazeSettings.DefaultMazeStartRow);

            DrawIntField("mazeStartCol", "Start Col",
                MazeSettings.MinMazeStartCol,
                tilemapCols - MazeSettings.MinMazeCols,
                MazeSettings.DefaultMazeStartCol);

            EditorGUILayout.Space();

            // Получение текущих значений для зависимых ограничений
            int mazeStartRow = GetInt("mazeStartRow");
            int mazeStartCol = GetInt("mazeStartCol");

            // ===== Maze Size =====
            EditorGUILayout.LabelField("Maze Size", EditorStyles.boldLabel);
            DrawIntField("mazeRows", "Maze Rows",
                MazeSettings.MinMazeRows,
                tilemapRows - mazeStartRow,
                MazeSettings.DefaultMazeRows);

            DrawIntField("mazeCols", "Maze Cols",
                MazeSettings.MinMazeCols,
                tilemapCols - mazeStartCol,
                MazeSettings.DefaultMazeCols);

            EditorGUILayout.Space();
            
            // Получение текущих значений для зависимых ограничений
            int mazeRows = GetInt("mazeRows");
            int mazeCols = GetInt("mazeCols");

            // ===== Safe Zone =====
            
            // Вычисление максимально допустимого радиуса safe zone
            int maxAllowedRadius = Mathf.Min(
                MazeSettings.MaxSafeZoneSquareRadius,
                Mathf.Max(
                    MazeSettings.MinSafeZoneSquareRadius,
                    (int)(Mathf.Min(mazeRows, mazeCols) * MazeSettings.SafeZoneFactor)
                )
            );

            EditorGUILayout.LabelField("Safe Zone", EditorStyles.boldLabel);
            DrawIntField("safeZoneSquareRadius", "Radius",
                MazeSettings.MinSafeZoneSquareRadius,
                maxAllowedRadius,
                MazeSettings.DefaultSafeZoneSquareRadius);

            EditorGUILayout.Space();

            // ===== Reset All =====
            
            // Проверка, равны ли все значения значениям по умолчанию
            bool isAllDefault =
                GetInt("tilemapRows") == MazeSettings.DefaultTilemapRows &&
                GetInt("tilemapCols") == MazeSettings.DefaultTilemapCols &&
                GetInt("mazeStartRow") == MazeSettings.DefaultMazeStartRow &&
                GetInt("mazeStartCol") == MazeSettings.DefaultMazeStartCol &&
                GetInt("mazeRows") == MazeSettings.DefaultMazeRows &&
                GetInt("mazeCols") == MazeSettings.DefaultMazeCols &&
                GetInt("safeZoneSquareRadius") == MazeSettings.DefaultSafeZoneSquareRadius;

            // Кнопка неактивная, если всё уже дефолтное
            EditorGUI.BeginDisabledGroup(isAllDefault);

            // Деактивация кнопки, если всё уже в дефолтном состоянии
            if (GUILayout.Button("Reset All"))
            {
                foreach (var t in targets)
                {
                    var provider = (MazeSettingsProvider)t;
                    provider.ResetToDefault();
                    // Сообщение Unity, что объект изменился (важно для сохранения)
                    EditorUtility.SetDirty(provider);
                }
            }

            EditorGUI.EndDisabledGroup();

            // Применение изменения обратно в объект
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Отрисовывает поле ввода int с:
        /// - ограничением min/max
        /// - кнопкой сброса к дефолтному значению
        /// - отключением кнопки, если значение уже дефолтное
        /// </summary>
        private void DrawIntField(string propertyName, string label, int min, int max, int defaultValue)
        {
            var prop = serializedObject.FindProperty(propertyName);

            EditorGUILayout.BeginHorizontal();

            int newValue = EditorGUILayout.IntField(label, prop.intValue);
            prop.intValue = Mathf.Clamp(newValue, min, max);

            bool isDefault = prop.intValue == defaultValue;

            EditorGUI.BeginDisabledGroup(isDefault);
            if (GUILayout.Button("↺", GUILayout.Width(30)))
            {
                prop.intValue = defaultValue;
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Получает текущее значение int из SerializedObject.
        /// Используется для расчёта зависимых ограничений.
        /// </summary>
        private int GetInt(string propertyName)
        {
            return serializedObject.FindProperty(propertyName).intValue;
        }
    }
}