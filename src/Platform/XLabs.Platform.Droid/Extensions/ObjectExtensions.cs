namespace XLabs.Platform
{
    using Android.App;
    using Android.Content;

    /// <summary>
    /// Object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Starts the activity using either the object itself if it a type of <see cref="Context"/>
        /// or alternatively using <see cref="Application.Context"/>
        /// </summary>
        /// <param name="o">O.</param>
        /// <param name="intent">Intent.</param>
        public static void StartActivity(this object o, Intent intent)
        {
            var context = o as Context;
            if (context != null)
            {
                context.StartActivity (intent);
            } 
            else
            {
                intent.SetFlags (ActivityFlags.NewTask);
                Application.Context.StartActivity (intent);
            }
        }

        /// <summary>
        /// Wraps the object to <see cref="JavaObject{T}"/> class.
        /// </summary>
        /// <param name="o">Object to wrap.</param>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <returns></returns>
        public static JavaObject<T> ToJavaObject<T>(this T o)
        {
            return new JavaObject<T>(o);
        }
    }

    /// <summary>
    /// Java object wrapper.
    /// </summary>
    /// <typeparam name="T">Type of object to wrap.</typeparam>
    public class JavaObject<T> : Java.Lang.Object
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaObject{T}"/> class.
        /// </summary>
        /// <param name="obj"></param>
        public JavaObject(T obj)
        {
            this.Value = obj;
        }

        /// <summary>
        /// The object that was wrapped.
        /// </summary>
        public T Value { get; private set; }
    }
}

