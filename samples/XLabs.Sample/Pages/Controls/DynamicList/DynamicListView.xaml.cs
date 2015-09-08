namespace XLabs.Sample.Pages.Controls.DynamicList
{
	using System;

	using XLabs;

	public partial class DynamicListView
    {
        private int _count;
        public DynamicListView()
        {
            InitializeComponent();

            this.Button.Clicked += (s, e) => this.DynamicList.Data.Add(string.Format("Added {0} items.", ++_count));

            this.ButtonDate.Clicked += (s, e) => this.DynamicList.Data.Add(DateTime.Now);

            this.DynamicList.OnSelected += dynamicList_OnSelected;
        }

        void dynamicList_OnSelected(object sender, EventArgs<object> e)
        {
            System.Diagnostics.Debug.WriteLine(e.Value);
            try
            {
                this.DynamicList.Remove(e.Value);
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
