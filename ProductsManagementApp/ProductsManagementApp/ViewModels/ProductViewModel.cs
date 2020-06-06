using ProductsManagementApp.Interfaces;
using ProductsManagementApp.Models;
using ProductsManagementApp.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using System;
using ProductsManagementApp.Converters;

namespace ProductsManagementApp.ViewModels
{
    public class ProductViewModel : ViewModelCore
    {
        #region Services
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IExpirationDateService _expirationDateService;
        #endregion

        #region Properties
        public int SelectedIndex { get; set; }
        private int _id { get; set; }
        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(()=>Id); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }
        private string _barecode;
        public string BareCode
        {
            get { return _barecode; }
            set { _barecode = value; RaisePropertyChanged(() => BareCode); }
        }
        private int _maxDays;
        public int MaxDays
        {
            get { return _maxDays; }
            set { _maxDays = value; RaisePropertyChanged(() => MaxDays); }
        }
        private byte[] _image;
        public byte[] Image
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged(() => Image); }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; RaisePropertyChanged(() => IsVisible); }
        }
        private Category category;
        public Category Category
        {
            get { return category; }
            set { if (value != null) { category = value; RaisePropertyChanged(() => Category); } }
        }
        private ImageSource imageSrc;
        public ImageSource ImageSrc
        {
            get { return imageSrc; }
            set { imageSrc = value; RaisePropertyChanged(() => ImageSrc); }
        }
        private string imageSrcString;
        public string ImageSrcString
        {
            get { return imageSrcString; }
            set { imageSrcString = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ExpirationDate> _expirationDates;
        public ObservableCollection<ExpirationDate> ExpirationDates
        {
            get { return _expirationDates; }
            set
            {
                _expirationDates = value;
                RaisePropertyChanged(() => ExpirationDates);
            }
        }
        public ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(() => Categories); }
        }

        #endregion

        #region Commands
        public ICommand SaveCommand => new Command<ProductViewModel>(async (item) => await SaveProduct(item));
        public ICommand ScanCodeCommand => new Command(async () => await ScanCodeClicked());
        public ICommand GalleryCommand => new Command(async () => await GalleryClicked());
        public ICommand CameraCommand => new Command(async () => await CameraClicked());
        public ICommand CollectCommand => new Command<ExpirationDate>(async (item) => await CollectProductsAsync(item));
        public ICommand MinusCommand => new Command<ExpirationDate>((item) => MinusCount(item));
        public ICommand PlusCommand => new Command<ExpirationDate>((item) => PlusCount(item));
        public ICommand ShowExpDateCommand => new Command(async (item) => await ShowExpDate(item));
        #endregion

        #region Constructors
        public ProductViewModel(IProductService productService, ICategoryService categoryService,IExpirationDateService expirationDateService, Product product):this(productService,categoryService, expirationDateService)
        {
            this.InitializeProperties(product);
        }
        public ProductViewModel(IProductService productService,ICategoryService categoryService, IExpirationDateService expirationDateService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._expirationDateService = expirationDateService;

            this.InitializeProperties(new Product(0, "", null, 0, 10, null));

            this.Categories = new ObservableCollection<Category>();

        }
        #endregion

        #region Methods

        private void InitializeProperties(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.BareCode = product.BareCode;
            this.MaxDays = product.MaxDays;
            this.Image = product.Image;
            this.IsVisible = false;
            this.Category = new Category
            {
                Id = product.CategoryId
            };
            this.ExpirationDates = new ObservableCollection<ExpirationDate>(product.ExpirationDates);
        }
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            int id = (int)navigationData;

            Product product = await this.LoadProductAsync(id);

            if(product!=null)
            {
                this.InitializeProperties(product);
                await this.LoadExpirationDatesAsync();
            }
            else
            {
                this.InitializeProperties(new Product(0, "", null, 0, 10, null));
            }

            await this.SetCategoriesAsync();

            IsBusy = false;
        }
        public override string ToString()
        {
            return $"{Id} {Name} {BareCode} " + Category??Category.ToString();
        }
        private async Task SaveProduct(ProductViewModel product)
        {
            if (product.BareCode == null)
                product.BareCode = "9922694580143";
            Product savedProduct = new Product(product.Id, product.Name, product.BareCode, product.Category.Id, product.MaxDays);
            if (product.Image != null)
                savedProduct.Image = product.Image;

            string result = "";

            if(product.Id==0)
                result = await _productService.PostAsync(savedProduct);
            else
                result = await _productService.PutAsync(savedProduct);

            if (String.IsNullOrEmpty(result))
            {
                await DialogService.ShowAlertAsync("Podczas zapisu produktu wystąpił problem", "Zapis produktu", "Ok");
            }
            else
            {
                await DialogService.ShowAlertAsync(result.ToString(), "Zapis produktu", "Ok");
                await NavigationService.RemoveBackStackAsync();
                await NavigationService.NavigateToAsync<CategoriesViewModel>();
            }
        }
        private async Task CollectProductsAsync(ExpirationDate expD)
        {
            var result = await _expirationDateService.PutAsync(expD);
            if (!String.IsNullOrEmpty(result))
            {
                await DialogService.ShowAlertAsync(Name + " w ilości " + expD.Count.ToString(), "Zebranie produktow", "Ok");
                await this.LoadExpirationDatesAsync();
            }
            else
            {
                await DialogService.ShowAlertAsync("Wystąpił błąd", "Zebranie produktow", "Ok");
            }
        }
        private void MinusCount(ExpirationDate expD)
        {
            if (expD.Count > 0)
            {
                int idx = ExpirationDates.IndexOf(expD);
                expD.Count--;
                ExpirationDates[idx] = expD;
            }
        }
        private void PlusCount(ExpirationDate expD)
        {
            int idx = ExpirationDates.IndexOf(expD);
            expD.Count++;
            ExpirationDates[idx] = expD;
        }
        private async Task ShowExpDate(object expDate)
        {
            int expDateId = 0;

            if(expDate!=null && expDate is ExpirationDate)
            {
                ExpirationDate tmp = (ExpirationDate)expDate;
                    if(tmp!=null)
                        expDateId = tmp.Id;

                await NavigationService.NavigateToAsync<ExpirationDateViewModel>(expDateId);
            }
            else
            {
                await NavigationService.NavigateToAsync<ExpirationDateViewModel>(new Product(Id,Name,BareCode,Category.Id, 10, null));
            }

        }
        private async Task ScanCodeClicked()
        {
            var scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.RemoveLastAsync();
                    await DialogService.ShowAlertAsync("Zeskanowany kod", result.Text + " " + result.BarcodeFormat, "OK");
                    if (result.BarcodeFormat != ZXing.BarcodeFormat.EAN_13)
                        await DialogService.ShowAlertAsync("Nie udało się zeskanować poprawnie kodu", "Błąd skanowania", "OK");
                    else
                        this.BareCode = result.Text;
                });
            };

            await NavigationService.PushZxingPage(scanPage);
        }
        private async Task CameraClicked()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DialogService.ShowAlertAsync("Aparat", "Aparat jest niedostępny", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Products Management",
                SaveToAlbum = true,
                CompressionQuality = 50,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Small,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;
            else
            {
                SetImageProperties(file);
            }

        }
        private async Task GalleryClicked()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DialogService.ShowAlertAsync("Galeria", "Galeria jest niedostępna", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Small,

            });

            if (file == null)
                return;
            else
            {
                SetImageProperties(file);
            }
        }
        private void SetImageProperties(MediaFile file)
        {
            this.ImageSrcString = file.Path;

            this.ImageSrc = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });


            using (var memoryStream = new MemoryStream())
            {
                var stream = file.GetStream();
                file.Dispose();
                stream.CopyTo(memoryStream);
                this.Image = memoryStream.ToArray();
            }

            this.ImageSrc = ImageSource.FromStream(() => new MemoryStream(this.Image));
        }

        private async Task LoadExpirationDatesAsync()
        {

            try
            {
                this.ExpirationDates = ListToObservableConverter<ExpirationDate>.ToObservable(await _expirationDateService.GetAsync(Id));
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Błąd pobierania dat przydatności", "Ok");
            }
        }
        private async Task<Product> LoadProductAsync(int id)
        {
            Product product = null;
            if (id != 0)
            {
                try
                {
                    product = await _productService.GetProductById(id);
                }
                catch (Exception e)
                {
                    await DialogService.ShowAlertAsync(e.Message, "Błąd pobierania szczegółów produktu", "OK");
                }

            }

            return product;
        }
        private async Task SetCategoriesAsync()
        {
            try
            {
                Categories = new ObservableCollection<Category>(await _categoryService.GetAsync());
                if (Category.Id == 0)
                    Category = new Category();
                else
                    Category = Categories.FirstOrDefault(c => c.Id == Category.Id);
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Błąd pobierania kategorii", "Ok");
            }
        }
        #endregion
    }
}
