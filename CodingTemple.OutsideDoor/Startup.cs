using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twilio.TwiML;

namespace CodingTemple.OutsideDoor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DayOfWeek[] days =
                Configuration.GetValue<string>("Days", "1, 2, 3, 4, 5, 6").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (DayOfWeek)int.Parse(x)).ToArray();
            //new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
          
            double startHour = Configuration.GetValue<double>("StartHour", 9);
            double endHour = Configuration.GetValue<double>("EndHour", 18);
            
            string[] phoneNumbers = Configuration.GetValue<string>("PhoneNumbers", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            app.Run(async (context) =>
            {
                VoiceResponse response = new VoiceResponse();
                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime currentCentralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, centralZone);
                if ((days.Contains(currentCentralTime.DayOfWeek)) && (currentCentralTime > currentCentralTime.Date.AddHours(startHour)) && (currentCentralTime < currentCentralTime.Date.AddHours(endHour)))
                {
                    response.Play(null, null, "ww9ww9ww9");
                    response.Hangup();
                }
                else if (phoneNumbers.Any())
                {
                    phoneNumbers.ToList().ForEach(x => { response.Dial(x); });
                }
                else
                {
                    response.Say("Access Denied");
                }
                context.Response.ContentType = "text/xml";
                await context.Response.WriteAsync(response.ToString());
            });
        }
    }
}
