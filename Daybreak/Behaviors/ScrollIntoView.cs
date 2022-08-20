using Microsoft.Xaml.Behaviors;
using System;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Behaviors
{
    public class ScrollIntoView : Behavior<ListView>
    {
        /// <summary>
        ///  When Beahvior is attached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += this.AssociatedObject_SelectionChanged;
        }

        /// <summary>
        /// On Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_SelectionChanged(object sender,
                                               SelectionChangedEventArgs e)
        {
            if (sender is ListView)
            {
                var listView = sender.As<ListView>();
                if (listView.SelectedItem != null)
                {
                    listView.Dispatcher.BeginInvoke(
                        (Action)(() =>
                        {
                            listView.UpdateLayout();
                            if (listView.SelectedItem !=
                            null)
                                listView.ScrollIntoView(
                                listView.SelectedItem);
                        }));
                }
            }
        }
        /// <summary>
        /// When behavior is detached
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -=
                this.AssociatedObject_SelectionChanged;

        }
    }
}
