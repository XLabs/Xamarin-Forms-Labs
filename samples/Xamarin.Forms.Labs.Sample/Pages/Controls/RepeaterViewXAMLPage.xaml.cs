using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Sample.ViewModel;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class RepeaterViewXAMLPage
    {
        public RepeaterViewXAMLPage()
        {
            InitializeComponent();
            var viewModel = new RepeaterViewXAMLViewModel();
            BindingContext = viewModel;
            viewModel.LoadData();
        }


        async void RepeaterItemCreated(object sender, RepeaterViewItemAddedEventArgs args)
        {
            Thing thing = args.Model as Thing;

            // Set animation start values;
            args.View.Opacity = 0;
            await args.View.LayoutTo(new Rectangle(500, args.View.Y, args.View.Width, args.View.Height), 0);

            // To stagger animation, multiply the delay amount with the index of the item being added .... await Task.Delay(200 * thing.Index);
            await Task.Delay(200);

            // Compose and start animations
            var translateAnimation = args.View.LayoutTo(new Rectangle(0, args.View.Y, args.View.Width, args.View.Height), 500, Easing.CubicInOut);
            var fadeInAnimation = args.View.FadeTo(1, 500, Easing.Linear);
            await Task.WhenAll(translateAnimation, fadeInAnimation);
        }
    }
}
