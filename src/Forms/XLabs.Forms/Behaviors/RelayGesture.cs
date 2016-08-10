// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="RelayGesture.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;

namespace XLabs.Forms.Behaviors
{
    /// <summary>
    /// Syncronous Implmentation of the IGesture
    /// Paramater is a T type
    /// </summary>
    public class RelayGesture<T> : IGesture where T:class
    {
        private readonly Action<GestureResult, T> _execute;
        private readonly Func<GestureResult, T, bool> _canexecute;


        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGesture(Action<GestureResult, T> execute, Func<GestureResult, T, bool> predicate = null)
        {
            this._execute = execute;
            _canexecute = predicate;
        }

        /// <summary>
        /// Excutes the action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater cast to T before calling the action</param>
        public void ExecuteGesture(GestureResult result, object param)
        {
            if (_execute != null) _execute(result, param as T);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater cast to T before calling the function</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object param)
        {
            return (_canexecute == null || _canexecute(result, param as T));
        }
    }

    /// <summary>
    /// Syncronous Implmentation of the IGesture
    /// Paramater is a objet type
    /// </summary>
    public class RelayGesture : IGesture
    {
        private readonly Action<GestureResult, object> _execute;
        private readonly Func<GestureResult,object,bool> _canexecute;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGesture(Action<GestureResult,object>execute,Func<GestureResult,object,bool>predicate=null)
        {
            this._execute = execute;
            _canexecute = predicate;
        }

        /// <summary>
        /// Excutes the action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        public void ExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            if (_execute != null) _execute(result, annoyingbaseobjectthing);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (_canexecute == null || _canexecute(result, annoyingbaseobjectthing));
        }       
    }
}