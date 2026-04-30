# Token Agent Prototype (Unity / Android-ready)

## Что включено

- Структура проекта под Unity согласно инструкции.
- Базовые C#-скрипты для core loop:
  - загрузка миссий,
  - расход энергии,
  - сбор стикеров,
  - выбор финального ответа,
  - успех/провал и экран результата.
- JSON с примерами миссий: `Assets/Resources/Missions/missions.json`.

## Быстрый старт

1. Откройте Unity Hub и создайте/откройте проект (Unity 6 или LTS).
2. Скопируйте папку `Assets/` в Unity-проект.
3. Создайте сцену `GameScene`.
4. На пустой объект `GameRoot` повесьте:
   - `MissionManager`
   - `EnergyManager`
   - `StickerBoard`
5. Создайте объект робота (placeholder) и добавьте `RobotController`.
6. Создайте UI-панели и подключите `AnswerPanel` и `ResultPanel` в инспекторе.
7. Создайте станции-кнопки и добавьте `StationController` с разными `ActionId`.
8. Свяжите ссылки в `MissionManager` через инспектор.
9. Добавьте кнопку `Assemble Answer`, вызывающую `MissionManager.OpenAnswerSelection()`.

## Android настройки

- Build Settings -> Android -> Switch Platform.
- Player Settings:
  - Orientation: Landscape Left/Right
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64
  - Minimum API Level: по целевому устройству (рекомендуется API 26+)
- UI делайте крупным (Canvas Scaler: Scale With Screen Size).

## Ограничения

Этот прототип использует заглушечную визуализацию и не включает реальные внешние сервисы.
