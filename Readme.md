# PikTools API framework
**PikTools API framework** - набор базовых пакетов для построения каркаса плагина Autodesk Revit.
В состав входят:
- _PikTools.Application.Api_ - содержит базовые классы для создания Application плагинов Revit.
- _PikTools.Command.Api_ - содержит базовые классы для создания Command плагинов Revit.
- _PikTools.Di_ - содержит базовые классы для настройки DI контейнера.
- _PikTools.Logs_ - содержит расширения для добавления к плагинам логирования.
## PikTools.Di
В качестве DI контейнера используется [**SimpleInjector**](https://simpleinjector.org/)
В пакете присутствуют базовые абстракции, используемые пакетами _PikTools.Application.Api_, _PikTools.Command.Api_, _PikTools.Logs_.