using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicLib.Animations
{
    /// <summary>
    /// Указывает в какую сторону будет выполнятся анимация
    /// </summary>
    public enum PageAnimation
    {
        None = 0,
        SlideAndFadeInFromRight = 1,
        SlideAndFadeOutFromLeft = 2
    }
}
