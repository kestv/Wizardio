using Assets.Scripts.Api.Dto;
using Assets.Scripts.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Api.Controllers
{
    public class GameSessionController : HttpController
    {
        string controller = "GameSessions";
        public Response<List<GameSession>> GetAvailableGameSessions()
        {
            var json = Get(controller, "");
            var response = JsonUtility.FromJson<Response<List<GameSession>>>(json);
            if (!response.hasErrors)
            {
                response.hasErrors = true;
                response.message = "Could not parse game session entity";
            }
            return response;
        }

        public void AddPlayerToSession(int _clientId, GameSession _session)
        {
            _session.PlayersCount += 1;
            Put(controller, _session);
        }

        public Response<GameSession> AddNewSession(GameSession _session) => JsonUtility.FromJson<Response<GameSession>>(Post(controller, _session));
    }
}
