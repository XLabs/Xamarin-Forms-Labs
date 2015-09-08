using CoreGraphics;
using Foundation;
using UIKit;

namespace XLabs.Forms.Controls
{
    [Register("RadioButtonView")]
    public class RadioButtonView : UIButton
    {
        public RadioButtonView()
        {
            Initialize();
        }

        public RadioButtonView(CGRect bounds)
            : base(bounds)
        {
            Initialize();
        }

        
        public bool Checked
        {
            set { this.Selected = value; }
            get { return this.Selected; }
        }

        public string Text
        {
            set { this.SetTitle(value, UIControlState.Normal); }
            
        }

        void Initialize()
        {
            this.AdjustEdgeInsets();
            this.ApplyStyle();

            this.TouchUpInside += (sender, args) => this.Selected = !this.Selected;
        }

        void AdjustEdgeInsets()
        {
            const float inset = 8f;

            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            this.ImageEdgeInsets = new UIEdgeInsets(0f, inset, 0f, 0f);
            this.TitleEdgeInsets = new UIEdgeInsets(0f, inset * 2, 0f, 0f);
        }

        void ApplyStyle()
        {
            this.SetImage(UIImage.FromBundle("Images/RadioButton/checked.png"), UIControlState.Selected);
            this.SetImage(UIImage.FromBundle("Images/RadioButton/unchecked.png"), UIControlState.Normal);
        }
    }
}