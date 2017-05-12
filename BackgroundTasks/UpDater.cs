
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    class UpDater
    {
        public static void UpdateTile(Datum1[] MovieList)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            string strxml = "<tile>" +
                                "<visual>" +
                                    "<binding template=\"TileMedium\">" +
                                        "<text hint-style=\"caption\"></text>" +//电影名
                                        "<text hint-style=\"captionSubtle\"></text>" +//电影类型
                                        "<text hint-style=\"captionSubtle\"></text>" +//电影上映日期
                                    "</binding>" +
                                    "<binding template=\"TileWide\">" +
                                        "<group>" +
                                            "<subgroup hint-weight=\"35\">" +
                                                "<image src=\"{0}\"/>" +//电影图地址
                                            "</subgroup>" +
                                            "<subgroup>" +
                                                "<text hint-style=\"caption\"></text>" +//电影名
                                                "<text hint-style=\"captionSubtle\"></text>" +//导演
                                                "<text hint-style=\"captionSubtle\"></text>" +//主演
                                                "<text hint-style=\"captionSubtle\"></text>" +//电影上映日期
                                            "</subgroup>" +
                                         "</group>" +
                                      "</binding>" +
                                      "<binding template=\"TileLarge\">" +
                                        "<group>" +
                                            "<subgroup hint-weight=\"50\">" +
                                                "<image  src=\"{0}\" />" +//电影图地址
                                            "</subgroup>" +
                                            "<subgroup>" +
                                                "<text hint-style=\"caption\"></text>" +//电影名
                                                "<text hint-style=\"captionSubtle\"></text>" +//导演
                                                "<text hint-style=\"captionSubtle\"></text>" +//主演
                                                "<text hint-style=\"captionSubtle\"></text>" +//电影类型
                                            "</subgroup>" +
                                        "</group>" +
                                        "<text hint-style=\"captionSubtle\"></text>" +//上映日期
                                     "</binding>" +
                                  "</visual>" +
                               "</tile>";
            if (MovieList != null)
            {
                for (int i = 0; i < MovieList.Length && i <= 5; i++)
                {
                    XmlDocument doc = new XmlDocument();
                    strxml = strxml.Replace("{0}", MovieList[i].iconaddress);
                    doc.LoadXml(strxml);
                    XmlNodeList elements = doc.GetElementsByTagName("text");

                    elements[0].AppendChild(doc.CreateTextNode(MovieList[i].tvTitle));
                    elements[1].AppendChild(doc.CreateTextNode(MovieList[i].type.data.a.name + " " + MovieList[i].type.data.b.name));
                    elements[2].AppendChild(doc.CreateTextNode(MovieList[i].playDate.data));

                    elements[3].AppendChild(doc.CreateTextNode(MovieList[i].tvTitle));
                    elements[4].AppendChild(doc.CreateTextNode("导演:" + MovieList[i].director.data.a.name));
                    elements[5].AppendChild(doc.CreateTextNode("主演:" + MovieList[i].star.data.a.name));
                    elements[6].AppendChild(doc.CreateTextNode(MovieList[i].playDate.data));

                    elements[7].AppendChild(doc.CreateTextNode(MovieList[i].tvTitle));
                    elements[8].AppendChild(doc.CreateTextNode("导演:" + MovieList[i].director.data.a.name));
                    elements[9].AppendChild(doc.CreateTextNode("主演:" + MovieList[i].star.data.a.name));
                    elements[10].AppendChild(doc.CreateTextNode(MovieList[i].type.data.a.name + " " + MovieList[i].type.data.b.name));
                    elements[11].AppendChild(doc.CreateTextNode(MovieList[i].playDate.data));

                    updater.Update(new TileNotification(doc));
                }
            } 
        }
    }
}
