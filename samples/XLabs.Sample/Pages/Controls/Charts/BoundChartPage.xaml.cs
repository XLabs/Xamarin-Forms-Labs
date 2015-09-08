namespace XLabs.Sample.Pages.Controls.Charts
{
	public partial class BoundChartPage
    {
        public BoundChartPage()
        {
            InitializeComponent();
            this.BindingContext = new BoundChartViewModel();
        }
    }
}
