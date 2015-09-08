﻿using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{    
    public partial class CheckBoxPage : ContentPage
    {    
        public CheckBoxPage ()
        {
            InitializeComponent ();

            listView.ItemsSource = Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().Select(c => c.ToString());
        }
    }
}

