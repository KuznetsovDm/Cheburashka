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

namespace CheburashkaHots
{
    public class CheburashkaHost
    {

        public virtual Task OnDisconnect()
        {
            throw new NotImplementedException();
        }
    }

    public class Host
    {
        private static HostOptions _options;

        public static event EventHandler<TcpClient> OnAcceptConnection;

        public static ConcurrentDictionary<Guid, TcpClient> _clients = new ConcurrentDictionary<Guid, TcpClient>();

        public async Task RunAsync()
        {
            try
            {
                var listener  = new TcpListener(IPAddress.Any,_options.Port);

                while(true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _clients.TryAdd(Guid.NewGuid(), client);
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


public interface IHost
{
    //IConnections Connections { get; }
}

public interface IObservable
{
    void Subscribe(IObserver observer);
}

public interface IObserver
{
    void OnError(IObservable observable, Exception error);
    void OnMessage(IObservable observable);
}

public class Connection : IObservable
{
    private IObserver _observer;
    private byte[] _buffer;

    public Connection(TcpClient client)
    {
        Receive(client);
        _buffer = new byte[1025];
    }

    private void Receive(TcpClient client)
    {
        Task.Run(() =>
        {
            try
            {
                using(var memory = new MemoryStream())
                using(var stream = client.GetStream())
                {
                    while(true)
                    {
                        stream.ReadAsync(_buffer, 0, _buffer.Length);
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

    public void Subscribe(IObserver observer)
    {
        _observer = observer;
    }
}