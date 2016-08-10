// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CameraViewRenderer.cs" company="XLabs Team">
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
using System.Collections.Generic;
using Android.Content;
using Android.Hardware;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class CameraViewRenderer.
	/// </summary>
	public class CameraViewRenderer : ViewRenderer<CameraView, CameraPreview>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				this.SetNativeControl(new CameraPreview(this.Context));
			}

			this.Control.PreviewCamera = this.Control.PreviewCamera ?? Camera.Open((int)e.NewElement.Camera);
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			switch (e.PropertyName)
			{
				case "Camera":
					this.Control.SwitchCamera(Camera.Open((int)this.Element.Camera));
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Control.PreviewCamera.Release();
			}

			base.Dispose(disposing);
		}
	}

	/// <summary>
	/// Class CameraPreview.
	/// </summary>
	public class CameraPreview : ViewGroup, ISurfaceHolderCallback
	{
		const string TAG = "Preview";

		SurfaceView _mSurfaceView;
		ISurfaceHolder _mHolder;
		Camera.Size _mPreviewSize;
		IList<Camera.Size> _mSupportedPreviewSizes;
		Camera _camera;

		/// <summary>
		/// Gets or sets the preview camera.
		/// </summary>
		/// <value>The preview camera.</value>
		public Camera PreviewCamera
		{
			get { return _camera; }
			set
			{
				_camera = value;
				if (_camera != null)
				{
					_mSupportedPreviewSizes = PreviewCamera.GetParameters().SupportedPreviewSizes;
					RequestLayout();
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraPreview"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public CameraPreview(Context context)
			: base(context)
		{
			_mSurfaceView = new SurfaceView(context);
			AddView(_mSurfaceView);

			// Install a SurfaceHolder.Callback so we get notified when the
			// underlying surface is created and destroyed.
			_mHolder = _mSurfaceView.Holder;
			_mHolder.AddCallback(this);
		}

		/// <summary>
		/// Switches the camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public void SwitchCamera(Camera camera)
		{
			if (PreviewCamera != null)
			{
				//PreviewCamera.StopPreview();
				PreviewCamera.Release();
			}

			PreviewCamera = camera;

			try
			{
				camera.SetPreviewDisplay(_mHolder);
			}
			catch (Java.IO.IOException exception)
			{
				Android.Util.Log.Error(TAG, "IOException caused by setPreviewDisplay()", exception);
			}

			Camera.Parameters parameters = camera.GetParameters();
			parameters.SetPreviewSize(_mPreviewSize.Width, _mPreviewSize.Height);
			RequestLayout();

			camera.SetParameters(parameters);
		}

		/// <summary>
		/// Called when [measure].
		/// </summary>
		/// <param name="widthMeasureSpec">The width measure spec.</param>
		/// <param name="heightMeasureSpec">The height measure spec.</param>
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			// We purposely disregard child measurements because act as a
			// wrapper to a SurfaceView that centers the camera preview instead
			// of stretching it.
			int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
			int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
			SetMeasuredDimension(width, height);

			if (_mSupportedPreviewSizes != null)
			{
				_mPreviewSize = GetOptimalPreviewSize(_mSupportedPreviewSizes, width, height);
			}
		}

		/// <summary>
		/// Called when [layout].
		/// </summary>
		/// <param name="changed">if set to <c>true</c> [changed].</param>
		/// <param name="l">The l.</param>
		/// <param name="t">The t.</param>
		/// <param name="r">The r.</param>
		/// <param name="b">The b.</param>
		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (changed && ChildCount > 0)
			{
				var child = GetChildAt(0);

				int width = r - l;
				int height = b - t;

				int previewWidth = width;
				int previewHeight = height;
				if (_mPreviewSize != null)
				{
					previewWidth = _mPreviewSize.Width;
					previewHeight = _mPreviewSize.Height;
				}

				// Center the child SurfaceView within the parent.
				if (width * previewHeight > height * previewWidth)
				{
					int scaledChildWidth = previewWidth * height / previewHeight;
					child.Layout((width - scaledChildWidth) / 2, 0,
						(width + scaledChildWidth) / 2, height);
				}
				else
				{
					int scaledChildHeight = previewHeight * width / previewWidth;
					child.Layout(0, (height - scaledChildHeight) / 2,
						width, (height + scaledChildHeight) / 2);
				}
			}
		}

		/// <summary>
		/// Surfaces the created.
		/// </summary>
		/// <param name="holder">The holder.</param>
		public void SurfaceCreated(ISurfaceHolder holder)
		{
			// The Surface has been created, acquire the camera and tell it where
			// to draw.
			try
			{
				if (PreviewCamera != null)
				{
					PreviewCamera.SetPreviewDisplay(holder);
				}
			}
			catch (Java.IO.IOException exception)
			{
				Android.Util.Log.Error(TAG, "IOException caused by setPreviewDisplay()", exception);
			}
		}

		/// <summary>
		/// Surfaces the destroyed.
		/// </summary>
		/// <param name="holder">The holder.</param>
		public void SurfaceDestroyed(ISurfaceHolder holder)
		{
			// Surface will be destroyed when we return, so stop the preview.
			if (PreviewCamera != null)
			{
				PreviewCamera.StopPreview();
			}
		}

		/// <summary>
		/// Gets the size of the optimal preview.
		/// </summary>
		/// <param name="sizes">The sizes.</param>
		/// <param name="w">The w.</param>
		/// <param name="h">The h.</param>
		/// <returns>Camera.Size.</returns>
		private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
		{
			const double AspectTolerance = 0.1;
			double targetRatio = (double)w / h;

			if (sizes == null)
				return null;

			Camera.Size optimalSize = null;
			double minDiff = Double.MaxValue;

			int targetHeight = h;

			// Try to find an size match aspect ratio and size
			foreach (Camera.Size size in sizes)
			{
				double ratio = (double)size.Width / size.Height;

				if (Math.Abs(ratio - targetRatio) > AspectTolerance)
					continue;

				if (Math.Abs(size.Height - targetHeight) < minDiff)
				{
					optimalSize = size;
					minDiff = Math.Abs(size.Height - targetHeight);
				}
			}

			// Cannot find the one match the aspect ratio, ignore the requirement
			if (optimalSize == null)
			{
				minDiff = Double.MaxValue;
				foreach (Camera.Size size in sizes)
				{
					if (Math.Abs(size.Height - targetHeight) < minDiff)
					{
						optimalSize = size;
						minDiff = Math.Abs(size.Height - targetHeight);
					}
				}
			}

			return optimalSize;
		}

		/// <summary>
		/// Surfaces the changed.
		/// </summary>
		/// <param name="holder">The holder.</param>
		/// <param name="format">The format.</param>
		/// <param name="w">The w.</param>
		/// <param name="h">The h.</param>
		public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h)
		{
			// Now that the size is known, set up the camera parameters and begin
			// the preview.
			var parameters = PreviewCamera.GetParameters();
			parameters.SetPreviewSize(_mPreviewSize.Width, _mPreviewSize.Height);
			RequestLayout();

			PreviewCamera.SetParameters(parameters);
			PreviewCamera.StartPreview();
		}
	}
}

