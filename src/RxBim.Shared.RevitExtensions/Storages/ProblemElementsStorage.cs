namespace RxBim.Shared.RevitExtensions.Storages
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    /// <summary>
    /// Хранилище проблемных элементов
    /// </summary>
    public class ProblemElementsStorage : IProblemElementsStorage
    {
        private readonly Dictionary<string, List<int>> _storage =
            new Dictionary<string, List<int>>();

        /// <inheritdoc/>
        public void AddProblemElement(int id, string problem)
        {
            if (_storage.ContainsKey(problem))
                _storage[problem].Add(id);
            else
                _storage.Add(problem, new List<int> { id });
        }

        /// <inheritdoc/>
        public IDictionary<string, IEnumerable<int>> GetCombineProblems()
        {
            return _storage
                .ToDictionary(
                    problem => problem.Key,
                    problem => new List<int>(problem.Value) as IEnumerable<int>);
        }

        /// <inheritdoc/>
        public IEnumerable<KeyValuePair<int, string>> GetProblems()
        {
            return _storage
                .SelectMany(problem => problem.Value.Select(id => new KeyValuePair<int, string>(id, problem.Key)));
        }

        /// <inheritdoc/>
        public bool HasProblems()
            => _storage.Any();

        /// <inheritdoc/>
        public void Clear()
            => _storage.Clear();
    }
}
