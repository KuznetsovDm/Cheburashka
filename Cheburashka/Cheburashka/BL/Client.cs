using Cheburashka.BE;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

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
            return await Task.Run(() =>
            {
                var formatter = new BinaryFormatter();

                return (BaseMessage)formatter.Deserialize(_stream);
            });
            //var data = new byte[64]; // буфер для получаемых данных
            //StringBuilder builder = new StringBuilder();
            //int bytes = 0;
            //do
            //{
            //    bytes = await _stream.ReadAsync(data, 0, data.Length);
            //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            //}
            //while (_stream.DataAvailable);
        }

        private async Task SendData(string data, string method)
        {
            await Task.Run(() =>
            {
                var formatter = new BinaryFormatter();
                var message = new BaseMessage
                {
                    Success = true,
                    Data = data,
                    Method = method
                };

                formatter.Serialize(_stream, message);
            });
        }
    }
}
