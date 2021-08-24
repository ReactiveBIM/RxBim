namespace RxBim.Shared.Ui.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Abstractions;

    /// <summary>
    /// ItemsControl с IScrollProvider
    /// </summary>
    public class ListBoxWithScrollProvider : ListBox, IScrollProvider
    {
        /// <inheritdoc/>
        public void ScrollToBottom()
        {
            Dispatcher.Invoke(() =>
            {
                var scrollViewer = FindScrollViewer(this);

                if (scrollViewer != null)
                    scrollViewer.ScrollToBottom();
            });
        }

        private ScrollViewer FindScrollViewer(DependencyObject root)
        {
            var queue = new Queue<DependencyObject>(new[] { root });

            do
            {
                var item = queue.Dequeue();

                if (item is ScrollViewer viewer)
                    return viewer;

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
                    queue.Enqueue(VisualTreeHelper.GetChild(item, i));
            }
            while (queue.Count > 0);

            return null;
        }
    }
}
