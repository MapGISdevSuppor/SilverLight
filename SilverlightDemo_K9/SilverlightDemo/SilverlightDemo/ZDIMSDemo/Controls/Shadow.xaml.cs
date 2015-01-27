using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ScatterView;
using agTweener;

namespace ZDIMSDemo.Controls
{
    public partial class Shadow : UserControl, IScatterViewShadow
    {
        private TweenListItem _Tween;

        public Shadow()
        {
            InitializeComponent();
        }

        public new void Drop()
        {
            if (this._Tween != null)
            {
                Tweener.RemoveTween(this._Tween);
            }
            TweenParameter p = new TweenParameter
            {
                Opacity = 0.9,
                time = 0.5,
                transition = TransitionType.easeOutBounce
            };
            this._Tween = Tweener.addTween(this.LayoutRoot, p);
        }

        public void Lift()
        {
            if (this._Tween != null)
            {
                Tweener.RemoveTween(this._Tween);
            }
            TweenParameter p = new TweenParameter
            {
                Opacity = 0.5,
                time = 0.5,
                transition = TransitionType.easeOutSine
            };
            this._Tween = Tweener.addTween(this.LayoutRoot, p);
        }
    }
}
