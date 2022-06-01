using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace Api.Core.Infrastructure
{
    public static class Helper
    {
        public static string EncodePassword(Algorithm hashAlgorithmType, string clearText)
        {
            switch (hashAlgorithmType)
            {
                case Algorithm.Md5:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Md5);

                case Algorithm.Sha1:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha1);

                case Algorithm.Sha256:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha256);

                case Algorithm.Sha384:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha384);

                case Algorithm.Sha512:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha512);

                default: throw new Exception("Method not implemented");
            }
        }

        public static string CreateRandomPassword(int passwordLength)
        {
            string str = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
            byte[] data = new byte[passwordLength];
            new RNGCryptoServiceProvider().GetBytes(data);
            char[] chArray = new char[passwordLength];
            int length = str.Length;
            for (int i = 0; i < passwordLength; i++)
            {
                chArray[i] = str[data[i] % length];
            }
            return new string(chArray);
        }

        public static bool SendEmail(string from, string to, string subject, string body)
        {
            MailMessage message = new MailMessage(from, to, subject, body)
            {
                Priority = MailPriority.High,
                IsBodyHtml = true
            };
            SmtpClient client = new SmtpClient();
            try
            {
                client.Send(message);
            }
            catch (Exception exception)
            {
                throw new SmtpException(exception.Message);
            }
            return true;
        }


        public static T DeserializeNode<T>(string xml) where T : class
        {
            string xmlr;

            if (xml.Contains("utf-16"))
            {
                xmlr = xml.Replace("utf-16", "utf-8");
            }
            else
            {
                xmlr = xml;
            }

            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm, encoding: Encoding.UTF8);
            stw.Write(xmlr);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result = ser.Deserialize(stm) as T;

            return result;
        }

        public static string RemoveAcents(this string helper)
        {
            var characters = new Dictionary<char, char>() { { 'á', 'a' },
                                                            { 'é', 'e' },
                                                            { 'í', 'i' },
                                                            { 'ó', 'o' },
                                                            { 'ú', 'u' },
                                                            { 'Á', 'A' },
                                                            { 'É', 'E' },
                                                            { 'Í', 'I' },
                                                            { 'Ó', 'O' },
                                                            { 'Ú', 'U' }};

            foreach (var character in characters)
                helper = helper.Replace(character.Key, character.Value);


            return helper;
        }

        public static Dictionary<String, DateTime> StartAndEndDates(DateTime date) 
        {
            Dictionary<String, DateTime> dates = new Dictionary<string, DateTime>();
            DateTime baseDate = date;

            var today = baseDate;
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);

            dates.Add("yesterday", yesterday);
            dates.Add("thisWeekStart", thisWeekStart);
            dates.Add("thisWeekEnd", thisWeekEnd);
            dates.Add("lastWeekStart", lastWeekStart);
            dates.Add("lastWeekEnd", lastWeekEnd);
            dates.Add("thisMonthStart", thisMonthStart);
            dates.Add("thisMonthEnd", thisMonthEnd);
            dates.Add("lastMonthStart", lastMonthStart);
            dates.Add("lastMonthEnd", lastMonthEnd);

            return dates;
        }

    }
}
