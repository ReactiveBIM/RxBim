namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Abstractions.ConfigurationBuilders;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    /// <inheritdoc />
    public class JsonPanelElementConverter : JsonConverter
    {
        private readonly HashSet<Type> _derivedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPanelElementConverter"/> class.
        /// </summary>
        public JsonPanelElementConverter()
        {
            _derivedTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAssignableFrom(typeof(IRibbonPanelElement)) && !x.IsAbstract && !x.IsInterface)
                .ToHashSet();
        }

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IRibbonPanelElement);
        }

        /// <inheritdoc />
        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            var obj = JObject.Load(reader); // Throws an exception if the current token is not an object.
            var contract = FindContract(obj, serializer);
            if (contract == null)
                throw new JsonSerializationException("no contract found for " + obj);
            if (existingValue == null || !contract.UnderlyingType.IsInstanceOfType(existingValue))
                existingValue = contract.DefaultCreator?.Invoke();
            using var sr = obj.CreateReader();
            if (existingValue != null)
                serializer.Populate(sr, existingValue);
            return existingValue;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private JsonObjectContract FindContract(JObject obj, JsonSerializer serializer)
        {
            List<JsonObjectContract> bestContracts = new List<JsonObjectContract>();
            foreach (var type in _derivedTypes)
            {
                var contract = serializer.ContractResolver.ResolveContract(type) as JsonObjectContract;
                if (contract == null)
                    continue;
                if (obj.Properties()
                    .Select(p => p.Name)
                    .Any(n => contract.Properties.GetClosestMatchProperty(n) == null))
                    continue;
                if (bestContracts.Count == 0 || bestContracts[0].Properties.Count > contract.Properties.Count)
                {
                    bestContracts.Clear();
                    bestContracts.Add(contract);
                }
                else if (contract.Properties.Count == bestContracts[0].Properties.Count)
                {
                    bestContracts.Add(contract);
                }
            }

            return bestContracts.Single();
        }
    }
}