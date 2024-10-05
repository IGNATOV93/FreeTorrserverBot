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
