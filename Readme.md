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
- _PikTools.Shared.RevitExtensions_ - содержит расширения к API Revit и вспомогательные сервисы по работе с ним

## PikTools.Di
В качестве DI контейнера используется [**SimpleInjector**](https://simpleinjector.org/)
В пакете присутствуют базовые абстракции, используемые пакетами _PikTools.Application.Api_, 
_PikTools.Command.Api_, _PikTools.Logs_, _PikTools.Shared.Ui_.

## PikTools.Shared.RevitExtensions
### Подключение пакета
В конфигурации приложения вызовите расширение для контейнера
``container.AddRevitHelpers();``

### Колекторы 
#### IScopedElementsCollector
``public interface IScopedElementsCollector : IElementsCollector``

Предоставляет выбор элементов из документа Revit
- ``ScopeType Scope { get; }`` - текущая выбранная часть элементов
- ``void SetScope(ScopeType scope)`` - позволяет задать текущий выбор части элементов
- ``bool HasElements(Document doc)`` - выдает наличие элементов согласно текущей выбранной части
- ``void SaveSelectedElements()`` - сохраняет внутри коллектора текущие выбранные в модели элементы и снимает их выделение (это нужно, т.к. выделенные элементы блокируют контекст Revit)
- ``void SelectSavedElements()`` - возвращает ранее сохраненные выделенные элементы в моделе (рекомендуется вызывать в конце работы плагина)
- ``FilteredElementCollector GetFilteredElementCollector(Document doc, bool ignoreFilter = false)`` - выдает коллектор элементов согласно текущей выбранной части (при ignoreFilter = true выбранная часть игнорируется)
- ``void SelectElement(int id)`` - выбрать в моделе элеменет по Id
- ``void ZoomElement(int id)`` - приблизить элемент на виде
- ``Element PickElement(Func<Element, bool> filterElement = null, string statusPrompt = "")`` - включает в Revit режим выбора элемента с заданным фильтром (при отмене выдает null)

Пример использования:

    _scopedElementsCollector.SetScope(ScopeType.SelectedElements);
    if (!_scopedElementsCollector.HasElements(_document))
        return;

    // Если при выборе части ScopeType.SelectedElements не сделать сохранения, то будет выдано ArgumentException
    _scopedElementsCollector.SaveSelectedElements();

    // Если нет выбранных элементов, то возвращается null
    var sel = _scopedElementsCollector
        .GetFilteredElementCollector(_document)
        ?.OfClass(typeof(FamilyInstance))
        .Cast<FamilyInstance>()
        .ToList();

    // TODO: Do something...

    _scopedElementsCollector.SelectSavedElements();

#### IDocumentsCollector
``public interface IDocumentsCollector``

Предоставляет список связанных документов Revit
- ``string GetMainDocumentTitle()`` - выдает заголовок основного документа
- ``IEnumerable<string> GetDocumentsTitles()`` - выдает список заголовков основного документа и всех связанных с ним документов

#### ISheetsCollector
``public interface ISheetsCollector``

Предоставляет список листов из проекта Revit
- ``Dictionary<string, List<string>> GetSheets()`` - выдает список листов, сгруппированных по документам в проекте
- ``Dictionary<string, Dictionary<string, List<string>>> GetSheets(string groupSheetParam)`` - выдает списко листов, сгруппированных по документам и по значению заданного параметра листа ``groupSheetParam``
- ``IEnumerable<string> GetSelectedSheets()`` - выдает список выбранных в диспетчере проекта листов

### Хранилища 
#### IProblemElementsStorage
``public interface IProblemElementsStorage``

Хранилище проблемных элементов с описанием проблемы
- ``void AddProblemElement(int id, string problem)`` - добавляет Id проблемного элемента и описание ошибки в хранилище
- ``bool HasProblems()`` - есть ли проблемные элементы в хранилище
- ``IEnumerable<KeyValuePair<int, string>> GetProblems()`` - выдает список всех проблем из хранилища
- ``IDictionary<string, IEnumerable<int>> GetCombineProblems()`` - выдает скомбинированный по проблемам список id элементов
- ``void Clear()`` - очищает хранилище

Пример использования:

    _problemElementsStorage.Clear();

    _problemElementsStorage.AddProblemElement(
        element.Id.IntegerValue, $"У элемента отсутсвует параметр {paramName}");
    
    // Получаем набор проблем для вывода пользователю
    var problems = _problemElementsStorage.GetCombineProblems();

### Расширения
#### Расширения для документа Revit
- ``Document GetOpenedLinkedDocument(this Document rootDoc, string linkedDocTitle)`` - открывает связанный документ по заданному заголовку (null, если такого документа нет)
- ``void CloseLinkedDocument(this Document rootDoc, Document linkedDocument)`` - закрывает открытый связанный документ
- ``RevitLinkType GetLinkType(this Document rootDoc, string linkedDocTitle)`` - выдает типа связанного документа по его названию (null, если такого документа нет)
- ``IEnumerable<int> GetCategoriesIds(
            this Document doc,
            IEnumerable<int> excludeCategories,
            bool includeSubCategories = true)`` - выдает список идентификаторов категорий, имеющихся в проекте (если includeSubCategories = false, то подкатегории игнорируеются)
- ``IEnumerable<View> GetViews(
            this Document doc,
            IEnumerable<string> viewsNames,
            bool isOrder = true)`` - выдает список заданных по названию видов документа (isOrder = true, по-умолчанию упорядочивается по номеру листа)

#### Расширения для заголовка документа Revit
- ``string GetDocumentStage(this Document doc)`` - выдает стадию документа
- ``string GetDocumentDiscipline(this Document doc)`` - выдает дисциплину документа
- ``string GetDocumentPartition(this Document doc)`` - выдает раздел документа

#### Расширения для работы с семействами Revit
- ``void AddFamilyParameters(
            this IEnumerable<Family> families,
            Document doc,
            IEnumerable<ExternalDefinition> parameters)`` - добавляет параметры в семейства

#### Расширения для геометрии в Revit
- ``XYZ GetProjectBasePoint(this Document doc)`` - выдает базовую точку проекта

#### Расширения для параметра элемента Revit
- ``Parameter GetParameterFromInstanceOrType(
            this Element elem,
            string parameterName)`` - выдает параметр из экземпляра или из типа элемента
- ``object GetParameterValue(this Parameter param)`` - выдает значение, содержащееся в параметре элемента (double конвертирует в единицы отображающиеся на модели), если параметра нет то выдаст null
- ``bool SetParameterValue(
            this Element element,
            string parameterName,
            object value)`` - записывает значение в параметр элемента по названию параметра, конвертирую значение по типу параметра
- ``bool SetParameterValue(
            this Parameter parameter,
            object value)`` - записывает значение в параметр, конвертирую значение по типу параметра
- ``void CopyParameterValue(this Parameter fromParameter, Parameter toParameter)`` - копирует значение из одного параметра в другой параметр, конвертируя при необходимости значение в тип копируемого параметра

#### Расширения для параметров проекта
- ``bool HasParameter(
            this Document doc,
            string parameterName)`` - проверяет наличие параметра с заданным именем в проекте
- ``bool AddProjectParameter(
            this Document doc,
            string name,
            ParameterType type,
            bool visible,
            CategorySet cats,
            BuiltInParameterGroup group,
            bool inst)`` - добавляет параметр в проект (inst = true означает, что параметр будет инстанцирован элементам проекта)
- ``void DeleteParameter(
            this Document doc,
            string parameterName,
            bool useTransaction = true,
            Func<ParameterElement, bool> condition = null)`` - удаляет из проекта параметр по названию
- ``ElementId GetParameterId(
            this Document doc,
            string parameterName)`` - выдает Id параметра по названию или null если такого параметра нет

## Gitflow