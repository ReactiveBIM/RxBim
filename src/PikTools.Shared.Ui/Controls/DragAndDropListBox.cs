namespace PikTools.Shared.Ui.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using PikTools.Shared.Ui.Helpers;

    /// <summary>
    /// ListBox, у которого можно перемещать элементы DragAndDrop
    /// </summary>
    /// <typeparam name="T">Класс, описывающий контекст элементов ListBox'а</typeparam>
    public class DragAndDropListBox<T> : ListBox
        where T : class
    {
        private Point _dragStartPoint;

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PreviewMouseMove += ListBox_PreviewMouseMove;

            var style = new Style(typeof(ListBoxItem));

            if (ItemContainerStyle != null)
                style.BasedOn = ItemContainerStyle;

            style.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));

            style.Setters.Add(
                new EventSetter(
                    ListBoxItem.PreviewMouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));

            style.Setters.Add(
                    new EventSetter(
                        ListBoxItem.DropEvent,
                        new DragEventHandler(ListBoxItem_Drop)));

            ItemContainerStyle = style;
        }

        private TP FindVisualParent<TP>(DependencyObject child)
            where TP : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is TP parent)
                return parent;

            return FindVisualParent<TP>(parentObject);
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem item)
            {
                var source = e.Data.GetData(typeof(T)) as T;
                var target = item.DataContext as T;

                int sourceIndex = Items.IndexOf(source);
                int targetIndex = Items.IndexOf(target);

                Move(source, sourceIndex, targetIndex);
            }
        }

        private void ListBoxItem_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                var customCursor = CursorHelper.CreateCursor(e.Source as UIElement);

                if (customCursor != null)
                {
                    e.UseDefaultCursors = false;
                    Mouse.SetCursor(customCursor);
                }
            }
            else
            {
                e.UseDefaultCursors = true;
            }

            e.Handled = true;
        }

        private void Move(T source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                if (ItemsSource is IList<T> items)
                {
                    items.Insert(targetIndex + 1, source);
                    items.RemoveAt(sourceIndex);
                }
            }
            else
            {
                if (ItemsSource is IList<T> items)
                {
                    int removeIndex = sourceIndex + 1;
                    if (items.Count + 1 > removeIndex)
                    {
                        items.Insert(targetIndex, source);
                        items.RemoveAt(removeIndex);
                    }
                }
            }
        }
    }
}
