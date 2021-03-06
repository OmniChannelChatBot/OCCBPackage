﻿using System;

namespace OCCBPackage.Options
{
    public class RefreshTokenOptions
    {
        public const string TokenName = "refresh_token";

        public DateTimeOffset Expires => DateTimeOffset.UtcNow.AddDays(ExpiresInDays);

        public double ExpiresInDays { get; set; } = 5.0;
    }
}
