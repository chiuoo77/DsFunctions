using System.Reflection;

namespace DsFunctions.Commons
{
    public class DsVersion
    {
        public enum APP_VERSION
        {
            SIMPLE,
            BUILD,
            RELEASE
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appVer"></param>
        /// <param name="ProductVersion">Application.ProductVersion</param>
        /// <param name="buildDate"></param>
        /// <returns></returns>
        public static string ProcGetAppVer(APP_VERSION appVer, string ProductVersion, bool buildDate = false)        // 현재 실행 중인 프로그램의 버전을 가져오는 함수
        {
            Assembly assemObj = Assembly.GetExecutingAssembly();
            Version v = assemObj.GetName().Version;

            string strRet;
            switch (appVer)
            {
                case APP_VERSION.BUILD:
                    if (buildDate)
                    {
                        strRet = string.Format("{0}.{1:C2}.{2}.{3} ({4})", v.Major.ToString(), v.Minor.ToString(), v.Build.ToString(), v.Revision.ToString(), Get_BuildDateTime(v).ToString("yyyyMMdd"));
                    }
                    else
                    {
                        strRet = string.Format("{0}.{1:C2}.{2}.{3}", v.Major.ToString(), v.Minor.ToString(), v.Build.ToString(), v.Revision.ToString());
                    }
                    break;
                case APP_VERSION.RELEASE:
                    if (buildDate)
                    {
                        strRet = string.Format("{0} ({1})", ProductVersion, Get_BuildDateTime(v).ToString("yyyyMMdd"));
                    }
                    else
                    {
                        strRet = string.Format("{0}", ProductVersion);
                    }

                    break;
                default:
                    strRet = string.Format("{0}.{1:c2}", v.Major.ToString(), v.Minor.ToString());
                    break;
            }
            return strRet;
        }

        private static DateTime Get_BuildDateTime(Version version = null)
        {
            // 주.부.빌드.수정
            // 주 버전    Major Number
            // 부 버전    Minor Number
            // 빌드 번호  Build Number
            // 수정 버전  Revision NUmber

            //세번째 값(Build Number)은 2000년 1월 1일부터
            //Build된 날짜까지의 총 일(Days) 수 이다.
            int day = version.Build;
            DateTime dtBuild = new DateTime(2000, 1, 1).AddDays(day);

            //네번째 값(Revision NUmber)은 자정으로부터 Build된
            //시간까지의 지나간 초(Second) 값 이다.
            int intSeconds = version.Revision;
            intSeconds = intSeconds * 2;
            dtBuild = dtBuild.AddSeconds(intSeconds);

            //시차 보정
            System.Globalization.DaylightTime daylingTime = TimeZone.CurrentTimeZone
                    .GetDaylightChanges(dtBuild.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuild, daylingTime))
                dtBuild = dtBuild.Add(daylingTime.Delta);

            return dtBuild;
        }
    }
}
