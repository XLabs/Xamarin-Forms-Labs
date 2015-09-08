namespace XLabs.Sample.Pages.Services
{
    using System;
    using System.Text;
    using Ioc;
    using Platform.Services;
    using Xamarin.Forms;

    public partial class SecureStoragePage : ContentPage
    {
        public SecureStoragePage()
        {
            InitializeComponent();

            var secureStorage = Resolver.Resolve<ISecureStorage>();

            this.SaveButton.Command = new Command(() =>
                {
                    try
                    {
                        secureStorage.Store(this.KeyEntry.Text, Encoding.UTF8.GetBytes(this.DataEntry.Text));
                    }
                    catch (Exception ex)
                    {
                        this.DisplayAlert("Error", ex.Message, "OK");
                    }
                },
                () => secureStorage != null && !(string.IsNullOrWhiteSpace(this.KeyEntry.Text) || string.IsNullOrWhiteSpace(this.DataEntry.Text)));

            this.DeleteButton.Command = new Command(() =>
                {
                    try
                    {
                        secureStorage.Delete(this.KeyEntry.Text);
                    }
                    catch (Exception ex)
                    {
                        this.DisplayAlert("Error", ex.Message, "OK");
                    }
                },
                () => secureStorage != null && !string.IsNullOrWhiteSpace(this.KeyEntry.Text));

            this.LoadButton.Command = new Command(() =>
                {
                    try
                    {
                        var bytes = secureStorage.Retrieve(this.KeyEntry.Text);
                        this.DataEntry.Text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    }
                    catch (Exception ex)
                    {
                        this.DisplayAlert("Error", ex.Message, "OK");
                    }
                },
                () => secureStorage != null && !string.IsNullOrWhiteSpace(this.KeyEntry.Text));

            this.KeyEntry.TextChanged += (sender, args) =>
            {
                ((Command)this.SaveButton.Command).ChangeCanExecute();
                ((Command)this.DeleteButton.Command).ChangeCanExecute();
                ((Command)this.LoadButton.Command).ChangeCanExecute();
            };

            this.DataEntry.TextChanged += (sender, args) =>
            {
                ((Command)this.SaveButton.Command).ChangeCanExecute();
            };
        }
    }
}
