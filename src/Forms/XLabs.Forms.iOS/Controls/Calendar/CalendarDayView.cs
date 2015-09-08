
//
//  CalendarMonthView.cs
//
//  Converted to MonoTouch on 1/22/09 - Eduardo Scoz || http://escoz.com
//  Originally reated by Devin Ross on 7/28/09  - tapku.com || http://github.com/devinross/tapkulibrary
//  Adapted for XLabs project by Vratislav Kalenda on 22/09/14 http://www.applifting.cz
/*
 
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
 
 */

namespace XLabs.Forms.Controls
{
    using System;
    using CoreGraphics;
    using Foundation;
    using UIKit;

    /// <summary>
    /// Class CalendarDayView.
    /// </summary>
    public class CalendarDayView : UIView
    {
        /// <summary>
        /// The paragraph style
        /// </summary>
        private static NSMutableParagraphStyle paragraphStyle;

        /// <summary>
        /// The _text
        /// </summary>
        private string _text;

        /// <summary>
        /// The _old backgorund color
        /// </summary>
        private UIColor _oldBackgorundColor;

        /// <summary>
        /// The _active
        /// </summary>
        private bool _active, _today, _selected, _marked, _available, _highlighted;

        /// <summary>
        /// The _MV
        /// </summary>
        private readonly CalendarMonthView _mv;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDayView"/> class.
        /// </summary>
        /// <param name="mv">The mv.</param>
        public CalendarDayView(CalendarMonthView mv)
        {
            _mv = mv;
            BackgroundColor = mv.StyleDescriptor.DateBackgroundColor;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is available.
        /// </summary>
        /// <value><c>true</c> if available; otherwise, <c>false</c>.</value>
        public bool Available { get { return _available; } set { _available = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get { return _text; } set { _text = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get { return _active; } set { _active = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is today.
        /// </summary>
        /// <value><c>true</c> if today; otherwise, <c>false</c>.</value>
        public bool Today { get { return _today; } set { _today = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is selected.
        /// </summary>
        /// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
        public bool Selected { get { return _selected; } set { _selected = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is marked.
        /// </summary>
        /// <value><c>true</c> if marked; otherwise, <c>false</c>.</value>
        public bool Marked { get { return _marked; } set { _marked = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarDayView"/> is highlighted.
        /// </summary>
        /// <value><c>true</c> if highlighted; otherwise, <c>false</c>.</value>
        public bool Highlighted { get { return _highlighted; } set { _highlighted = value; SetNeedsDisplay(); } }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            UIImage img = null;
            var color = _mv.StyleDescriptor.InactiveDateForegroundColor;
            BackgroundColor = _mv.StyleDescriptor.InactiveDateBackgroundColor;
            var backgroundStyle = CalendarView.BackgroundStyle.Fill;


            if (!Active || !Available)
            {
                if (Highlighted)
                {
                    BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
                }
                //color = UIColor.FromRGBA(0.576f, 0.608f, 0.647f, 1f);
                //img = UIImage.FromBundle("Images/Calendar/datecell.png");
            }
            else if (Today && Selected)
            {
                color = _mv.StyleDescriptor.SelectedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/todayselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            }
            else if (Today)
            {
                color = _mv.StyleDescriptor.TodayForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.TodayBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.TodayBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/today.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            }
            else if (Selected || Marked)
            {
                //color = UIColor.White;
                color = _mv.StyleDescriptor.SelectedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/datecellselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            }
            else if (Highlighted)
            {
                color = _mv.StyleDescriptor.HighlightedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
            }
            else
            {
                color = _mv.StyleDescriptor.DateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.DateBackgroundColor;
                //img = UIImage.FromBundle("Images/Calendar/datecell.png");
            }

            //if (img != null)
            //img.Draw(new RectangleF(0, 0, _mv.BoxWidth, _mv.BoxHeight));
            var context = UIGraphics.GetCurrentContext();
            if (_oldBackgorundColor != BackgroundColor)
            {
                if (backgroundStyle == CalendarView.BackgroundStyle.Fill)
                {
                    context.SetFillColor(BackgroundColor.CGColor);
                    context.FillRect(new CGRect(0, 0, _mv.BoxWidth, _mv.BoxHeight));
                }
                else
                {
                    context.SetFillColor(Highlighted
                        ? _mv.StyleDescriptor.HighlightedDateBackgroundColor.CGColor
                        : _mv.StyleDescriptor.DateBackgroundColor.CGColor);

                    context.FillRect(new CGRect(0, 0, _mv.BoxWidth, _mv.BoxHeight));

                    var smallerSide = Math.Min(_mv.BoxWidth, _mv.BoxHeight);
                    var center = new CGPoint(_mv.BoxWidth / 2, _mv.BoxHeight / 2);
                    var circleArea = new CGRect(center.X - smallerSide / 2, center.Y - smallerSide / 2, smallerSide, smallerSide);

                    if (backgroundStyle == CalendarView.BackgroundStyle.CircleFill)
                    {
                        context.SetFillColor(BackgroundColor.CGColor);
                        context.FillEllipseInRect(circleArea.Inset(1, 1));
                    }
                    else
                    {
                        context.SetStrokeColor(BackgroundColor.CGColor);
                        context.StrokeEllipseInRect(circleArea.Inset(2, 2));
                    }
                }
            }


            color.SetColor();
            var inflated = new CGRect(0, 0, Bounds.Width, Bounds.Height);
            //			var attrs = new UIStringAttributes() {
            //				Font = _mv.StyleDescriptor.DateLabelFont,
            //				ForegroundColor = color,
            //				ParagraphStyle = 
            //
            //			};
            //((NSString)Text).DrawString(inflated,attrs);
            //DrawString(Text, inflated,_mv.StyleDescriptor.DateLabelFont,UILineBreakMode.WordWrap, UITextAlignment.Center);
            DrawDateString((NSString)Text, color, inflated);

            //            if (Marked)
            //            {
            //                var context = UIGraphics.GetCurrentContext();
            //                if (Selected || Today)
            //                    context.SetRGBFillColor(1, 1, 1, 1);
            //                else if (!Active || !Available)
            //					UIColor.LightGray.SetColor();
            //				else
            //                    context.SetRGBFillColor(75/255f, 92/255f, 111/255f, 1);
            //                context.SetLineWidth(0);
            //                context.AddEllipseInRect(new RectangleF(Frame.Size.Width/2 - 2, 45-10, 4, 4));
            //                context.FillPath();
            //
            //            }
            _oldBackgorundColor = BackgroundColor;
            //Console.WriteLine("Drawing of cell took {0} msecs",(DateTime.Now-dt).TotalMilliseconds);
        }

        /// <summary>
        /// Draws the date string.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="color">The color.</param>
        /// <param name="rect">The rect.</param>
        private void DrawDateString(NSString dateString, UIColor color, CGRect rect)
        {
            if (paragraphStyle == null)
            {
                paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
                paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
                paragraphStyle.Alignment = UITextAlignment.Center;

            }
            var attrs = new UIStringAttributes()
            {
                Font = _mv.StyleDescriptor.DateLabelFont,
                ForegroundColor = color,
                ParagraphStyle = paragraphStyle
            };
            var size = dateString.GetSizeUsingAttributes(attrs);
            var targetRect = new CGRect(
                rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
                rect.Y + (float)Math.Floor((rect.Height - size.Height) / 2f),
                                        size.Width,
                                        size.Height
                                    );
            dateString.DrawString(targetRect, attrs);
        }

        //		/*
        //      (void) drawString: (NSString*) s
        //           withFont: (UIFont*) font
        //             inRect: (CGRect) contextRect {
        //
        //    /// Make a copy of the default paragraph style
        //    NSMutableParagraphStyle *paragraphStyle = [[NSParagraphStyle defaultParagraphStyle] mutableCopy];
        //    /// Set line break mode
        //    paragraphStyle.lineBreakMode = NSLineBreakByTruncatingTail;
        //    /// Set text alignment
        //    paragraphStyle.alignment = NSTextAlignmentCenter;
        //
        //    NSDictionary *attributes = @{ NSFontAttributeName: font,
        //                                  NSForegroundColorAttributeName: [UIColor whiteColor],
        //                                  NSParagraphStyleAttributeName: paragraphStyle };
        //
        //    CGSize size = [s sizeWithAttributes:attributes];
        //
        //    CGRect textRect = CGRectMake(contextRect.origin.x + floorf((contextRect.size.width - size.width) / 2),
        //                                 contextRect.origin.y + floorf((contextRect.size.height - size.height) / 2),
        //                                 size.width,
        //                                 size.height);
        //
        //    [s drawInRect:textRect withAttributes:attributes];
        //}
        //
        //
        //		*/
    }
}
