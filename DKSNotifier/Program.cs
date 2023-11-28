﻿using System;
using System.Configuration;

namespace DKSNotifier
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // настройки программы
            ConfigurationStorage configurationStorage = new ConfigurationStorage(
                
                // код НО
                codeNO: ConfigurationManagerReader.AppSettingRead("CodeNO", "0000"),
                
                // настройка по уволенным
                dismissalCheck: ConfigurationManagerReader.AppSettingRead("CheckDismissial", true),
                dismissalCountDays: ConfigurationManagerReader.AppSettingRead("DismissalCountDays", 4),
                dismissalOrdType: ConfigurationManagerReader.AppSettingRead("DismissalOrdType"),

                // настройка по перемещению
                movingCheck: ConfigurationManagerReader.AppSettingRead("MovingCheck", true),
                movingCountDays: ConfigurationManagerReader.AppSettingRead("MovingCountDays", 4),
                movingOrdType: ConfigurationManagerReader.AppSettingRead("MovingOrdType"),

                // настройка по отпускам
                vacationCheck: ConfigurationManagerReader.AppSettingRead("VacationCheck", true),
                vacationCountDays: ConfigurationManagerReader.AppSettingRead("VacationCountDays", 4),
                vacationOrdType: ConfigurationManagerReader.AppSettingRead("VacationOrdType"),
                vacationTypeCode: ConfigurationManagerReader.AppSettingRead("VacationTypeCode"),

                // настройка почтового сервера
                emailServerName: ConfigurationManagerReader.AppSettingRead("EmailServerName", ""),
                emailServerPort: ConfigurationManagerReader.AppSettingRead("EmailServerPort", 25),
                emailFrom: ConfigurationManagerReader.AppSettingRead("EmailFrom", ""), 
                emailTo: ConfigurationManagerReader.AppSettingRead("EmailTo", ""),
                emailSend: ConfigurationManagerReader.AppSettingRead("EmailSend", true),

                // настройка выгрузки html-файла
                useOutFile: ConfigurationManagerReader.AppSettingRead("UseOutFile", true),
                dirOut: ConfigurationManagerReader.AppSettingRead("DirOut", ".\\HtmlOut"),

                // настройка подключения к MS SQL Server
                sqlConnectionString: ConfigurationManagerReader.ConnectionStringRead("Mssql")           
            );
                 
            // путь к xml-файлу для сохранения информации о полученных сведениях (для исключения повторного направления/сохранения информации)            
            string xmlBaseFile = AppDomain.CurrentDomain.BaseDirectory + "base.xml";
            
            // запуск приложения
            new AppStarter(configurationStorage, xmlBaseFile).Run();
        }       

    }
}
