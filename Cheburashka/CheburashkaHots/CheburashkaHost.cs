using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cheburashka.BE;
using Cheburashka.Extensions;

namespace CheburashkaHots
{
    public class CheburashkaHost : Host
    {
        public CheburashkaHost()
        {
        }

        public override Task OnConnect(Connection e)
        {
            throw new NotImplementedException();
        }

        public override Task OnDisconnect((Connection, Exception) info)
        {
            throw new NotImplementedException();
        }

        public override Task OnMessage((Connection, BaseMessage) info)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Host
    {
        private HostOptions _options;

        private Connections _connection = new Connections();

        public IConnectionManager ConnectionManager { get => _connection; }

        public Host()
        {
            ConnectionManager.OnConnected += (obj, e) => OnConnect(e);
            ConnectionManager.OnDisconnected += (obj, e) => OnDisconnect(e);
            ConnectionManager.OnMessage += (obj, e) => OnMessage(e);
        }

        public abstract Task OnConnect(Connection e);

        public abstract Task OnDisconnect((Connection, Exception) info);

        public abstract Task OnMessage((Connection, BaseMessage) info);

        public async Task RunAsync()
        {
            try
            {
                var listener  = new TcpListener(IPAddress.Any,_options.Port);
                listener.Start();
                while(true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _connection.Add(new global::Connection(client, Guid.NewGuid()));
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public Host Configure(Action<HostOptions> action)
        {
            var options = new HostOptions();
            action(options);
            _options = options;
            return this;
        }
    }

    public class HostOptions
    {
        public int Port { get; internal set; }
    }

}

public interface IObservable<T>
{
    void Subscribe(IObserver<T> observer);
}

public interface IObserver<T>
{
    void OnError(T obj, Exception error);
    void OnMessage(T obj, BaseMessage message);
}

public interface IConnectionManager
{
    event EventHandler<Connection> OnConnected;
    event EventHandler<(Connection, Exception)> OnDisconnected;
    event EventHandler<(Connection,BaseMessage)> OnMessage;
}

public class Connections : IObserver<Connection>, IConnectionManager
{
    private ConcurrentDictionary<Guid,Connection> _connections;

    public event EventHandler<Connection> OnConnected;
    public event EventHandler<(Connection, Exception)> OnDisconnected;
    public event EventHandler<(Connection,BaseMessage)> OnMessageManager;

    event EventHandler<(Connection,BaseMessage)> IConnectionManager.OnMessage
    {
        add
        {
            OnMessageManager += value;
        }

        remove
        {
            OnMessageManager += value;
        }
    }

    public void Add(Connection connection)
    {
        _connections.TryAdd(connection.Id, connection);
        connection.Subscribe(this);
        OnConnected?.Invoke(this, connection);
    }

    public void OnError(Connection obj, Exception error)
    {
        _connections.TryRemove(obj.Id, out _);
        OnDisconnected?.Invoke(this, (obj, error));
    }

    public void OnMessage(Connection obj, BaseMessage message)
    {
        OnMessageManager?.Invoke(this, (obj, message));
    }
}

public class Connection : IObservable<Connection>, IDisposable
{
    private IObserver<Connection> _observer;
    private TcpClient _client;

    public Guid Id { get; private set; }

    public Connection(TcpClient client, Guid id)
    {
        Id = id;
        _client = client;
        Receive();
    }

    private void Receive()
    {
        Task.Run(async () =>
        {
            try
            {
                using(var stream = _client.GetStream())
                {
                    while(true)
                    {
                        var message = await stream.GetMessage();
                        _observer.OnMessage(this, message);
                        //_observer.OnMessage();
                    }
                }
            }
            catch(Exception ex)
            {
                _observer.OnError(this, ex);
            }

        });
    }

    public async Task Send(BaseMessage message)
    {
        try
        {
            using(var stream = _client.GetStream())
            {
                var bytes = await message.GetBytes();
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        catch(Exception ex)
        {
            _observer.OnError(this, ex);
        }
    }

    public void Subscribe(IObserver<Connection> observer)
    {
        _observer = observer;
    }

    public void Dispose()
    {
        _observer = null;
        _client?.Dispose();
        _client = null;
    }
}