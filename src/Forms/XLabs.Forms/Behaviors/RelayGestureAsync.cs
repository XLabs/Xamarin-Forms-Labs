using System;
using System.Threading.Tasks;

namespace XLabs.Forms.Behaviors
{
    /// <summary>
    /// ASyncronous Implmenta tion of the IGesture
    /// The execute is asyncronous while the canexecute is syncronous
    /// Paramater is an T type
    /// </summary>
    public class RelayGestureAsync<T> : IGesture where T:class
    {
        private readonly Func<GestureResult, T, Task> _asyncExecute;
        private readonly Func<GestureResult, T, bool> _canexecute;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The asyncronous action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGestureAsync(Func<GestureResult, T, Task> execute, Func<GestureResult, T, bool> predicate)
        {
            _asyncExecute = execute;
            _canexecute = predicate;
        }
        /// <summary>
        /// Excutes the asyncronous action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater</param>
        public async void ExecuteGesture(GestureResult result, object param)
        {
            if (_asyncExecute != null) await Execute(result, param);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (_canexecute == null || _canexecute(result, annoyingbaseobjectthing as T));
        }
        /// <summary>
        /// Virtual aync funciton that the user can override to provide
        /// any custom functionality required.
        /// </summary>
        /// <param name="gesture"><see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing"></param>
        /// <returns></returns>
        protected virtual async Task Execute(GestureResult gesture, object annoyingbaseobjectthing)
        {
            await _asyncExecute(gesture, annoyingbaseobjectthing as T);
        }
    }

    /// <summary>
    /// ASyncronous Implmenta tion of the IGesture
    /// The execute is asyncronous while the canexecute is syncronous
    /// Paramater is an object type
    /// </summary>
    public class RelayGestureAsync : IGesture
    {
        private readonly Func<GestureResult, object, Task> _asyncExecute;
        private readonly Func<GestureResult, object, bool> _canexecute;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The asyncronous action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGestureAsync(Func<GestureResult, object, Task> execute, Func<GestureResult, object, bool> predicate)
        {
            _asyncExecute = execute;
            _canexecute = predicate;
        }
        /// <summary>
        /// Excutes the asyncronous action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater</param>
        public async void ExecuteGesture(GestureResult result, object param)
        {
            if (_asyncExecute != null) await Execute(result, param);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (_canexecute == null || _canexecute(result,annoyingbaseobjectthing));
        }
        /// <summary>
        /// Virtual aync funciton that the user can override to provide
        /// any custom functionality required.
        /// </summary>
        /// <param name="gesture"><see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing"></param>
        /// <returns></returns>
        protected virtual async Task Execute(GestureResult gesture, object annoyingbaseobjectthing)
        {
            await _asyncExecute(gesture, annoyingbaseobjectthing);
        }
    }
}