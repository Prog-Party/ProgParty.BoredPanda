using ProgParty.BoredPanda.ApiUniversal.Result;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace ProgParty.BoredPanda
{
    public class BoredPandaDataContext : INotifyPropertyChanged
    {
        public ObservableCollection<OverviewResult> Gallery { get; set; } = new ObservableCollection<OverviewResult>();

        private bool _galleryItemsLoading = false;
        public bool GalleryItemsLoading
        {
            get { return _galleryItemsLoading; }
            set
            {
                _galleryItemsLoading = value;

                OnPropertyChanged("GalleryItemsLoadingVisibility");
            }
        }
        public Visibility GalleryItemsLoadingVisibility => GalleryItemsLoading ? Visibility.Visible : Visibility.Collapsed;

        public int GalleryItemIndex = 0;
        public int SelectedGallery = 0;

        public ApiUniversal.Parameter.ArticleCategory CurrentArticleGallery { get; set; } = ApiUniversal.Parameter.ArticleCategory.All;

        private bool _articleLoading = false;
        public bool ArticleLoading
        {
            get { return _articleLoading; }
            set
            {
                _articleLoading = value;

                OnPropertyChanged("ArticleLoadingVisibility");
            }
        }

        public Visibility ArticleLoadingVisibility => ArticleLoading ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<ArticleResult> Articles { get; internal set; } = new ObservableCollection<ArticleResult>();

        public int GalleryPaging = 0;

        public BoredPandaDataContext()
        {
        }

        internal OverviewResult GetNextGallery(int getNextGalleryCounter = 12)
        {
            SelectedGallery++;
            var galleryIndex = SelectedGallery;

            var galleryItem = Gallery.FirstOrDefault(g => g.Index == galleryIndex);
            if (galleryItem == null)
            {
                if (getNextGalleryCounter != 0)
                    return GetNextGallery(getNextGalleryCounter - 1);
            }

            return galleryItem;
        }

        internal bool NeedGalleryScrape()
        {
            var galleryIndex = SelectedGallery;
            if (galleryIndex > (GalleryItemIndex - 4))
            {
                return true;
            }

            return false;
        }

        internal void InitializeNewArticle(ArticleResult article)
        {
            ArticleLoading = false;
            Articles.Clear();
            Articles.Add(article);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}