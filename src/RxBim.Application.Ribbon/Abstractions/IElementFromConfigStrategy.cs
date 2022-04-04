﻿namespace RxBim.Application.Ribbon.Abstractions
{
    using System.Collections.Generic;
    using ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting an element from a configuration section.
    /// </summary>
    public interface IElementFromConfigStrategy
    {
        /// <summary>
        /// Returns true if strategy is applicable for this configuration section. Otherwise returns false.
        /// </summary>
        /// <param name="elementSection">Configuration section.</param>
        bool IsApplicable(IConfigurationSection elementSection);

        /// <summary>
        /// Creates an element and adds to a elements collection.
        /// </summary>
        /// <param name="elementSection">Element configuration section.</param>
        /// <param name="elements">Elements collection.</param>
        void CreateFromConfigAndAdd(IConfigurationSection elementSection, ICollection<IRibbonPanelElement> elements);
    }
}