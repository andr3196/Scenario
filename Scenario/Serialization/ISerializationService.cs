﻿using System;
namespace Scenario.Domain
{
    public interface ISerializationService
    {
        string Serialize<TType>(TType type);

        TType? Deserialize<TType>(string type);
    }
}
