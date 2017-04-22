using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Sadjad_AI_CSharpClient_2017
{
    class Program
    {
        static void Main(string[] args)
        {
            Ai AI = new Ai();

            var socket = IO.Socket(AI.ServerAdr);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected...");
                socket.Emit("init", AI.Token);
            });

            while(true)
            {
                socket.On("result", (data) =>
                {
                    Int32[] touch = new Int32[2];

                    AI.doTurn(ref touch);

                    socket.Emit("touch", AI.Token, touch[0], touch[1]);
                });
            }
        }
    }
}
