using ReposCore.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ReposServices.Caching
{

    public enum CacheTimes
    {
        OneMinute = 1,
        OneHour = 60,
        TwoHours = 120,
        SixHours = 360,
        TwelveHours = 720,
        OneDay = 1440
    }

    public partial interface ICacheService
        : ICacheManager
    {

    }
}