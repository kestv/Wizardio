using Assets.Scripts.Api.Dto;
using Assets.Scripts.Api.Models;
using System;
using UnityEngine;

namespace Assets.Scripts.Api.Controllers
{
    public class UsersController : HttpController
    {
        string controller = "users";
        public Response<User> GetUser(long id)
        {
            return JsonUtility.FromJson<Response<User>>(Get(controller, id));
        }

        public Response<User> AddUser(User user)
        {
            return JsonUtility.FromJson<Response<User>>(Post(controller, user));
        }

        public Response<User> Login(string username)
        {
            var response = Get(String.Format("{0}/login", controller), username);
            return JsonUtility.FromJson<Response<User>>(response);
        }

        public void UpdateUser(User user)
        {
            Put(controller, user);
        }
    }
}
