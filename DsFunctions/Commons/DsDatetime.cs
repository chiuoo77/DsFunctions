namespace DsFunctions.Commons
{
    public class DsDatetime
    {
        /// <summary>
        /// 파라미터 정보에 따라 DateTime을 반환하는 함수
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="Date"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static DateTime GetDateTimes(string DateTimes, string Date, string Time)
        {
            try
            {
                if (!string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Time))
                {
                    DateTime.TryParse(string.Format("{0} {1}", Date, Time), out DateTime dt);
                    return dt;
                }
                else if (!string.IsNullOrEmpty(DateTimes))
                {
                    return DateTime.Parse(DateTimes);
                }
            }
            catch (Exception ex)
            {

            }

            return new DateTime();
        }

        public static DateTime IntToDateTime(int intDate)
        {
            return new DateTime(1970, 1, 1, 9, 0, 0).AddSeconds(intDate);
        }

        public static uint DateTimeToInt(DateTime theDate)
        {
            return (uint)(theDate - new DateTime(1970, 1, 1, 9, 0, 0)).TotalSeconds;
        }

        public static DateTime CombineDateTime(DateTime dt, DateTime tm)
        {
            string strCombine = string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), tm.ToString("HH:mm:ss"));
            DateTime.TryParse(strCombine, out DateTime dtStudy);
            return dtStudy;
        }

        /// <summary>
        /// Millisecond를 DateTime 형식으로 변경해주는 함수
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeByMilliseconds(long date)
        {
            DateTime resultTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(date) + TimeSpan.FromHours(9);
            return resultTime;
        }

        /// <summary>
        /// DateTime을 Millisecond 형식으로 변경해주는 함수
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetMillisecondsByDateTime(DateTime date)
        {
            long resultTime = (long)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return resultTime;
        }

        /// <summary>
        /// 초를 분, 초로 변환해서 반환
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static string GetSecondToMinSecString(int Second)
        {
            int Min = Second % 3600 / 60;
            int Sec = Second % 3600 % 60;

            return string.Format("{0}:{1}", Min.ToString().PadLeft(2, '0'), Sec.ToString().PadLeft(2, '0'));
        }

        public static bool CheckTypeDateTime(string dt)
        {
            if (dt == null || dt == string.Empty) return false;
            try
            {
                DateTime tmp = DateTime.Parse(dt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 날짜형식이 아닌 것을 Date형태의 String변환
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static string GetDateToString(string strDate, string format = "yyyy-MM-dd")
        {
            if (strDate == null) return "";
            if (strDate.Length == 8)
            {
                string tmp = string.Empty;
                tmp = strDate.Substring(0, 4);
                tmp += "-";
                tmp += strDate.Substring(4, 2);
                tmp += "-";
                tmp += strDate.Substring(6, 2);

                DateTime.TryParse(tmp, out DateTime dt);
                if (dt != null)
                {
                    return DateTime.Parse(dt.ToString()).ToString(format);
                }
                else
                {
                    return "";
                }
            }
            else if (strDate.Length == 12)
            {
                // 년월일시분
                string tmp = string.Empty;
                tmp = strDate.Substring(0, 4);
                tmp += "-";
                tmp += strDate.Substring(4, 2);
                tmp += "-";
                tmp += strDate.Substring(6, 2);
                tmp += " ";
                tmp += strDate.Substring(8, 2);
                tmp += ":";
                tmp += strDate.Substring(10, 2);
                tmp += ":00";
                DateTime.TryParse(tmp, out DateTime dt);
                if (dt != null)
                {
                    return DateTime.Parse(dt.ToString()).ToString(format);
                }
                else
                {
                    return "";
                }
            }
            else if (strDate.Length == 14)
            {
                string tmp = string.Empty;
                tmp = strDate.Substring(0, 4);
                tmp += "-";
                tmp += strDate.Substring(4, 2);
                tmp += "-";
                tmp += strDate.Substring(6, 2);
                tmp += " ";
                tmp += strDate.Substring(8, 2);
                tmp += ":";
                tmp += strDate.Substring(10, 2);
                tmp += ":";
                tmp += strDate.Substring(12, 2);

                DateTime.TryParse(tmp, out DateTime dt);
                if (dt != null)
                {
                    return DateTime.Parse(dt.ToString()).ToString(format);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                DateTime.TryParse(strDate, out DateTime dt);
                return dt.ToString(format);
            }
        }

        public static string GetTimeToString(string strTime)
        {
            string strRet = string.Empty;
            if (strTime.IndexOf(".") > 0)
            {
                strTime = strTime.Substring(0, strTime.IndexOf("."));
            }

            bool chkDateTime = false;
            if (strTime.Length == 6)
            {
                string tmp = string.Empty;
                tmp = strTime.Substring(0, 2);
                tmp += ":";
                tmp += strTime.Substring(2, 2);
                tmp += ":";
                tmp += strTime.Substring(4, 2);
                strTime = tmp;
                chkDateTime = true;
            }
            else
            {
                try
                {
                    DateTime.Parse(strTime);
                    chkDateTime = true;
                }
                catch { }
            }

            if (chkDateTime)
            {
                strRet = DateTime.Parse(strTime).ToString("HH:mm:ss");
            }
            return strRet;
        }

        public static DateTime GetParseDateTime(string Date, string Time)
        {
            if (Date == null) Date = string.Empty;
            if (Time == null) Time = string.Empty;

            string strDate = GetDateToString(Date);
            string strTime = GetTimeToString(Time);

            DateTime value = DateTime.Parse("1900-01-01 00:00:00");
            DateTime.TryParse(strDate, out DateTime dtDate);
            DateTime.TryParse(strTime, out DateTime dtTime);

            if (strDate != string.Empty && strTime != string.Empty)
            {
                DateTime.TryParse(string.Format("{0} {1}", dtDate.ToString("yyyy-MM-dd"), dtTime.ToString("HH:mm:ss")), out value);
            }
            else if (strDate != string.Empty && strTime == string.Empty)
            {
                DateTime.TryParse(strDate, out value);
            }

            return value;
        }

        public static int GetAgeByDOB(string BirthDay, string standardDate = "", bool KoreanAgeType = true)
        {
            if (BirthDay is null || BirthDay == string.Empty) return 0;
            if (standardDate == "") standardDate = DateTime.Now.ToString("yyyy-MM-dd");
            if (BirthDay.Length == 8)
            {
                BirthDay = string.Format("{0}-{1}-{2}", BirthDay.Substring(0, 4), BirthDay.Substring(4, 2), BirthDay.Substring(6, 2));
            }

            BirthDay = DateTime.Parse(BirthDay).ToString("yyyy-MM-dd");

            int Age;
            int BirthYear = Convert.ToInt32(BirthDay.Substring(2, 2));

            if (Convert.ToInt32(BirthDay.Substring(0, 4)) > 2100) return 0;

            int NowYear = Convert.ToDateTime(standardDate).Year;

            if (BirthDay.Substring(0, 2) == "19")
            {
                Age = NowYear - (1900 + BirthYear);
            }
            else
            {
                Age = NowYear - (2000 + BirthYear);
            }

            if (KoreanAgeType)
            {
                int BirthMonth = Convert.ToInt32(BirthDay.Substring(5, 2));
                int nowMonth = Convert.ToDateTime(standardDate).Month;

                if (BirthMonth == nowMonth)
                {
                    int BirthDays = Convert.ToInt32(BirthDay.Substring(8, 2));
                    int nowDay = Convert.ToDateTime(standardDate).Day;

                    if (BirthDays <= nowDay)
                        Age++;
                }
                else if (BirthMonth < nowMonth)
                    Age++;
            }

            return Age;
        }
    }
}