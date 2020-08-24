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
## PikTools.Di
В качестве DI контейнера используется [**SimpleInjector**](https://simpleinjector.org/)
В пакете присутствуют базовые абстракции, используемые пакетами _PikTools.Application.Api_, 
_PikTools.Command.Api_, _PikTools.Logs_, _PikTools.Shared.Ui_.

## Gitflow