using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Services
{
    public partial class FontManager
    {
        private const short InitialSize = 24;

        public Font FindClosest(string name, double desiredHeight)
        {
            var height = this.GetHeight(Font.OfSize(name, InitialSize));

            var multiply = (int)((desiredHeight / height) * InitialSize);


            var f1 = Font.OfSize(name, multiply);
            var f2 = Font.OfSize(name, multiply + 1);

            var h1 = this.GetHeight(f1);
            var h2 = this.GetHeight(f2);

            var d1 = Math.Abs(h1 - desiredHeight);
            var d2 = Math.Abs(h2 - desiredHeight);

            return d1 < d2 ? f1 : f2;
        }

    }
}
