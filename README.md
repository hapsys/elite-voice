# ED scoring events
## Предназначанеи:
Программа яаляется обработчиком событий журнала пилота игры Elite: Dangerous. Она автоматоматически отслеживает изменения в последнем файле журнала и обрабатывает события в нем, в соответствии с файлом конфигурации (***config\\config.xml***)
###### Что может делать программа, реагируя на события:

1. Прогрывать звуковые файлы с синхронном и асинхронном режимах. Останавливать проигрывание в асинхронном режиме.
2. Проговаривать текст из игры и фразы, определенные пользователем.
3. Изменять голос произношения, его громкость и скорость.
4. В соответствии с параметрами события, принимать то или другое решение.
5. Случайным образом принимать то или другое решение.

## Установка програмы:
1. Разворачиваем архив из https://yadi.sk/d/_eCcY7lRy883R в папку на компьютере.
1. Качаем и устанавливаем синтезаторы голосов http://www.programs74.ru/rhvoice.html (устанавливаем саму программу, языки русский и английский и, затем, сами русские/английские голоса).
1. Копируем содержимое папки **appdata** в **%APPDATA%** на компьютере (обчно это ***С:\\Users\\\<USER\>\\AppData\\Roaming\\***). Это позволит немного поиграться с синтезируемой речью. Более подрубную информацию можно найти тут: https://github.com/Olga-Yakovleva/RHVoice/wiki/Configuration-file-%28Russian%29
1. Запускаем программу **EliteVoice.exe**. Обработка началась. (Программу, без зазрения совести, можно запускать/останавливать в любой момент)

## Синтаксис конфигурации (config\\config.xml):

Элемент | Атрибуты | Описание
------- | -------- | --------
Configuration | | Корневой элемент конфигурации
Init | | Секция инициализации программы
Replace | | Устанавливает параметры замены/добавления параметра события и/или замена произносимого текста
Event | **name** - имя события | Секция события в игре. Все дочерние команды выполняются при возникновении данного события.
Play | **name** - имя проигрывателя<br>**file** - имя звукового файла<br>**volume**[0..100] - уровень громкости<br>**async**[true\|**false**] - асихронное вопроизведение | Проиграть звуковой файл
Stop | **name** - имя проигрывателя<br>**fade** - время затухания в миллисекундах| Остановить проигрыватель звукового файла. Если параметр **name** отсутствует, то останавливаются все проигрыватели.
TextToSpeech | **voice** - имя голоса озвучки<br>**volume**[0..100] - уровень громкости<br>**pitch**[-10..10] - скорость произношения<br> | Установить параметры произношения текста.
Text | **select** - название параметра события | Произнести текст из параметра события или взять его из содержимого элемента.
Pause | **value** - время остановки в миллисекундах | Приостановить выполнение комманд на **value** миллисекунд.
Randomize | | Выбрать одну из дочерних команд случайным образом.
Block | **priority**[**1**..] - приоритет выбора блока, чем выше, тем больше вероятность | Блок команд, для элемента Randomize
Switch | **select** - название параметра события | Выбор параметра для уловия выбора в дочерних элементах Case/Default
Case | **match\|imatch** - соотвествует регулярному выражению<br>**equal\|iequal** - полное соответствие  | Проверка условия значения параметра, заданного элементом Switch. Префикс **i** у атрибута означает, что сравнение будет регистронезависимым.
Default | | Условие по умолчанию в блоке Switch. Обязательно должен быть **последним** элементом.
 

