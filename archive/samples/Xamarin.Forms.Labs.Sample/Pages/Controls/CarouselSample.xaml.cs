namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    using System;
    using System.Collections.ObjectModel;

    public partial class CarouselSample  : ContentPage
    {
        public CarouselSample()
        {
            InitializeComponent();
            BindingContext = new CarouselVM();
        }
    }


    //Bad practice, only doing this for quick sample....

    public class CarouselVM : BaseViewModel
    {
        public CarouselVM()
        {
            Models=new ObservableCollection<PageWidget>
                {
                    new WorkOrder
                        {
                            Client = "John Smith",
                            Description = "Plumbing"
                        },
                    new Message
                        {
                            Sender = "Cindy",
                            Received = DateTime.Now,
                            Content = "John is getting flooded!!!"
                        },
                    new WorkOrder
                        {
                            Client = "Jane Jones",
                            Description = "Shoveling"
                        },
                    new Message
                        {
                            Sender = "Cindy",
                            Received = DateTime.Now,
                            Content = "Jane is snowed in"
                        },
                                            new WorkOrder
                        {
                            Client = "Dave Roberts",
                            Description = "Ghost writing"
                        },
                    new Message
                        {
                            Sender = "Cindy",
                            Received = DateTime.Now,
                            Content = "Dave is out of ideas"
                        },
                    new WorkOrder
                        {
                            Client = "George Henry",
                            Description = "Life advice"
                        },
                    new Message
                        {
                            Sender = "Cindy",
                            Received = DateTime.Now,
                            Content = "George isn't sure who he  is"
                        }



                };
        }

        public ObservableCollection<PageWidget> Models { get; set; }
    }

    public class PageWidget
    {
        public PageWidget(string title) { Title = title; }
        public string Title { get; private set; }
    }

    public class WorkOrder : PageWidget
    {
        public WorkOrder() : base("Work Order") { }
        public string Client { get; set; }
        public string Description { get; set; }
    }

    public class Message : PageWidget
    {
        public Message() : base("Message"){}
        public string Sender { get; set; }
        public DateTime Received { get; set; }
        public string Content { get; set; }
    }
}
