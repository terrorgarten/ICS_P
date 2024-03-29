﻿using System;
using Carpool.App.Messages;

namespace Carpool.App.Services;

public interface IMediator
{
    void Register<TMessage>(Action<TMessage> action)
        where TMessage : IMessage;

    void Send<TMessage>(TMessage message)
        where TMessage : IMessage;

    void UnRegister<TMessage>(Action<TMessage> action)
        where TMessage : IMessage;
}