using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalPipeComm
{

    public class PipeClient
    {
        private NamedPipeClientStream pipeStream;
        public event EventHandler<PipeMessageEventArgs> MessageRecieved;

        public PipeClient(string PipeName)
        {
            pipeStream = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            pipeStream.Connect(10000);
        }

        public void StartListening()
        {
            Task.Run(Listen);
        }

        public async Task Listen()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 16];
                var read = await pipeStream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0) break;
                int count = Array.IndexOf<byte>(buffer, 0, 0);
                if (count < 0) count = buffer.Length;
                var message = Encoding.ASCII.GetString(buffer, 0, count);
                MessageRecieved?.Invoke(this, new PipeMessageEventArgs() { Message = message });
            }
        }

        public async Task<bool> Send(string Message)
        {
            try
            {
                if (pipeStream.IsConnected)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(Message);
                    await pipeStream.WriteAsync(buffer, 0, buffer.Length);
                    await pipeStream.FlushAsync();
                    pipeStream.WaitForPipeDrain();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

    public class PipeMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
