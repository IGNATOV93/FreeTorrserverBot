# Telegram Bot For Torrserver

Данный проект — бот для Telegram, который включает функции автосмены пароля [Torrserver](https://github.com/YouROK/TorrServer), а также возможность ручной смены пароля через бота.

## Установка

Для корректной работы внутри проекта необходимо создать файл `settings.ini` с следующими полями:

```ini
[Profile0]
YourBotTelegramToken=ваш токен бота
AdminChatId=ваш id чата в Telegram
TimeAutoChangePassword=время автосмены пароля, например 23:00
FilePathTorrserverBd=/opt/torrserver/accs.db   ; путь к настройкам torrserver
FilePathTor=/opt/torrserver/torrserver         ; путь к самому torrserver
```

## Как запустить

1. **Скачайте архив проекта**:
   - Перейдите на [страницу релизов](https://github.com/ваш_репозиторий/releases) и скачайте последний архив.

2. **Распакуйте архив**:
   - Используйте команду:
     ```bash
     unzip ваш_архив.zip
     ```

3. **Убедитесь, что установлен .NET 8**:
   - Для работы проекта требуется .NET 8. Установите его, следуя [инструкциям по установке](https://docs.microsoft.com/ru-ru/dotnet/core/install/linux).

4. **Запустите проект в фоне**:
   - Используйте команду:
     ```bash
     screen -S ftor
     ```
   - После этого перейдите в папку с проектом:
     ```bash
     cd /opt/FreeTorrBot/
     ```
   - И запустите бота:
     ```bash
     ./FreeTorrBot
     ```
