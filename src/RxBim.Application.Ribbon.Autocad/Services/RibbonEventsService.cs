﻿namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.Windows;
    using Ribbon.Abstractions;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Ribbon events service
    /// </summary>
    public class RibbonEventsService : IRibbonEventsService, IDisposable
    {
        private readonly IRibbonMenuBuilderFactory _builderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonEventsService"/> class.
        /// </summary>
        /// <param name="builderFactory"><see cref="IRibbonMenuBuilderFactory"/>.</param>
        public RibbonEventsService(IRibbonMenuBuilderFactory builderFactory)
        {
            _builderFactory = builderFactory;
        }

        /// <inheritdoc />
        public void Run()
        {
            if (ComponentManager.Ribbon is null)
                ComponentManager.ItemInitialized += OnItemInitialized;
            else
                VariableChangedForRibbonRebuild();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.SystemVariableChanged -= OnSystemVariableChanged;
            ComponentManager.ItemInitialized -= OnItemInitialized;
        }

        private void OnItemInitialized(object sender, RibbonItemEventArgs e)
        {
            if (ComponentManager.Ribbon is null)
                return;

            ComponentManager.ItemInitialized -= OnItemInitialized;
            OnIdleForRibbonCreating();
            VariableChangedForRibbonRebuild();
        }

        private void VariableChangedForRibbonRebuild()
        {
            Application.SystemVariableChanged += OnSystemVariableChanged;
        }

        private void OnSystemVariableChanged(object sender, SystemVariableChangedEventArgs e)
        {
            const string wsCurrentVariableName = "WSCURRENT";
            if (e.Name.Equals(wsCurrentVariableName, StringComparison.OrdinalIgnoreCase))
            {
                OnIdleForRibbonCreating();
            }
        }

        private void OnIdleForRibbonCreating()
        {
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            _builderFactory.CurrentBuilder?.BuildRibbonMenu();
            Application.Idle -= OnIdle;
        }
    }
}