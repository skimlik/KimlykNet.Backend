using System;

namespace KimlykNet.Services.Abstractions.Configuration;

public class NotificationsSettings
{
    public static readonly string SectionName = "Notifications";

    public Uri WebHookUri { get; set; }   
}