// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="RelayCommand{T}.cs" company="XLabs Team">
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
using System.Windows.Input;

namespace XLabs
{
    /// <summary>
    /// Generic RelayCommand class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// The execute
        /// </summary>
        private readonly Action<T> execute;

        /// <summary>
        /// The can execute
        /// </summary>
        private readonly Predicate<T> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;

            if (canExecute != null)
            {
                this.canExecute = canExecute;
            }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raise <see cref="RelayCommand{T}.CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute.Invoke((T)parameter);
        }

        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute((T)parameter);
            }
        }
    }
}