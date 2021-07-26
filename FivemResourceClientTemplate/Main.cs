using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;

namespace FivemResourceClientTemplate
{
    public class Main : BaseScript
    {
        public Main()
        {
            //Evento personalizado
            EventHandlers["template:connected"] += new Action<string, string, string>(OnConnected);

            //Um evento que é enfileirado após o início de um recurso.
            EventHandlers["onClientResourceStart"] += new Action<string>(OnStart);
        }

        //Evento template:connected que é disparado pelo servidor.
        //Exemplo de como trocar dados entre client e server.
        //Ver OnJoin no server.
        private void OnConnected(string arg1, string arg2, string arg3)
        {
            Debug.WriteLine("Data received from server...");
            Debug.WriteLine($"name:{arg1}");
            Debug.WriteLine($"steam:{arg2}");
            Debug.WriteLine($"handle:{arg3}");
        }

        //Evento que acontece quando o resouce inicializa do lado do client.
        private void OnStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine($"{DateTime.Now} - Starting resource...{resourceName}");

            API.RegisterCommand("ping", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // TODO: make a vehicle! fun!
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "Me", $"pong!" }
                });
            }), false);
        }
    }
}
