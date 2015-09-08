**Xamarin Forms Labs** [![Build status](https://ci.appveyor.com/api/projects/status/33q2u1d3dpn3abgn?svg=true)](https://ci.appveyor.com/project/xlabs/xamarin-forms-labs)
=====================

[![Build status](https://ci.appveyor.com/api/projects/status/33q2u1d3dpn3abgn?svg=true)](https://ci.appveyor.com/project/xlabs/xamarin-forms-labs)

**XLabs** is a open source project that aims to provide a powerful and cross platform set of services and controls tailored to work with Xamarin and [Xamarin Forms](http://xamarin.com/forms).

Call for action for all Xamarin Developers, embrace this project and share your controls and services with the community, add your own control to the toolkit.

**Important for developers**
The master branch is the current development branch.
The v.2.0 is the stable branch.

**Available controls**

 - [AutoCompleteView (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/AutoCompleteView)
 - [Calendar Control (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/Calendar-Control)
 - [Checkbox (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/Checkbox-Control)
 - [DynamicListView (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/DynamicListView)
 - ExtendedContentView (beta) 
 - [ExtendedEntry (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedEntry)
 - [ExtendedLabel (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedLabel)
 - [ExtendedScrollView (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedScrollView)
 - [ExtendedTabbedPage](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedTabbedPage)  
 - [ExtendedTextCell (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedTextCell)
 - [ExtendedViewCell (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ExtendedViewCell)
 - [HybridWebView (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/HybridWebView)
 - GradientContentView (iOS/Android beta)
 - [GridView (IOS beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/GridView)
 - [ImageButton (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/ImageButton)
 - RadioButton (beta)
 - [RepeaterView (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/RepeaterView)
 - [SegmentedControlView (IOS beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/SegmentedControl)
 - [Web Image (beta)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/WebImage)
 - IconButton (IOS beta)
 - [CircleImage (IOS/Android alpha)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/CircleImage)
 - [HyperLinkLabel](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/HyperLinkLabel)

**Available services**

 - Accelerometer
 - Cache
 - Camera (Picture and Video picker, Take Picture, Take Video)
 - Device (battery info, device info, sensors, accelerometers)
 - Display
 - Geolocator
 - Phone Service (cellular network info, make phonecalls)
 - SoundService
 - Text To Speech 
 - Secure Storage
 - Settings


**Available Mvvm helpers (Beta)**

 - ViewModel (navigation, isbusy)
 - ViewFactory
 - IOC
 - IXFormsApp (application events)

**Available Plugins**
    
 - Serialization (ServiceStackV3, ProtoBuf, JSON.Net)
 - Caching (SQLLiteSimpleCache)
 - Dependency Injection containers (TinyIOC, Autofac, NInject, SimpleInjector, Unity)
 - Web (RestClient)
 - [Charting (Line, Bar & Pie) (Alpha)](https://github.com/XLabs/Xamarin-Forms-Labs/wiki/Charting)
 
_________________


**HOW-TO**
======

We are working in a great [wiki][1] on how to use the controls and services. 


https://github.com/XLabs/Xamarin-Forms-Labs/wiki

[Good forum post helping you setup and use XLabs](http://forums.xamarin.com/discussion/35991/how-to-install-setup-and-use-xlabs#latest)

Using the MVVM Helpers
-----------

**ViewFactory**
Coming soon



Using the controls
-----------


Add XLabs.Forms reference to your projects , main pcl, ios, android, and wp.

Xaml :

Reference the assembly namespace 

     xmlns:controls="clr-namespace:XLabs.Forms.Controls;assembly=XLabs.Forms"

Render your control:

     <controls:ImageButton Text="Twitter" BackgroundColor="#01abdf" TextColor="#ffffff" HeightRequest="75" WidthRequest="175" Image="icon_twitter" Orientation="ImageToLeft"  ImageHeightRequest="50" ImageWidthRequest="50" />
      
Or from your codebehind:


	var button = new ImageButton() {
				ImageHeightRequest = 50,
				ImageWidthRequest = 50,
				Orientation = Orientation.ImageToLeft,
				Source = "icon_twitter.png",
				Text = "Twitter"
			};
	stacker.Children.Add (button);
	


Using the Services
-----------

**TextToSpeechService** 

	Resolver.Resolve<ITextToSpeechService>().Speak(TextToSpeak);
	
**Device** 

		var device = Resolver.Resolve<IDevice>();
		device.Display; //display information
		device.Battery; //battery information

	
**PhoneService** 

	 	var device = Resolver.Resolve<IDevice>();
		// not all devices have phone service, f.e. iPod and Android tablets
		// so we need to check if phone service is available
		if (device.PhoneService != null)
		{
			device.PhoneService.DialNumber("+1 (855) 926-2746");
		}


Initializing the Services
-----------
Do this before using the services

**Step 1:** 
* iOS => Make sure your AppDelegate inherits from XFormsApplicationDelegate

* Android => MainActivity inherits from XFormsApplicationDroid

* Windows Phone => Add this line to your App.cs 
				  var app = new XFormsAppWP(); app.Init(this);

**Step 2:** 
		Initialize the container in your app startup code.

		var container = new SimpleContainer ();
		container.Register<IDevice> (t => AppleDevice.CurrentDevice);
		container.Register<IDisplay> (t => t.Resolve<IDevice> ().Display);
		container.Register<INetwork>(t=> t.Resolve<IDevice>().Network);

		Resolver.SetResolver (container.GetResolver ());

[For more info on initialization go to the Labs Wiki](https://github.com/XLabs/Xamarin-Forms-Labs/wiki)
________________


**Helper**
======

> Based in last developments (master)

[Master- XLabs Framework Helper for online use](http://htmlpreview.github.io/?https://raw.githubusercontent.com/XLabs/Xamarin-Forms-Labs/master/Helper/master/Web/Index.html)

[Master - Xamarin.Forms.Labs.chm file for offline use](https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/Helper/master/Xamarin.Forms.Labs.chm)
________________


**Build the project**
======

To develop on this project, just clone the project to your computer, package restore is enable so build the solution first, if you get any errors try to build each project independently .
		
__________________

**Nuget**
======

**Main Packages:**

- [XLabs.Platform](http://www.nuget.org/packages/XLabs.Platform/)
- [XLabs.Forms] (http://www.nuget.org/packages/XLabs.Forms/)

**Plugins:**

* To be updated...
 
__________________

**Contributions:**
======
 - Shawn Anderson
 - [Jim Bennett](http://www.jimbobbennett.io) [@jimbobbennett](https://twitter.com/jimbobbennett)
 - Filip De Vos  [@foxtricks](https://twitter.com/foxtricks) 
 - [Kevin E. Ford](http://windingroadway.blogspot.com/) [@Bowman74](https://twitter.com/Bowman74)
 - [Eric Grover](http://www.ericgrover.com) [@bluechiperic](https://twitter.com/bluechiperic) 
 - Ben Ishiyama-Levy [@mrbrl](http://www.monovo.io) 
 - [Sami M. Kallio](https://www.linkedin.com/profile/view?id=4900454)
 - Bart Kardol
 - Petr Klíma
 - [Thomas Lebrun](http://blog.thomaslebrun.net/) [@thomas_lebrun](https://twitter.com/thomas_lebrun) 
 - [Rui Marinho](http://ruimarinho.net/)  [@ruiespinho](https://twitter.com/ruiespinho)
 - [Mitch Milam](http://blogs.infinite-x.net) [@mitchmilam](https://twitter.com/mitchmilam)
 - [Oren Novotny](http://blog.novotny.org) [@onovotny](https://twitter.com/onovotny)
 - Michael Ridland [@rid00z ](https://twitter.com/rid00z)
 - Chris Riesgo [@chrisriesgo](https://twitter.com/chrisriesgo)
 - [Nicholas Rogoff](http://blog.nicholasrogoff.com/) [@nrogoff](https://twitter.com/nrogoff)
 - [Sara Silva](http://saramgsilva.com) [@saramgsilva](https://twitter.com/saramgsilva)
 - Jason Smith [@jassmith87](https://twitter.com/jassmith87)
 - Ryan Wischkaemper
 - Kazuki Yasufuku

 
**Other Project Contributions:**
------------------
- Xamarin.Mobile


**Contribute**
------------------

Everbody is welcome to contribute with any kind of controls or features at this time.

Twitter hashtag : [#xflabs](https://twitter.com/search?q=xflabs)
		
		  
		  
_________________

**CHAT**
======

[XLabs Chat room online on Jabbr ](https://jabbr.net/#/rooms/Xamarin-Labs)

__________________

**License**
======

License Apache 2.0 more about that in the [LICENSE][2] file. 




  [1]: https://github.com/XLabs/Xamarin-Forms-Labs/wiki
  [2]: https://github.com/XForms/XForms-Toolkit/blob/master/LICENSE
  
  

