using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using UniversalImageLoader.Core;
using UniversalImageLoader.Core.Assist;
using UniversalImageLoader.Core.Display;
using UniversalImageLoader.Core.Listener;
using Console = System.Console;

namespace WoWonder.Helpers
{
    public class ImageCacheLoader
    {
        public static DisplayImageOptions DefaultOptions;
        public static DisplayImageOptions CircleOptions;
        public static ImageSize SizeMinimized = new ImageSize(30, 30);
       

        public static void InitImageLoader(Context context)
        {
            try
            {
                SetImageOption();

                ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(context)
                    .ThreadPriority(Thread.NormPriority - 2)
                    .TasksProcessingOrder(QueueProcessingType.Lifo)
                    .DefaultDisplayImageOptions(DefaultOptions)
                    .DiskCacheSize(100 * 1024 * 1024)
                    .Build();

                 ImageLoader.Instance.SetDefaultLoadingListener(new ImageLoadingListener());

                if (!ImageLoader.Instance.IsInited)
                    ImageLoader.Instance.Init(config);             
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SetImageOption()
        {
            DefaultOptions = new DisplayImageOptions.Builder()
                .ShowImageOnLoading(Resource.Drawable.Grey_Offline)
                .ShowImageForEmptyUri(Resource.Drawable.Grey_Offline)
                .ShowImageOnFail(Resource.Drawable.ImagePlacholder)
                .CacheInMemory(true)
                .CacheOnDisk(true)
                .ConsiderExifParams(true)
                .Displayer(new FadeInBitmapDisplayer(500))
                .ImageScaleType(ImageScaleType.Exactly)
                .Build();

            CircleOptions = new DisplayImageOptions.Builder()
                .ShowImageOnLoading(Resource.Drawable.Grey_Offline)
                .ShowImageForEmptyUri(Resource.Drawable.Grey_Offline)
                .ShowImageOnFail(Resource.Drawable.ImagePlacholder)
                .CacheInMemory(true)
                .CacheOnDisk(true)
                .ConsiderExifParams(true)
                .Displayer(new FadeInBitmapDisplayer(500)).Displayer(new CircleBitmapDisplayer())
                .ImageScaleType(ImageScaleType.Exactly)
                .Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">any url like websites or disk or storage will work</param>
        /// <param name="image">ImageView view</param>
        /// <param name="downsized"> compress the image to 100 x 100 px</param>
        /// <param name="circle">Set to true with Downsized true if you want to display the image as circle</param>
        public static void LoadImage(string url, ImageView image,bool downsized,bool circle)
        {
            try
            {
                var options = DefaultOptions;

                if (circle)
                    options = CircleOptions;

                if (downsized)
                {
                    ImageLoader.Instance.LoadImage(url, SizeMinimized, options, new WithParamImageLoadingListener(image));
                } 
                else
                {
                    ImageLoader.Instance.DisplayImage(url, image);
                }
                
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }

       


        public class ImageLoadingListener : Java.Lang.Object, IImageLoadingListener
        {
            public void OnLoadingCancelled(string p0, View p1)
            {

            }

            public void OnLoadingComplete(string p0, View p1, Bitmap p2)
            {
                try
                {
                    p2.Dispose();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            public void OnLoadingFailed(string p0, View p1, FailReason p2)
            {

            }

            public void OnLoadingStarted(string p0, View p1)
            {

            }
        }

        public class WithParamImageLoadingListener : Java.Lang.Object, IImageLoadingListener
        {
            public ImageView Image;
            public WithParamImageLoadingListener(ImageView image)
            {
                Image = image;
            }
            public void OnLoadingCancelled(string p0, View p1)
            {

            }

            public void OnLoadingComplete(string p0, View p1, Bitmap p2)
            {
                try
                {
                    Image.SetImageBitmap(p2);
                    p2.Dispose();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            public void OnLoadingFailed(string p0, View p1, FailReason p2)
            {

            }

            public void OnLoadingStarted(string p0, View p1)
            {

            }
        }

    }
}