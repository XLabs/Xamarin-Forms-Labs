using System;

namespace XLabs
{
    /// <summary>
    /// Event extensions.
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Raise the specified event.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <param name="sender">Sender object.</param>
        /// <param name="value">Argument value.</param>
        /// <typeparam name="T">The value type.</typeparam>
        public static void Invoke<T>(this EventHandler<EventArgs<T>> handler, object sender, T value)
        {
            var handle = handler;
            if (handle != null)
            {
                handle(sender, new EventArgs<T>(value));
            }
        }

        public static bool TryInvoke<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            var handle = handler;
            if (handle != null)
            {
                handle(sender, args);
                return true;
            }

            return false;
        }
    }
}
