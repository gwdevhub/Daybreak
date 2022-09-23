﻿using System.Windows.Controls;

namespace Daybreak.Services.Navigation
{
    public interface IViewProducer
    {
        void RegisterView<T>()
            where T : UserControl;
        void RegisterPermanentView<T>()
            where T : UserControl;
    }
}
