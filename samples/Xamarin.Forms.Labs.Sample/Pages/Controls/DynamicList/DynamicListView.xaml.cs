using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.DynamicList
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class DynamicListView: BaseView
    {
        private int count;
        public DynamicListView()
        {
            InitializeComponent();

            this.button.Clicked += (s, e) => this.dynamicList.Data.Add(string.Format("Added {0} items.", ++count));

            this.buttonDate.Clicked += (s, e) => this.dynamicList.Data.Add(DateTime.Now);

            this.dynamicList.OnSelected += dynamicList_OnSelected;
        }

        void dynamicList_OnSelected(object sender, EventArgs<object> e)
        {
            System.Diagnostics.Debug.WriteLine(e.Value);
            try
            {
                this.dynamicList.Remove(e.Value);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
