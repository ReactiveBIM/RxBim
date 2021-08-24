namespace RxBim.Shared.Ui.Behaviors
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Дополняет ListBox свойствами SelectedItems и SelectedValues для двойной привязки к множественному выбору
    /// </summary>
    /// <typeparam name="T">Тип данным в ItemsSource</typeparam>
    /// <remarks>https://tyrrrz.me/blog/wpf-listbox-selecteditems-twoway-binding</remarks>
    public class ListBoxSelectionBehavior<T> : Behavior<ListBox>
    {
        /// <summary>
        /// Свой-во зависимости для SelectedItems
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems),
                typeof(IList),
                typeof(ListBoxSelectionBehavior<T>),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemsChanged));

        /// <summary>
        /// Свой-во зависимости для SelectedValues
        /// </summary>
        public static readonly DependencyProperty SelectedValuesProperty =
            DependencyProperty.Register(nameof(SelectedValues),
                typeof(IList),
                typeof(ListBoxSelectionBehavior<T>),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedValuesChanged));

        private bool _viewHandled;
        private bool _modelHandled;

        /// <summary>
        /// Выбранные элементы
        /// </summary>
        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Выбранные значения
        /// </summary>
        public IList SelectedValues
        {
            get => (IList)GetValue(SelectedValuesProperty);
            set => SetValue(SelectedValuesProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnListBoxItemsChanged;

            _modelHandled = true;
            SelectedValuesToItems();
            SelectItems();
            _modelHandled = false;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
                ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnListBoxItemsChanged;
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (ListBoxSelectionBehavior<T>)sender;
            if (behavior._modelHandled)
                return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.SelectedItemsToValues();
            behavior.SelectItems();
            behavior._modelHandled = false;
        }

        private static void OnSelectedValuesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (ListBoxSelectionBehavior<T>)sender;
            if (behavior._modelHandled)
                return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.SelectedValuesToItems();
            behavior.SelectItems();
            behavior._modelHandled = false;
        }

        private static object GetDeepPropertyValue(object obj, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return obj;

            while (true)
            {
                if (path.Contains('.'))
                {
                    string[] split = path.Split('.');
                    string remainingProperty = path.Substring(path.IndexOf('.') + 1);
                    obj = obj
                        .GetType()
                        .GetProperty(split[0])
                        .GetValue(obj, null);
                    path = remainingProperty;
                    continue;
                }

                return obj
                    .GetType()
                    .GetProperty(path)
                    .GetValue(obj, null);
            }
        }

        // Propagate selected items from model to view
        private void SelectItems()
        {
            if (AssociatedObject.SelectionMode != SelectionMode.Multiple)
                return;

            _viewHandled = true;
            AssociatedObject.SelectedItems.Clear();
            if (SelectedItems != null)
            {
                foreach (var item in SelectedItems)
                    AssociatedObject.SelectedItems.Add(item);
            }

            _viewHandled = false;
        }

        // Update SelectedItems based on SelectedValues
        private void SelectedValuesToItems()
        {
            if (AssociatedObject.SelectionMode != SelectionMode.Multiple)
                return;

            if (SelectedValues == null)
            {
                SelectedItems = null;
            }
            else
            {
                SelectedItems = AssociatedObject.Items
                        .Cast<T>()
                        .Where(i => SelectedValues.Contains((T)GetDeepPropertyValue(i, AssociatedObject.SelectedValuePath)))
                        .ToList();
            }
        }

        // Update SelectedValues based on SelectedItems
        private void SelectedItemsToValues()
        {
            if (AssociatedObject.SelectionMode != SelectionMode.Multiple)
                return;

            if (SelectedItems == null)
            {
                SelectedValues = null;
            }
            else
            {
                SelectedValues = SelectedItems
                        .Cast<T>()
                        .Select(i => (T)GetDeepPropertyValue(i, AssociatedObject.SelectedValuePath))
                        .ToList();
            }
        }

        // Propagate selected items from view to model
        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (_viewHandled)
                return;
            if (AssociatedObject.Items.SourceCollection == null)
                return;

            SelectedItems = AssociatedObject.SelectedItems
                .Cast<T>()
                .ToList();
        }

        // Re-select items when the set of items changes
        private void OnListBoxItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_viewHandled)
                return;
            if (AssociatedObject.Items.SourceCollection == null)
                return;

            SelectItems();
        }
    }
}
