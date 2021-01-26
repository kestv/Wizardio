using Assets.Scripts.Api.Dto;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Api.Controllers
{
    public abstract class HttpController
    {
        protected const string Url = "http://localhost:5000/api";
        protected const string ContentType = "application/json";

        protected string Get(string controller, object value)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}/{1}/{2}", Url, controller, value));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            jsonResponse = new string(jsonResponse.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
            return jsonResponse;
        }

        protected string Post(string controller, object entity)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}/{1}", Url, controller));
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = ContentType;
            string entityJson = JsonUtility.ToJson(entity);
            byte[] buffer = Encoding.UTF8.GetBytes(entityJson);
            using (var stream = request.GetRequestStream()) {
                stream.Write(buffer, 0, buffer.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            if (jsonResponse != null)
            {
                return jsonResponse;
            }
            return null;
        }

        protected string Put(string controller, object entity)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}/{1}", Url, controller));
            request.Method = WebRequestMethods.Http.Put;
            request.ContentType = ContentType;
            string entityJson = JsonUtility.ToJson(entity);
            byte[] buffer = Encoding.UTF8.GetBytes(entityJson);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            if (jsonResponse != null)
            {
                return jsonResponse;
            }
            return null;
        }
    }
}
