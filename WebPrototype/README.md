# Browser prototype + GitHub Pages

Локализованный браузерный прототип Token Agent (RU/EN), который можно запустить без Unity.

## Локальный запуск

```bash
python -m http.server 8080
```

Открыть:

- `http://localhost:8080/WebPrototype/`

## Публикация на GitHub Pages (чтобы запустить прямо сейчас)

В репозитории уже добавлен workflow: `.github/workflows/deploy-webprototype-pages.yml`.

Сделайте один раз:

1. Запушьте изменения в GitHub (ветка `main` или `master`).
2. В GitHub: **Settings → Pages**.
3. В `Build and deployment` выберите **Source = GitHub Actions**.
4. Перейдите во вкладку **Actions** и дождитесь выполнения `Deploy WebPrototype to GitHub Pages`.

После успешного деплоя откройте:

- `https://<your-github-username>.github.io/<repo-name>/WebPrototype/`

> Пример: `https://octocat.github.io/AI-agent-simulator/WebPrototype/`

## Что локализовано

- Заголовки UI
- Кнопки
- Сообщения успеха/провала
- Системные тексты энергии/ранга

Переключатель языка находится в верхней части экрана.
