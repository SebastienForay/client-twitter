using Windows.UI.Xaml.Controls;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitterDotNet.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class HomeTimelinePage : Page
    {
        public HomeTimelinePage()
        {
            this.InitializeComponent();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var verticalOffsetValue = TweetsScrollViewer.VerticalOffset;
            var maxVerticalOffsetValue = TweetsScrollViewer.ExtentHeight - TweetsScrollViewer.ViewportHeight;

            // Dernier item de la liste atteint
            if (maxVerticalOffsetValue < 0 || verticalOffsetValue == maxVerticalOffsetValue)
            {
                ViewModel.LoadMoreTweets();
            }

        }
    }
}
