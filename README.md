# Telegram Bot AdTorrBot

Данный проект — бот для Telegram, который включает функции автосмены пароля [Torrserver](https://github.com/YouROK/TorrServer), а также возможность ручной смены пароля через бота.

## Установка

Для корректной работы внутри проекта необходимо отредактировать файл `settings.json` с следующими полями:

```json

{
"YourBotTelegramToken": "ваш токен бота"
"AdminChatId": "ваш id чата в Telegram"
"FilePathTorrserver": "путь к настройкам torrserver-пример-"/opt/torrserver/"
}
```
1. **Скачайте архив проекта**:
   - Скачайте тут архив с проектом [FreeTorrBot-linux-64.rar](https://github.com/IGNATOV93/FreeTorrserverBot/releases/tag/v1.01).

2. **Распакуйте архив**:
   - Используйте команду:
     ```bash
     sudo unrar x FreeTorrBot-linux-64.rar /opt/
     ```

3. **Установите .NET 8**:
   - Выполните следующие команды:
     ```bash
     sudo apt-get install -y wget apt-transport-https
     wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb
     sudo dpkg -i packages-microsoft-prod.deb
     sudo apt-get update
     sudo apt-get install -y dotnet-sdk-8.0
     ```
   - Проверьте установку:
     ```bash
     dotnet --version
     ```
4. **Установите права на доступ**:
   - Установите права на чтение для файлов в папке бота:
     ```bash
     sudo chmod +r /opt/AdTorrBot /opt/AdTorrBot/settings.json /opt/AdTorrBot/autoStartFreeTorrBot.sh
     ```
   - Установите права на чтение и запись для файла `accs.db` в папке Torrserver:
     ```bash
     sudo chmod 664 /opt/torrserver/accs.db
     ```
   - Установите права на чтение для файла `TorrServer-linux-amd64` в папке Torrserver:
     ```bash
     sudo chmod +r /opt/torrserver/TorrServer-linux-amd64
     ```
5. **Установите `unrar` и `screen`** (если они не установлены):
   - Установите `unrar`:
     ```bash
     sudo apt-get install -y unrar
     ```
   - Установите `screen`:
     ```bash
     sudo apt-get install -y screen
     ```


## Как запустить

1. **Запустите проект в фоне через screen**
   - Используйте команду:
     
    screen -S adtorrbot cd /opt/AdTorrBot/./AdTorrBot

   -Свернуть окно screen (без этого бот остановится сразу при выходе).
   ```bash
     Ctrl+ A+D
   ```
   
3. **Или автоматический запуск бота и при перезагрузке**
Добавление скрипта в `crontab`

Выполните команду:
```bash
crontab -e
```
Добавьте в `crontab`:
```bash
@reboot /opt/AdTorrBot/autoStartFreeTorrBot.sh
 ```
<details>
<summary>Основные команды для работы с `screen`</summary>

- **Создание нового окна**:
  - `screen -S <имя_сессии>` — создаёт новую сессию с указанным именем.

- **Запуск существующей сессии**:
  - `screen -r <имя_сессии_или_id>` — восстанавливает существующую сессию.

- **Свернуть сессию в фоне**:
  - `Ctrl + A` затем `D` — сворачивает текущую сессию в фоне.

- **Просмотр списка сессий**:
  - `screen -ls` — отображает список активных сессий `screen`.

</details>



