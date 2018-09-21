using Cheburashka.BE;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Cheburashka.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Cheburashka.BL
{
    public class Client
    {
        private string _login;
        private string _hostname;
        private int _port;
        private TcpClient _client;
        private NetworkStream _stream;

        public Client(string login, string hostname, int port)
        {
            _login = login;
            _hostname = hostname;
            _port = port;

            _client = new TcpClient();
            Task.Factory.StartNew(Process);
        }

        private async Task<BaseMessage> Process()
        {
            await _client.ConnectAsync(_hostname, _port);

            _stream = _client.GetStream();

            try
            {
                while (true)
                {
                    var result = await ReadData();
                    //
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _client.Close();
            }
        }

        private async Task<BaseMessage> ReadData()
        {
            var list = new List<byte[]>();
            int bytes = 0;
            do
            {
                var data = new byte[1024]; // буфер для получаемых данных
                bytes = await _stream.ReadAsync(data, 0, data.Length);
                list.Add(data);
            }
            while (_stream.DataAvailable);

            return MessageHelper.GetMessage(list.SelectMany(b => b).ToArray());
        }

        private async Task SendData(string data, string method)
        {
            var message = new BaseMessage
            {
                Success = true,
                Data = data,
                Method = method
            };

            var bytes = await message.GetBytes();
            int count = 1024;
            int offset = 0;
            do
            {
                await _stream.WriteAsync(bytes, offset, count);
                offset += count;
            } while (offset < bytes.Length);
        }

        public async Task Login(string login)
        {

        }
    }
}
