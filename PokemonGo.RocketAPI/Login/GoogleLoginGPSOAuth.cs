﻿using DankMemes.GPSOAuthSharp;
using PokemonGo.RocketAPI.Exceptions;
using System.Collections.Generic;

namespace PokemonGo.RocketAPI.Login
{
    public static class GoogleLoginGPSOAuth
    {
        public static string DoLogin(string username, string password)
        {
            GPSOAuthClient client = new GPSOAuthClient(username, password);
            Dictionary<string, string> response = client.PerformMasterLogin();
            
            if(response.ContainsKey("Error"))
                throw new GoogleException(response["Error"]);

            //Todo: captcha/2fa implementation

            if(!response.ContainsKey("Auth"))
                throw new GoogleOfflineException();

            Dictionary<string, string> oauthResponse = client.PerformOAuth( response["Token"],
                "audience:server:client_id:848232511240-7so421jotr2609rmqakceuu1luuq0ptb.apps.googleusercontent.com",
                "com.nianticlabs.pokemongo",
                "321187995bc7cdc2b5fc91b11a96e2baa8602c62");

            if(!oauthResponse.ContainsKey("Auth"))
                throw new GoogleOfflineException();

            return oauthResponse["Auth"];
        }
    }
}
