﻿#nullable enable

using System;

namespace Slipstream.Shared
{
    public interface IEventHandlerController
    {
        public bool Enabled { get; set; }

        public event EventHandler<IEvent>? OnDefault;

        public event EventHandler<IEvent>? OnAllways;

        public T Get<T>();

        public void HandleEvent(IEvent? ev);
    }
}