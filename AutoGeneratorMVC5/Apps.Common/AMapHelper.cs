/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： AMapHelper
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-12-30 21:07:22 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Common
{
    public class AMapHelper
    {
        public static double pi = 3.14159265358979324;
        public static double a = 6378245.0;
        public static double ee = 0.00669342162296594323;
        /**
            * 计算
            */
        public static double[] transform(double wgLat, double wgLon, double[] latlng)
        {

            double[] latlon = new double[2];
            if (outOfChina(wgLat, wgLon))
            {
                latlng[0] = wgLat;
                latlng[1] = wgLon;
                latlon = latlng;
                return latlon;
            }
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            latlng[0] = wgLat + dLat;
            latlng[1] = wgLon + dLon;
            latlon = latlng;
            return latlon;
        }
        private static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        private static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                    * pi)) * 2.0 / 3.0;
            return ret;
        }

        //GPS获取的数据转换成高德地图经纬度
        public static double[] GetAMapLoc(string lat, string lon)
        {
            //3851.399535 /100 = 38度
            var DegreeLat = Utils.StrToInt(lat.Split('.')[0], 0) / 100;
            var DegreeLon = Utils.StrToInt(lon.Split('.')[0], 0) / 100;
            //（3851.399535 - 38*100）= 51分
            var MinLat = (Utils.StrToInt(lat.Split('.')[0], 0) - DegreeLat * 100);
            var MinLon = (Utils.StrToInt(lon.Split('.')[0], 0) - DegreeLon * 100);
            //0.399535 * 60 =23.9721 秒
            var SecLat = (Utils.StrToDouble("0." + lat.Split('.')[1], 0) * 60).ToString().Split('.')[0];
            var SecLon = (Utils.StrToDouble("0." + lon.Split('.')[1], 0) * 60).ToString().Split('.')[0];
            //38 + （51/60）+(23/3600)  = 38.85638888888889
            var DblLat = DegreeLat + (Utils.StrToDouble(MinLat.ToString(),0) / 60) + (Utils.StrToDouble(SecLat.ToString(),0) / 3600);
            var DblLon = DegreeLon + (Utils.StrToDouble(MinLon.ToString(), 0) / 60) + (Utils.StrToDouble(SecLon.ToString(), 0) / 3600);
            //double[] darr = GPSUtil.gps84_To_Gcj02(DblLat, DblLon);
            //调用纠偏后返回
            return GPSUtil.transform(DblLat, DblLon);
            //return new double[] { DblLat, DblLon };
        }
    }
}
