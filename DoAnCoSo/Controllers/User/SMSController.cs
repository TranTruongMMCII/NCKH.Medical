using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace StoreASP.Controllers
{
    public class SMSController : Controller
    {
        const string APIKey = "DB414C9BA1141848750819249A7B94";
        const string SecretKey = "8B24FA9EAF85BD8307A2A0A07FF215";

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Send(string phone, string message)
        {
            string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V2/";
            UTF8Encoding encoding = new UTF8Encoding();
            string strResult = string.Empty;
            string customers = "";
            phone = Session["SDT"].ToString();
            message = Session["Message"].ToString();

            string[] lstPhone = phone.Split(',');

            for (int i = 0; i < lstPhone.Count(); i++)
            {
                customers = customers + @"<CUSTOMER>"
                                + "<PHONE>" + lstPhone[i] + "</PHONE>"
                                + "</CUSTOMER>";
            }


            string SampleXml = @"<RQST>"
                               + "<APIKEY>" + APIKey + "</APIKEY>"
                               + "<SECRETKEY>" + SecretKey + "</SECRETKEY>"
                               + "<ISFLASH>0</ISFLASH>"
                               + "<SMSTYPE>7</SMSTYPE>"//SMSTYPE 3: đầu số ngẫu nhiên tốc độ chậm, SMSTYPE=7: đầu số ngẫu nhiên tốc độ cao, SMSTYPE=4: Đầu số 19001534; SMSTYpe=6: đàu số 8755                               
                               + "<CONTENT>" + message + "</CONTENT>"
                               + "<CONTACTS>" + customers + "</CONTACTS>"


           + "</RQST>";
            //Tham khao them ve SMSTYPE tai day: http://esms.vn/SMSApi/ApiSendSMSNormal


            string postData = SampleXml.Trim().ToString();
            // convert xmlstring to byte using ascii encoding
            byte[] data = encoding.GetBytes(postData);
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            // set method as post
            webrequest.Method = "POST";
            webrequest.Timeout = 500000;
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
            // set content length
            webrequest.ContentLength = data.Length;
            // get stream data out of webrequest object
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

            // set utf8 encoding
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // read response stream from response object
            StreamReader loResponseStream =
                new StreamReader(webresponse.GetResponseStream(), enc);
            // read string from stream data
            strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();
            // below steps remove unwanted data from response string
            strResult = strResult.Replace("</string>", "");

            return RedirectToAction("ThanhToan", "Booking");
        }
        public ActionResult Send2(string phone, string message)
        {
            string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V2/";
            UTF8Encoding encoding = new UTF8Encoding();
            string strResult = string.Empty;
            string customers = "";
            phone = Session["SDT2"].ToString();
            message = Session["Message2"].ToString();

            string[] lstPhone = phone.Split(',');

            for (int i = 0; i < lstPhone.Count(); i++)
            {
                customers = customers + @"<CUSTOMER>"
                                + "<PHONE>" + lstPhone[i] + "</PHONE>"
                                + "</CUSTOMER>";
            }


            string SampleXml = @"<RQST>"
                               + "<APIKEY>" + APIKey + "</APIKEY>"
                               + "<SECRETKEY>" + SecretKey + "</SECRETKEY>"
                               + "<ISFLASH>0</ISFLASH>"
                               + "<SMSTYPE>7</SMSTYPE>"//SMSTYPE 3: đầu số ngẫu nhiên tốc độ chậm, SMSTYPE=7: đầu số ngẫu nhiên tốc độ cao, SMSTYPE=4: Đầu số 19001534; SMSTYpe=6: đàu số 8755                               
                               + "<CONTENT>" + message + "</CONTENT>"
                               + "<CONTACTS>" + customers + "</CONTACTS>"


           + "</RQST>";
            //Tham khao them ve SMSTYPE tai day: http://esms.vn/SMSApi/ApiSendSMSNormal


            string postData = SampleXml.Trim().ToString();
            // convert xmlstring to byte using ascii encoding
            byte[] data = encoding.GetBytes(postData);
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            // set method as post
            webrequest.Method = "POST";
            webrequest.Timeout = 500000;
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
            // set content length
            webrequest.ContentLength = data.Length;
            // get stream data out of webrequest object
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

            // set utf8 encoding
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // read response stream from response object
            StreamReader loResponseStream =
                new StreamReader(webresponse.GetResponseStream(), enc);
            // read string from stream data
            strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();
            // below steps remove unwanted data from response string
            strResult = strResult.Replace("</string>", "");

            return RedirectToAction("ResultManager", "Admin");
        }
    }
}
