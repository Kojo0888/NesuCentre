using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NesuCentre.Helpers
{
    public static class AnimationHelper
    {
        public static void StartResourceAnimation(FrameworkElement fnElem, string resourceKey)
        {
            var storyboard = fnElem.FindResource(resourceKey) as Storyboard;
            if (storyboard != null)
                storyboard.Begin(fnElem);
        }

        public static void StopResourceAnimation(FrameworkElement fnElem, string resourceKey)
        {
            var storyboard = fnElem.FindResource(resourceKey) as Storyboard;
            if (storyboard != null)
                storyboard.Stop(fnElem);
        }
    }
}
