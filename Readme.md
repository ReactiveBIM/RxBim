# PikTools API framework
**PikTools API framework** - набор базовых пакетов для построения каркаса плагина Autodesk Revit.
В состав входят:
- _PikTools.Application.Api_ - содержит базовые классы для создания Application плагинов Revit.
- _PikTools.Application.Ui.Api_ - содержит расширения для добавления к плагинам меню на ленту Revit.
- _PikTools.Command.Api_ - содержит базовые классы для создания Command плагинов Revit.
- _PikTools.Di_ - содержит базовые классы для настройки DI контейнера.
- _PikTools.Logs_ - содержит расширения для добавления к плагинам логирования.
- _PikTools.Analyzers_ - содержит анализаторы кода для плагинов.
- _PikTools.Shared.Ui_ - содержит стили, контролы, конвертеры и вспомогательные сервисы для разработки UI плагинов Revit.
- _PikTools.Nuke_ - плагин для Nuke, содержит Target для сборки MSI из произвольного проекта плагина Revit.

## PikTools.Di
В качестве DI контейнера используется [**SimpleInjector**](https://simpleinjector.org/)
В пакете присутствуют базовые абстракции, используемые пакетами _PikTools.Application.Api_, 
_PikTools.Command.Api_, _PikTools.Logs_, _PikTools.Shared.Ui_.

## PikTools.Nuke
####Порядок работы:
- Установить пакет - _`PM> Install BimLab.PikTools.Nuke`_  в проект ``_build``.
- Обновить класс ``Build``(должен наследовать класс ``PikToolsBuild``) - ``class Build : NukeBuild`` => ``class Build : PikToolsBuild``.
- После этого в плане выполнения появится ``Target`` ``BuildMsi``. Данный таргет полностью автоматизирует сборку MSI пакета, 
попутно проверяет все необходимые условия для сборки, выводя ошибки при их обнаружении. 

####Основные требования для сборки MSI пакета:
- В системе установлен .NET Framework 3.5
- В системе установлен ``PikTools.MsiBuilder.Bin`` и выставлена переменная окружения ``PIKTOOLS_MSIBUILDER_BIN``. 
Для сборки приложения в данном репозитории необходимо вызвать комманду ``nuke PublishMsiBuildTool`` 
 