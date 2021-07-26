using CitizenFX.Core;
using System;

namespace FivemResourceServerTemplate
{
    public class Main : BaseScript
    {
        public Main()
        {
            //Um evento do lado do servidor que é acionado quando um jogador está tentando se conectar.
            //Este evento pode ser cancelado para rejeitar o jogador instantaneamente, supondo que uma validação mal sucedida ocorra.
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);

            //Um evento do lado do servidor que é acionado quando um jogador tem um NetID atribuído.
            EventHandlers["playerJoining"] += new Action<Player>(OnJoin);
        }


        //Evento que acontece após o player conectar ao servidor.
        //Esse método dispara o evento "template:connected" e passsa as informações para o Client.
        private void OnJoin([FromSource] Player player)
        {
            Debug.WriteLine("Triggering ClientEvent");
            TriggerClientEvent("template:connected", player.Name, player.Identifiers["steam"], player.Handle);
        }

        //Evento que acontece quando um player está se conectando.
        //deferrals.done(). Libera o acesso ao client se for chamado sem argumentos. Se for chamado recebendo string, rejeita a conexão e informa a string recebida.
        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();//Obrigatório!
            await Delay(0); //Obrigatório!

            //obtem o id da Steam (SteamId)
            var steamIdentifier = player.Identifiers["steam"];

            //Se o identificador for nulo...
            if (string.IsNullOrWhiteSpace(steamIdentifier))
            {
                deferrals.done($"Não foi possível identificar a STEAM. Por favor verifique se a STEAM está aberta");
            }

            //Você pode criar uma função para fazer Log de acessos
            Debug.WriteLine($"^3>>>{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")} Nova conexão: {playerName} (Identificador: [{steamIdentifier}]|Licença:[{player.Identifiers["license"]}])^7");

            //Escrevendo na tela do client que está checando a conta...
            deferrals.update($"Olá {playerName}, estou checando sua conta...");

            bool resultadoUsuario = true; //Assumindo que houve uma validação no usuário e deu certo...

            //Se a validação não der certo...
            if (!resultadoUsuario)
            {
                deferrals.done($"Usuário não foi encontrado na base.");
            }

            //Libera o acesso do client ao server...
            deferrals.done();
        }
    }
}
