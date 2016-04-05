using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using MalApi;
using jaMAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SGEnviro;
using System.Xml;
using System.Web;
using SGEnviro.Forecasts;

namespace animeBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        int count = 0;
        string searchText="";
        public static string CreateRequest(string message)
        {
            string UrlRequest = "http://myanimelist.net/api/anime/search.xml?q="+message;
            return UrlRequest;
        }//end of cr8request
       // [ActionName("route")]
        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request2 = WebRequest.Create(requestUrl) as HttpWebRequest;
                request2.Credentials = new NetworkCredential("BambiNuggets", "MyAnimeListAcc78621!");
                HttpWebResponse response = request2.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.Read();
                return null;
            }
        }//end of make request

        //public void ProcessResponse(XmlDocument animeListResponse)
        //{
        //    if (animeListResponse != null)
        //    {
        //        //    lbl.Text = "Call Success";
        //    }
        //    else
        //    {
        //        //    lbl.Text = "Call Fail";
        //    }

        //    XmlNodeList titleList = animeListResponse.GetElementsByTagName("title");
        //    XmlNodeList synopsisList = animeListResponse.GetElementsByTagName("synopsis");

        //    //StringBuilder sb = new StringBuilder();

        //    //for (int i = 0; i < titleList.Count; i++)
        //    //{
        //        //string builder messes up the output
        //        /*
        //        sb.AppendFormat("\r\n{0})  Title: {1}", i, titleList[i].InnerXml);
        //        sb.AppendFormat("\r\n  Synopsis: {0}", synopsisList[i].InnerXml);

        //        txtArea.AppendText(sb.ToString());*/

        //        searchText += ((0 + 1) + ") Title: " + titleList[0].InnerXml + "\r\n");
        //        //txtArea.AppendText("Synopsis:  " + synopsisList[i].InnerXml + "\r\n");
        //        //txtArea.AppendText("Synopsis:  " + formatSynopsis(synopsisList[i]) + "\r\n");
        //        searchText += ("Synopsis:  " + decodeHtml(synopsisList[0].InnerXml) + "\r\n");
        //        searchText += ("-----------------------------------------------------------------------------\r\n");

        //   // }

        //} //end of ProcessResponse

        ////public string formatSynopsis(XmlNode node)
        ////{
        ////    StreamReader streamReader = new StreamReader(node.InnerXml);
        ////    string text = streamReader.ReadToEnd();
        ////    streamReader.Close();

        ////    string pattern = "[&amp;][&lt;][&gt;][&quot;][&apos;]";
        ////    return Regex.Replace(text, @"[^ a - zA - Z0 - 9_.]", "HI", RegexOptions.Compiled);
        ////} //end of formatSynopsis

        public static string decodeHtml(string text)
        {
            return WebUtility.HtmlDecode(text);
        }//end of decode html
        public async Task<Message> Post([FromBody]Message message)
        {
            string finalOutput="";
            if (message.Type == "Message")
            {

                //if (message.Text=="Search")
                //{
                //    ////Create the REST Services 'Find Location by Query' request
                //    string animeListRequest = CreateRequest();
                //    XmlDocument animeListResponse = MakeRequest(animeListRequest);
                //    ProcessResponse(animeListResponse);
                //    return message.CreateReplyMessage(searchText);
                //}
                if (message.Text == "Help")
                {
                    return message.CreateReplyMessage("Currently you may retrieve the anime list of a person from MAL by typing their name or finding out the PSI of the north region by typing PSI,enjoy");
                }
                else if (message.Text == "Romance")
                {
                    return message.CreateReplyMessage("Shigatsu Wa Kimi no Uso");
                }

                else if (message.Text == "PSI")
                {
                    string response = "";
                    var api = new SGEnviroApi("781CF461BB6606ADEA01E0CAF8B352742F03D1821750BC04");
                    Pm25Update result = await api.GetPm25UpdateAsync();
                    response = "PSI for North Region is " + result.North.Reading;
                    return message.CreateReplyMessage(response);
                }
                //MAL anime list search retrieval
                //using (MyAnimeListApi api = new MyAnimeListApi())
                //{
                //    api.UserAgent = "my_app"; // MAL now requires applications to be whitelisted. Whitelisted applications identify themselves by their user agent.

                //    MalUserLookupResults userLookup = api.GetAnimeListForUser(message.Text);
                //    foreach (MyAnimeListEntry listEntry in userLookup.AnimeList)
                //    {
                //        finalOutput += (listEntry.AnimeInfo.Title + " | " + listEntry.Score + ". ");

                //    }

                //    return message.CreateReplyMessage(string.Concat(finalOutput.Take(1000)));
                //}

                //Create the REST Services 'Find Location by Query' request
                else {
                    try
                    {
                        string animeListRequest = CreateRequest(message.Text);
                        XmlDocument animeListResponse = MakeRequest(animeListRequest);
                        //    ProcessResponse(animeListResponse);
                        XmlNodeList titleList = animeListResponse.GetElementsByTagName("title");
                        XmlNodeList synopsisList = animeListResponse.GetElementsByTagName("synopsis");
                        //for (int i = 0; i < titleList.Count; i++)
                        //{
                        //string builder messes up the output
                        /*
                        sb.AppendFormat("\r\n{0})  Title: {1}", i, titleList[i].InnerXml);
                        sb.AppendFormat("\r\n  Synopsis: {0}", synopsisList[i].InnerXml);

                        txtArea.AppendText(sb.ToString());*/

                        searchText += ((1) + ") Title: " + titleList[0].InnerXml + "\r\n");
                        //txtArea.AppendText("Synopsis:  " + synopsisList[i].InnerXml + "\r\n");
                        //txtArea.AppendText("Synopsis:  " + formatSynopsis(synopsisList[i]) + "\r\n");
                        searchText += ("Synopsis:  " + decodeHtml(synopsisList[0].InnerXml) + "\r\n");
                        searchText += ("-----------------------------------------------------------------------------\r\n");

                        //}
                        return message.CreateReplyMessage(searchText);
                    }
                    catch
                    {
                        return message.CreateReplyMessage("searchfailed");
                    }
                }


            }

            else
            {
                return HandleSystemMessage(message);
            }
        }
    
        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}