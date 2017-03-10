using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColossalCave.Engine.AssetModels;

namespace ColossalCave.Engine.Interfaces
{
    public interface IMessageProvider
    {
        Message GetMessage(int id);
    }
}
