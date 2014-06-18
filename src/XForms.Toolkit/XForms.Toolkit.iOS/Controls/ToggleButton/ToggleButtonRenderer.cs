using System.ComponentModel;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XForms.Toolkit.Controls;
using XForms.Toolkit.iOS.Controls.ImageButton;
using XForms.Toolkit.iOS.Controls.ToggleButton;

[assembly: ExportRenderer(typeof(ToggleButton), typeof(ToggleButtonRenderer))]
namespace XForms.Toolkit.iOS.Controls.ToggleButton
{
	public class ToggleButtonRenderer : ImageButtonRenderer
	{
		private Toolkit.Controls.ToggleButton ToggleButton
		{
			get { return (Toolkit.Controls.ToggleButton)this.Element; }
		}

	    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
	    {
	        base.OnElementChanged(e);
            
			UpdateBackground (); // NOTE: Since we can't override the base one we have to change the color again here... this is not the right way to do it but it is the only way currently
			UpdateTextColor ();
		}

		protected override void UpdateImage ()
		{
            if (this.ToggleButton != null) 
			{
                if (this.ToggleButton.IsSelected && !string.IsNullOrEmpty(this.ToggleButton.SelectedImage)) 
				{
                    this.SetupImage(this.ToggleButton.SelectedImage);
				}
                else if (!this.ToggleButton.IsSelected && !string.IsNullOrEmpty(this.ToggleButton.UnSelectedImage)) 
				{
                    this.SetupImage(this.ToggleButton.UnSelectedImage);
				}
			}
		}

	    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
	    {
	        if (e.PropertyName == Toolkit.Controls.ToggleButton.IsSelectedProperty.PropertyName ||
				e.PropertyName == Toolkit.Controls.ToggleButton.SelectedImageProperty.PropertyName ||
				e.PropertyName == Toolkit.Controls.ToggleButton.UnSelectedImageProperty.PropertyName) 
			{
				UpdateImage ();
				UpdateBackground ();
				UpdateTextColor ();
			}

            base.OnElementPropertyChanged(sender, e);
		}

		private void UpdateBackground()
		{
			if (ToggleButton.IsSelected)
				Control.BackgroundColor = ToggleButton.SelectedBackgroundColor.ToUIColor();
			else
				Control.BackgroundColor = ToggleButton.UnSelectedBackgroundColor.ToUIColor();
		}

		private void UpdateTextColor()
		{
			// TODO: Possibly think about allowing the setting of these colors differently so the users can specify a special disabled color and such

			if (ToggleButton.IsSelected) 
			{
                Control.SetTitleColor(ToggleButton.SelectedTextColor.ToUIColor(), UIControlState.Normal);
                Control.SetTitleColor(ToggleButton.SelectedTextColor.ToUIColor(), UIControlState.Highlighted);
                Control.SetTitleColor(ToggleButton.SelectedTextColor.ToUIColor(), UIControlState.Disabled);
			} 
			else 
			{
                Control.SetTitleColor(ToggleButton.UnSelectedTextColor.ToUIColor(), UIControlState.Normal);
                Control.SetTitleColor(ToggleButton.UnSelectedTextColor.ToUIColor(), UIControlState.Highlighted);
                Control.SetTitleColor(ToggleButton.UnSelectedTextColor.ToUIColor(), UIControlState.Disabled);
			}
		}
	}
}

