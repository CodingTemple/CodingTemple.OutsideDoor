using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Twilio.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace CodingTemple.OutsideDoor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            DayOfWeek[] Weekdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };


            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");


            DateTime currentCentralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, centralZone);


            TwilioResponse response = new TwilioResponse();

            double startHour = 9;
            double endHour = 18;

            if (ConfigurationManager.AppSettings.AllKeys.Contains("StartHour"))
            {
                double.TryParse(ConfigurationManager.AppSettings["StartHour"], out startHour);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("EndHour"))
            {
                double.TryParse(ConfigurationManager.AppSettings["EndHour"], out endHour);
            }

            string[] phoneNumbers = new string[0];
            if (ConfigurationManager.AppSettings.AllKeys.Contains("PhoneNumbers"))
            {
                phoneNumbers = ConfigurationManager.AppSettings["PhoneNumbers"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if ((Weekdays.Contains(currentCentralTime.DayOfWeek)) && (currentCentralTime > currentCentralTime.Date.AddHours(startHour)) && (currentCentralTime < currentCentralTime.Date.AddHours(endHour)))
            {
                response.Play("/Dtmf-9.wav");
                response.Pause(1);
                response.Hangup();
            }
            else if(phoneNumbers.Any())
            {
                response.DialNumbers(phoneNumbers);  
            }
            else
            {
                response.Say("Access Denied");
            }
            return new TwiMLResult(response);
        }

    }
}
