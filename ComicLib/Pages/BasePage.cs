using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using ComicLib.Animations;
using System.ComponentModel;

namespace ComicLib.Pages
{
    /// <summary>
    /// Базовая страница для анимации
    /// </summary>
    public class BasePage : System.Windows.Controls.Page
    {
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutFromLeft;
        public float SlideSeconds { get; set; } = 0.8f;

        public BasePage()
        {

            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = Visibility.Collapsed;

            this.Loaded += BasePage_Loaded;
        }

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }

        private async void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            await AnimateOut();
        }

        public async Task AnimateIn()
        {
            switch (this.PageLoadAnimation)
            {
                case PageAnimation.None:
                    break;

                case PageAnimation.SlideAndFadeInFromRight:

                    var storyboard = new Storyboard();
                    var slideAnimation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(this.SlideSeconds)),
                        From = new Thickness(this.WindowWidth, 0, -this.WindowWidth, 0),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
                    storyboard.Children.Add(slideAnimation);

                    storyboard.Begin(this);

                    this.Visibility = Visibility.Visible;

                    await Task.Delay((int)this.SlideSeconds * 1000);

                    break;

                case PageAnimation.SlideAndFadeOutFromLeft:
                    break;

                default:
                    break;
            }
        }

        public async Task AnimateOut()
        {
            switch (this.PageUnloadAnimation)
            {
                case PageAnimation.None:
                    break;

                case PageAnimation.SlideAndFadeInFromRight:
                    break;

                case PageAnimation.SlideAndFadeOutFromLeft:

                    var storyboard = new Storyboard();
                    var slideAnimation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(this.SlideSeconds)),
                        From = new Thickness(0),
                        To = new Thickness(-this.WindowWidth, 0, this.WindowWidth, 0),
                        DecelerationRatio = 0.9f
                    };
                    Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
                    storyboard.Children.Add(slideAnimation);

                    storyboard.Begin(this);

                    this.Visibility = Visibility.Visible;

                    await Task.Delay((int)this.SlideSeconds * 1000);

                    break;

                default:
                    break;
            }
        }
    }
}
