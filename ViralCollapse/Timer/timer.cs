using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViralCollapse.Models;

namespace ViralCollapse.Timer
{
    public static class timer
    {
        private static Dictionary<string, double[]> dic = new Dictionary<string, double[]>();
        private static List<EchartsModel> pronvincedic = new List<EchartsModel>();
        private static List<EchartsModel> echartsModels = null;
        private static List<EchartsMapModel> echartsMapModels = null;


        public static void getData(object source, System.Timers.ElapsedEventArgs e)
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://view.inews.qq.com/") };
            HttpResponseMessage httpResponseMessage = httpClient.GetAsync("g2/getOnsInfo?name=disease_h5").GetAwaiter().GetResult();
            var result = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            nCoVData data = JsonConvert.DeserializeObject<nCoVData>(result);
            nCoVDataDetail nCoVDataDetail = JsonConvert.DeserializeObject<nCoVDataDetail>(data.Data);
            GetPositionOfJson(nCoVDataDetail.AreaTree);
            SaveToFile(JsonConvert.SerializeObject(dic, Formatting.Indented));
        }

        public static void LoadMap()
        {
            Dictionary<string, double[]> tempdic = ReadToFile();
            if (tempdic != null)
            {
                dic = tempdic;
            }
        }

        public static Dictionary<string, double[]> getAddress(){
            return dic;
        }
        public static List<EchartsMapModel> getProvinceData()
        {
            return echartsMapModels;
        }

        public static List<EchartsModel> getProvinceMapData()
        {
            return pronvincedic;
        }

        public static List<EchartsModel> getCityData()
        {
            return echartsModels;
        }

        private static Dictionary<string, double[]> ReadToFile()
        {
            string readPath= AppDomain.CurrentDomain.BaseDirectory + "data.json";
            if (File.Exists(readPath))
            {
               String Json= System.IO.File.ReadAllText(readPath);
               return JsonConvert.DeserializeObject<Dictionary<string, double[]>>(Json);
            }
            return null;
        }

        private static void SaveToFile(string data)
        {
            var writePath = AppDomain.CurrentDomain.BaseDirectory + "data.json";
            if (!File.Exists(writePath))
            {
                using (FileStream fs = new FileStream(writePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(data);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(writePath, FileMode.Open, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(data);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        private static void GetPositionOfJson(IList<AreaTree> areaTrees)
        {
            // 考虑到查询成本
            echartsModels = new List<EchartsModel>();
            echartsMapModels = new List<EchartsMapModel>();
            pronvincedic =new List<EchartsModel>();
            EchartsModel echartsModel = new EchartsModel();
            EchartsMapModel echartsMapModel = new EchartsMapModel();
            foreach (var country in areaTrees)
            {
                var CountryName = country.Name;
                //|| CountryName != "台湾
                if (CountryName != "中国")
                {
                    //only china 其他国家不管
                    continue;
                }
               
                foreach (var province in country.Children)
                {
                    var provinceRank = "省";
                    if (province.Name == "北京" || province.Name == "天津" || province.Name == "上海" || province.Name == "重庆")
                    {
                        provinceRank = "市";
                    }
                    var provinceName = province.Name + provinceRank;

                    //加入统计 因为西藏省区
                    if (!dic.Keys.Contains(province.Name))
                    {
                        var fullName = provinceName + province.Name;
                        mapv3 mapv3 = GetHttpRequest(fullName).Result;
                        if (mapv3.status == 0)
                        {
                            var location = mapv3.result.location;
                            double[] d = new double[] { location.lng, location.lat };
                            dic.Add(province.Name, d);
                        }
                    }
                    EchartsModel echartsModelPClone = (EchartsModel)echartsModel.clone();
                    echartsModelPClone.name = province.Name;
                    echartsModelPClone.value = province.Total.Confirm;
                    pronvincedic.Add(echartsModelPClone);

                    if (province.Name == "西藏"|| province.Name == "台湾"|| province.Name == "香港" || province.Name == "澳门")
                    {
                        fullPoint(echartsModel, province.Name, provinceName,province.Total.Confirm);
                    }

                    //颜色
                    EchartsMapModel echartsMapModelClone = (EchartsMapModel)echartsMapModel.clone();
                    echartsMapModelClone.name = province.Name;
                    //总感染人数
                    EchartsModel InfectSum = (EchartsModel)echartsModel.clone();
                    InfectSum.name = "感染人数";
                    InfectSum.value = province.Total.Confirm;
                    //治愈人数
                    EchartsModel CureSum = (EchartsModel)echartsModel.clone();
                    CureSum.name = "治愈人数";
                    CureSum.value = province.Total.Heal;
                    //沉重的死亡人数
                    EchartsModel HeadSum = (EchartsModel)echartsModel.clone();
                    HeadSum.name = "死亡人数";
                    HeadSum.value = province.Total.Dead;
                    List<EchartsModel> echartsModelMapList = new List<EchartsModel>();
                    echartsModelMapList.Add(InfectSum);
                    echartsModelMapList.Add(CureSum);
                    echartsModelMapList.Add(HeadSum);
                    echartsMapModelClone.value = echartsModelMapList;
                    echartsMapModels.Add(echartsMapModelClone);
                    foreach (var city in province.children)
                    {
                        var fullName = provinceName + city.Name;

                        fullPoint(echartsModel, city.Name, fullName, city.Total.Confirm);
                        
                        
                    }
                }
            }
        }

        private static void fullPointTest(EchartsModel echartsModel, string name, string where, int confirm)
        {
            //if (!dic.Keys.Contains(name))
            //{

            mapv3 mapv3 = GetHttpRequest(where).Result;
            if (mapv3.status == 0)
            {
                var location = mapv3.result.location;
                double[] d = new double[] { location.lng, location.lat };
                dic.Add(name, d);
            }
            //}
            //后续需要优化部分
            EchartsModel echartsModelClone = (EchartsModel)echartsModel.clone();
            echartsModelClone.name = name;
            echartsModelClone.value = confirm;
            echartsModels.Add(echartsModelClone);
        }

        private static void fullPoint(EchartsModel echartsModel, string name,string where,int confirm)
        {
            if (!dic.Keys.Contains(name))
            {

                mapv3 mapv3 =  GetHttpRequest(where).Result;
                if (mapv3.status == 0)
                {
                    var location = mapv3.result.location;
                    double[] d = new double[] { location.lng, location.lat };
                    dic.Add(name, d);
                }
            }
            //后续需要优化部分
            EchartsModel echartsModelClone = (EchartsModel)echartsModel.clone();
            echartsModelClone.name = name;
            echartsModelClone.value = confirm;
            echartsModels.Add(echartsModelClone);
        }

        private static async Task<mapv3> GetHttpRequest(string areaAddress)
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://api.map.baidu.com/") };
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("geocoding/v3/?address=" + areaAddress + "&output=json&ak=SqEg0trDj2ajPGoxtQQHVSa9nAh3ChKS");
            var result = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            mapv3 data = JsonConvert.DeserializeObject<mapv3>(result);
            return data;
        }
    }
}
