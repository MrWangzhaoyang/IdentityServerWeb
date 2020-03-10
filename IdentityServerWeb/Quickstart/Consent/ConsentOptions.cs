// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServerWeb.Quickstart.UI
{
    public class ConsentOptions
    {
        public static bool EnableOfflineAccess = true;
        public static string OfflineAccessDisplayName = "脱机访问网络资源";
        public static string OfflineAccessDescription = "访问您的应用程序和资源，即使您是离线的";

        public static readonly string MustChooseOneErrorMessage = "您必须选择至少一个权限";
        public static readonly string InvalidSelectionErrorMessage = "无效的选择";
    }
}
