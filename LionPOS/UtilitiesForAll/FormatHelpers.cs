using System;
using System.Linq;
using System.Globalization;
using System.Threading;
using LionPOSServiceContractModels;

using LionPOSServiceOperationLayer.InitiateSystemStartUp;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;

namespace UtilitiesForAll
{
    public class FormatHelpers
    {
        public SessionCM sessionObj { get; set; }
        public FormatHelpers()
        {
            
        }
        public FormatHelpers(SessionCM sessionObj)
        {
            this.sessionObj = sessionObj;
        }
        //HC 24-02-2016 Created
        public string errorRedirectURL(int logid)
        {
            return ConstantDictionaryCM.MaintenanceWebsiteURL_string.Replace("<logid>", logid.ToString());
        }
        public DateTime DateTimeNow()
        {
            
            string timeZoneInfoId = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.TimeZoneInfoId.title).Select(a => a.values).Single();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        }
        public string mySQLDateFormat(string date)
        {
            string shortFormat = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Short.title).Select(a => a.values).Single();
            return DateTime.ParseExact(date, shortFormat, null).ToString("yyyy-MM-dd");
        }
        public string DateFormatShort()
        {
            
            string shortFormat = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Short.title).Select(a => a.values).Single();
            return shortFormat;

        }
        public string DateFormatLong()
        {
            
            string longFormat = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Long.title).Select(a => a.values).Single();
            return longFormat;
        }
        public string DateInShortFormat(DateTime? date)
        {
            
            string shortFormat = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Short.title).Select(a => a.values).Single();
            return DateTimeNow().ToString(shortFormat);

        }
        public string DateInLongFormat(DateTime? date)
        {
            
            string longFormat = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Date_Format_Long.title).Select(a => a.values).Single();
            return DateTimeNow().ToString(longFormat);
        }
        public CultureInfo getCultureInformation()
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            return culture;
        }
        public void setCultureInformation()
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();

            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            //For Framework 4.5 , 4.6
            //CultureInfo.DefaultThreadCurrentCulture = culture;
            //CultureInfo.DefaultThreadCurrentUICulture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        public string getFormatedNumberWithCurrency(string number)
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            decimal parsed = decimal.Parse(number, CultureInfo.InvariantCulture);
            return string.Format(culture, "{0:c}", parsed);
        }
        public string getFormatedNumberWithoutDecimalPlace(string number)
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            decimal parsed = decimal.Parse(number, CultureInfo.InvariantCulture);
            return string.Format(culture, "{0:c0}", parsed);
        }
        public string getFormatedNumberWithoutCurrencyAndDecimalPlace(string number)
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            decimal parsed = decimal.Parse(number, CultureInfo.InvariantCulture);
            return string.Format(culture, "{0:#,#}", parsed);
        }
        public string getFormatedNumberWithCurrencyAndWithoutDecimalPlace(string number)
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            decimal parsed = decimal.Parse(number, CultureInfo.InvariantCulture);
            return string.Format(culture, "{0:c#,#}", parsed);
        }
        public string getDefaultCurrency()
        {
            
            string cultureInfo = sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Culture_Information.title).Select(a => a.values).Single();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureInfo);
            return culture.NumberFormat.CurrencySymbol;
        }
        public string getSettingsCurrency()
        {
            
            return sessionObj.settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Currency_Symbol.title).Select(a => a.values).Single();
        }
    }
}