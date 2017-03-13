using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class LookAroundHandler : BaseHandler, ILookAroundHandler
    {
        public LookAroundHandler(ILogger<LookAroundHandler> log,
           IResponseBuilder responseBuilder,
           IMessageProvider messageProvider,
           ILocationProvider locationProvider,
           IAdventureContextHelper advHelper,
           IMapHelper mapHelper,
           AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, advHelper, mapHelper)
        { }
            
        public override void Handle()
        {
            _log.LogInformation("Handling LookAround");

            // Can't see in the dark.
            if (!_advHelper.IsCurrentLocationLight)
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.MovePitchDark, 1);
                return;
            }

            var curLoc = _advHelper.CurrentLocation;
            var lookStr = _advHelper.GetParameterValue("visuals");
            var directionStr = _advHelper.GetParameterValue("directions");
            if (directionStr != null)
            {
                _responseBuilder.AddToResponse(MsgMnemonic.CantGiveMoreDetail, 1);
                _responseBuilder.AddToResponse(curLoc.Description);
            }
            else
            {
                _responseBuilder.AddToResponse(curLoc.Description);
                _mapHelper.EnumerateItemsHere();
            }
        }
    }
}
