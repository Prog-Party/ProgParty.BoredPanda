using ProgParty.BoredPanda.ApiUniversal.Parameter;
using ProgParty.BoredPanda.ApiUniversal.Result;
using ProgParty.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ProgParty.BoredPanda
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public BoredPandaDataContext PageDataContext { get; set; }
        public Pivot Pivot => searchPivot;

        CancellationTokenSource source = new CancellationTokenSource();
        SynchronizationContext context = SynchronizationContext.Current;
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            Config.Instance = new Config(this)
            {
                Pivot = searchPivot,
                AppName = "BoredPanda",
                Ad = new ConfigAd()
                {
                    AdHolder = AdHolder,
                    AdApplicationId = "bc5d2773-5216-47a6-804e-ce6640711282",
                    SmallAdUnitId = "11566395",
                    MediumAdUnitId = "11566399",
                    LargeAdUnitId = "11566398"
                }
            };

#if DEBUG
            ProgParty.Core.Config.Instance.LicenseInformation = CurrentAppSimulator.LicenseInformation;
#else
            ProgParty.Core.Config.Instance.LicenseInformation = CurrentApp.LicenseInformation;
#endif

            ProgParty.Core.License.LicenseInfo.SetLicenseInformation();

            Register.Execute();

            PageDataContext = new BoredPandaDataContext();
            DataContext = PageDataContext;

            string gallery = GetSavedGalleryType();
            if (string.IsNullOrEmpty(gallery))
                gallery = "All";

            ComboBoxMenu.SelectedIndex = (int)GetByName(gallery);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Register.RegisterOnNavigatedTo(Config.Instance.LicenseInformation);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Register.RegisterOnLoaded();
        }

        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProgParty.Core.Pages.Contact), null);
        }

        private void BuyBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProgParty.Core.Pages.Shop));
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private async void ShowGallery(ArticleCategory category)
        {
            //Core.Track.Telemetry.Instance.Action("Filter", new Dictionary<string, string>() { { "filter", category.ToString() } });
            PageDataContext.CurrentArticleGallery = category;
            var parameters = new OverviewParameter() { StartOver = true, Category = category, Paging = 0 };
            await Task.Factory.StartNew(() => Searcher.ExecuteGaleryScrape(this, context, parameters), source.Token);
        }

        private async void GalleryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems.FirstOrDefault() as OverviewResult;
            if (selectedItem == null)
                return;

            PageDataContext.SelectedGallery = selectedItem.Index;

            if (PageDataContext.NeedGalleryScrape())
                await Task.Factory.StartNew(() => Searcher.ExecuteGaleryScrape(this, context), source.Token);

            await Task.Factory.StartNew(() => Searcher.ExecuteSingleArticleScrape(this, context, selectedItem), source.Token);

        }

        private void ComboBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.First() as ComboBoxItem;
            string name = item.Name;
            SaveGalleryType(name);
            ShowArticleGallery(name);
        }

        private void ShowArticleGallery(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ArticleCategory articleCategory = GetByName(name);
            ShowGallery(articleCategory);
        }

        private void SaveGalleryType(string name) => new ProgParty.Core.Storage.Storage().StoreInLocal("gallerytype", name);

        private string GetSavedGalleryType() => new ProgParty.Core.Storage.Storage().LoadFromLocal("gallerytype")?.ToString() ?? "";

        private ArticleCategory GetByName(string name)
        {
            switch (name)
            {
                case "Advertising": return ArticleCategory.Advertising;
                case "All": return ArticleCategory.All;
                case "Animals": return ArticleCategory.Animals;
                case "Architecture": return ArticleCategory.Architecture;
                case "Art": return ArticleCategory.Art;
                case "Automotive": return ArticleCategory.Automotive;
                case "BodyArt": return ArticleCategory.BodyArt;
                case "Comics": return ArticleCategory.Comics;
                case "DigitalArt": return ArticleCategory.DigitalArt;
                case "DIY": return ArticleCategory.DIY;
                case "Drawing": return ArticleCategory.Drawing;
                case "Entertainment": return ArticleCategory.Entertainment;
                case "FoodArt": return ArticleCategory.FoodArt;
                case "Funny": return ArticleCategory.Funny;
                case "FurnitureDesign": return ArticleCategory.FurnitureDesign;
                case "GoodNews": return ArticleCategory.GoodNews;
                case "GraphicDesign": return ArticleCategory.GraphicDesign;
                case "History": return ArticleCategory.History;
                case "Home": return ArticleCategory.Home;
                case "Illustration": return ArticleCategory.Illustration;
                case "Installation": return ArticleCategory.Installation;
                case "InteriorDesign": return ArticleCategory.InteriorDesign;
                case "LandArt": return ArticleCategory.LandArt;
                case "Nature": return ArticleCategory.Nature;
                case "NeedleAndThread": return ArticleCategory.NeedleAndThread;
                case "OpticalIllusions": return ArticleCategory.OpticalIllusions;
                case "Other": return ArticleCategory.Other;
                case "Packaging": return ArticleCategory.Packaging;
                case "Painting": return ArticleCategory.Painting;
                case "PaperArt": return ArticleCategory.PaperArt;
                case "Parenting": return ArticleCategory.Parenting;
                case "Photography": return ArticleCategory.Photography;
                case "Pics": return ArticleCategory.Pics;
                case "ProductDesign": return ArticleCategory.ProductDesign;
                case "Recycling": return ArticleCategory.Recycling;
                case "Science": return ArticleCategory.Science;
                case "Sculpting": return ArticleCategory.Sculpting;
                case "SocialIssues": return ArticleCategory.SocialIssues;
                case "StreetArt": return ArticleCategory.StreetArt;
                case "Style": return ArticleCategory.Style;
                case "Technology": return ArticleCategory.Technology;
                case "Travel": return ArticleCategory.Travel;
                case "Typography": return ArticleCategory.Typography;
                case "Video": return ArticleCategory.Video;
                case "Weird": return ArticleCategory.Weird;
                default:
                    throw new System.NotImplementedException("This type is not implemented " + name);
            }
        }

        private async void LoadMoreGalleries_Click(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() => Searcher.ExecuteGaleryScrape(this, context), source.Token);
        }

        private async void LoadPreviousGallery_Click(object sender, RoutedEventArgs e)
        {
            if (PageDataContext.NeedGalleryScrape())
                await Task.Factory.StartNew(() => Searcher.ExecuteGaleryScrape(this, context), source.Token);

            var gallery = PageDataContext.GetNextGallery();

            if (gallery == null)
            {
                //should not be possible, because gallery's are loaded async before the last gallery is reached.
                new MessageDialog("Geen gallerij aanwezig, laad er meer.").ShowAsync();
                Pivot.SelectedIndex = 0;
            }
            else
            {
                await Task.Factory.StartNew(() => Searcher.ExecuteSingleArticleScrape(this, context, gallery), source.Token);
            }
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            var share = new ProgParty.Core.Share.ShareUrl();

            share.RegisterForShare(sender as MenuFlyoutItem, ((sender as MenuFlyoutItem).DataContext as OverviewResult).Url);
        }
    }
}
