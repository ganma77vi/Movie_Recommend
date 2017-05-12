using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Web.Syndication;
using Newtonsoft.Json;
using System.Net.Http;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BackgroundTasks
{
    public sealed class BlogFeedBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
                Datum1[] RecentMovieListResult=null;
                Rootobject2 City =null;
                Rootobject recentmovielist;
                try
                {
                    string city = await HttpRequest.GetCity();
                    if (city != "0" && city != "1" && city != "2")
                        City = JsonConvert.DeserializeObject<Rootobject2>(city);
                    else
                        RecentMovieListResult=null;
                    if (City.regeocode.addressComponent.city == null)
                    {
                        city = City.regeocode.addressComponent.province;
                    }
                    else
                    {
                        city = City.regeocode.addressComponent.city.ToString();
                    }
                    string json = await HttpRequest.GetRecentMovieInformationRequest(city);
                    json = json.Replace("\"1\"", "a");
                    json = json.Replace("\"2\"", "b");
                    json = json.Replace("\"3\"", "c");
                    json = json.Replace("\"4\"", "d");
                    json = json.Replace("\"5\"", "e");
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        recentmovielist = JsonConvert.DeserializeObject<Rootobject>(json);
                        switch (recentmovielist.error_code)
                        {
                            case 0:
                                {
                                    RecentMovieListResult = recentmovielist.result.data[0].data;
                                    break;
                                }
                            #region 服务器错误码
                            case 209403:
                                {
                                    Debug.WriteLine("网络错误，请重试");
                                    break;
                                }
                            case 209405:
                                {
                                    Debug.WriteLine("查询不到热映电影相关信息");
                                    break;
                                }
                            case 209404:
                                {
                                    Debug.WriteLine("定位异常，城市名为空");
                                    break;
                                }
                            #endregion
                            #region 系统级错误码
                            case 10001:
                                {
                                    Debug.WriteLine("错误的请求KEY,请联系开发人员");
                                    break;
                                }
                            case 10002:
                                {
                                    Debug.WriteLine("该KEY无请求权限,请联系开发人员");
                                    break;
                                }
                            case 10003:
                                {
                                    Debug.WriteLine("KEY过期,请联系开发人员");
                                    break;
                                }
                            case 10004:
                                {
                                    Debug.WriteLine("错误的OPENID");
                                    break;
                                }
                            case 10005:
                                {
                                    Debug.WriteLine("应用未审核超时，请提交认证，请联系开发人员");
                                    break;
                                }
                            case 10007:
                                {
                                    Debug.WriteLine("未知的请求源");
                                    break;
                                }
                            case 10008:
                                {
                                    Debug.WriteLine("被禁止的IP");
                                    break;
                                }
                            case 10009:
                                {
                                    Debug.WriteLine("被禁止的KEY");
                                    break;
                                }
                            case 10011:
                                {
                                    Debug.WriteLine("当前IP请求超过限制");
                                    break;
                                }
                            case 10012:
                                {
                                    Debug.WriteLine("请求超过次数限制");
                                    break;
                                }
                            case 10013:
                                {
                                    Debug.WriteLine("测试KEY超过请求限制");
                                    break;
                                }
                            case 10014:
                                {
                                    Debug.WriteLine("系统内部异常");
                                    break;
                                }
                            case 10020:
                                {
                                    Debug.WriteLine("接口维护");
                                    break;
                                }
                            case 10021:
                                {
                                    Debug.WriteLine("接口停用");
                                    break;
                                }
                                #endregion
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    Debug.WriteLine("动态磁贴后台任务出现网络异常");
                    return;
                }
                catch (Exception)
                {
                    Debug.WriteLine("动态磁贴后台任务出现异常");
                    return;
                }
                UpDater.UpdateTile(RecentMovieListResult);
                deferral.Complete();
            }
            catch
            {
                Debug.WriteLine("后台任务出现异常");
            }
        }
    }
}
